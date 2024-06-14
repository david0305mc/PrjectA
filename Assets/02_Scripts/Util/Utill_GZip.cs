using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO.Compression;
using System.IO;
using System.Text;

public partial class Utill
{
    public static string Compress(string str)
    {
        byte[] data = Encoding.UTF8.GetBytes(str);
        using (MemoryStream ms = new MemoryStream())
        {
            using (GZipStream gs = new GZipStream(ms, CompressionMode.Compress))
            { gs.Write(data, 0, data.Length); }

            return System.Convert.ToBase64String(ms.ToArray());
        }
    }
    public static byte[] Compress(byte[] bytes)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (GZipStream gs = new GZipStream(ms, CompressionMode.Compress))
            { gs.Write(bytes, 0, bytes.Length); }

            return ms.ToArray();
        }
    }
    public static string Compress_string(byte[] bytes)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (GZipStream gs = new GZipStream(ms, CompressionMode.Compress))
            { gs.Write(bytes, 0, bytes.Length); }

            return System.Convert.ToBase64String(ms.ToArray());
        }
    }

    public static string Decompress(string str)
    {
        byte[] data = System.Convert.FromBase64String(str);

        using (MemoryStream ms = new MemoryStream(data))
        {
            using (GZipStream gs = new GZipStream(ms, CompressionMode.Decompress))
            {
                using (StreamReader reader = new StreamReader(gs))
                {
                    return reader.ReadToEnd();
                }

                //return Encoding.UTF8.GetString(gs)
            }
            //return Encoding.Unicode.GetString(ms.ToArray());
        }
    }
    public static byte[] Decompress_Byte(string str)
    {
        byte[] data = System.Convert.FromBase64String(str);

        using (MemoryStream ms = new MemoryStream(data))
        {
            using (GZipStream gs = new GZipStream(ms, CompressionMode.Decompress))
            {

                return ms.ToArray();
                //using (StreamReader reader = new StreamReader(gs))
                //{
                //    return reader.ReadToEnd();
                //}

                //return Encoding.UTF8.GetString(gs)
            }
            //return Encoding.Unicode.GetString(ms.ToArray());
        }
    }
    public static byte[] Decompress(byte[] bytes)
    {
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            using (GZipStream gs = new GZipStream(ms, CompressionMode.Decompress))
            {
                return ms.ToArray();
                //using (StreamReader reader = new StreamReader(gs))
                //{
                //    return reader.ReadToEnd();
                //}

                //return Encoding.UTF8.GetString(gs)
            }
            //return Encoding.Unicode.GetString(ms.ToArray());
        }
    }
}
