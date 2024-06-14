using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ByteReader 
{
    byte[] mBuffer;
    int mOffset = 0;

    public ByteReader(byte[] bytes) { mBuffer = bytes; }
    public ByteReader(TextAsset asset) { mBuffer = asset.bytes; }


    static public ByteReader Open(string path)
    {
#if UNITY_EDITOR || (!UNITY_FLASH && !NETFX_CORE && !UNITY_WP8 && !UNITY_WP_8_1)
        FileStream fs = File.OpenRead(path);

        if (fs != null)
        {
            fs.Seek(0, SeekOrigin.End);
            byte[] buffer = new byte[fs.Position];
            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            return new ByteReader(buffer);
        }
#endif
        return null;
    }

    public bool canRead { get { return (mBuffer != null && mOffset < mBuffer.Length); } }

    private static string ReadLine(byte[] buffer, int start, int count)
    {
        return Encoding.UTF8.GetString(buffer, start, count);
    }

    public string ReadLine() {
        return ReadLine(true);
    }

    public string ReadLine(bool skipEmptyLines)
    {
        int max = mBuffer.Length;

        // Skip empty characters
        if (skipEmptyLines)
        {
            while (mOffset < max && mBuffer[mOffset] < 32) ++mOffset;
        }

        int end = mOffset;

        if (end < max)
        {
            for (; ; )
            {
                if (end < max)
                {
                    int ch = mBuffer[end++];
                    if (ch != '\n' && ch != '\r') continue;
                }
                else ++end;

                string line = ReadLine(mBuffer, mOffset, end - mOffset - 1);
                mOffset = end;
                return line;
            }
        }
        mOffset = max;
        return null;
    }

    public Dictionary<string, string> ReadDictionary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        char[] separator = new char[] { '=' };

        while (canRead)
        {
            string line = ReadLine();
            if (line == null)
            {
                break;
            }
            if (line.StartsWith("//"))
            {
                continue;
            }
            
            string[] split = line.Split(separator, 2, System.StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 2)
            {
                string key = split[0].Trim();
                string val = split[1].Trim().Replace("\\n", "\n");
                dict[key] = val;
            }
        }
        return dict;
    }

    static BetterList<string> mTemp = new BetterList<string>();

    /// <summary>
    /// Read a single line of Comma-Separated Values from the file.
    /// </summary>

    public BetterList<string> ReadCSV(char separator = ',')
    {
        mTemp.Clear();
        string line = "";
        bool insideQuotes = false;
        int wordStart = 0;

        while (canRead)
        {
            if (insideQuotes)
            {
                string s = ReadLine(false);
                if (s == null) return null;
                s = s.Replace("\\n", "\n");
                line += "\n" + s;
            }
            else
            {
                line = ReadLine(true);
                if (line == null) return null;
                line = line.Replace("\\n", "\n");
                wordStart = 0;
            }

            for (int i = wordStart, imax = line.Length; i < imax; ++i)
            {
                char ch = line[i];

                if (ch == separator)
                {
                    if (!insideQuotes)
                    {
                        mTemp.Add(line.Substring(wordStart, i - wordStart));
                        wordStart = i + 1;
                    }
                }
                else if (ch == '"')
                {
                    if (insideQuotes)
                    {
                        if (i + 1 >= imax)
                        {
                            mTemp.Add(line.Substring(wordStart, i - wordStart).Replace("\"\"", "\""));
                            return mTemp;
                        }

                        if (line[i + 1] != '"')
                        {
                            mTemp.Add(line.Substring(wordStart, i - wordStart).Replace("\"\"", "\""));
                            insideQuotes = false;

                            if (line[i + 1] == separator)
                            {
                                ++i;
                                wordStart = i + 1;
                            }
                        }
                        else ++i;
                    }
                    else
                    {
                        wordStart = i + 1;
                        insideQuotes = true;
                    }
                }
            }

            if (wordStart < line.Length)
            {
                if (insideQuotes) continue;
                mTemp.Add(line.Substring(wordStart, line.Length - wordStart));
            }
            return mTemp;
        }
        return null;
    }

}
