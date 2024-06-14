using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;

public class CsvReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    //static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    // 파일명 순으로 입력해 주세요
    public static string[] tableData =
        {
        "TableData/Randombox",
        "TableDataEx/Text_ui",
        };
    public static string tableEnumData = "TableDataEx/TableEnum";
    public static string configTable = "TableData/Config";


    public static void LoadLocalCsvTest()
    {
        string fileStr;
        foreach (var item in tableData)
        {
            fileStr = File.ReadAllText(Path.Combine(GameConfig.LocalTableFolderPath, string.Format("{0}.csv", item)));
            ResourceManager.Instance.AddTableAsset(item, fileStr);
        }

        fileStr = File.ReadAllText(Path.Combine(GameConfig.LocalTableFolderPath, string.Format("{0}.csv", configTable)));
        ResourceManager.Instance.AddTableAsset(configTable, fileStr);
    }

    public static void ParseDataTable()
    {
        foreach (var item in tableData)
        {
            string tableName = Path.GetFileName(item);
            try
            {
                var rowList = ParseCSV3(string.Format("{0}", item));
                //Debug.Log($"ParseDataTable {tableName} Start");
                Type type = typeof(Datatable);
                MethodInfo method = type.GetMethod(string.Format("Load{0}", tableName));
                method.Invoke(Datatable.Instance, new object[] { rowList });
                //Debug.Log($"ParseDataTable {tableName} End");
            }
            catch
            {
                Debug.LogError($"load Filed tableName {tableName}");
            }
        }
        ParseConfigTable();
        Datatable.Instance.MakeClientDT();
    }

    /// <summary>
    /// 내장 리소스 폴더에서 로딩하는 스트링 테이블<br></br>
    /// Localization.GetBuiltInUIString(키값) 함수로 사용
    /// </summary>
    public static void LoadBultInText(string _tableName)
    {
        //var rowList = ParseCSV3(_tableName);
        //_Datatable.Instance.LoadBuiltInText_ui(rowList);
    }

    public static void LoadUIText()
    {
        var rowList = ParseCSV3("TableDataEx/Text_ui");   
        Datatable.Instance.LoadText_ui(rowList);
    }
    public static void ParseConfigTable()
    {
        //var rowDic = ParseConfigTable(configTable);
        //ConfigTable.Instance.LoadConfig(rowDic);
    }

    private static void ParseCSV()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/Test/testCSV.csv");
        bool endOfFile = false;
        while (!endOfFile)
        {
            string data = sr.ReadLine();
            if (data == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data.Split(',');
            for (int i = 0; i < data_values.Length; i++)
            {
                Debug.Log("v : " + i.ToString() + " " + data_values[i].ToString());
            }
        }

    }

    private static void ParseCSV2()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/Test/testCSV.csv");
        bool endOfFile = false;
        while (!endOfFile)
        {
            string data = sr.ReadLine();
            if (data == null)
            {
                endOfFile = true;
                break;
            }

            List<string> strList = SeperateStrWithComma(data);

            foreach (var item in strList)
            {
                Debug.Log("item : " + item);
            }
        }
    }

    public static List<Dictionary<string, object>> GetConfigTableData(string path)
    {
        var rowList = new List<Dictionary<string, object>>();

        Debug.Log($"GetConfigTableData path : {path}");
        string fileStr = File.ReadAllText(Path.Combine(GameConfig.LocalTableFolderPath, string.Format("{0}.csv", path))); 
        StringReader sr = new StringReader(fileStr);

        string readData = sr.ReadLine();
        sr.ReadLine();
        var header = Regex.Split(readData, SPLIT_RE);

        bool endOfFile = false;

        while (!endOfFile)
        {
            string data = sr.ReadLine();
            if (data == null)
            {
                endOfFile = true;
                break;
            }
            var values = data.Split(',');
            if (values.Length == 0 || values[0] == "")
                continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                value = value.Replace("<br>", "\n");
                value = value.Replace("<c>", "c");
                //Debug.Log("value " + value);
                entry[header[j]] = value;
            }
            rowList.Add(entry);
        }

        return rowList;
    }

    public static List<Dictionary<string, object>> GetTableEnum(string path)
    {
        Debug.Log($"GetTableSchema path : {path}");
        string fileStr = File.ReadAllText(Path.Combine(GameConfig.LocalTableFolderPath, string.Format("{0}.csv", path)));
        StringReader sr = new StringReader(fileStr);

        string readData = sr.ReadLine();
        var header = Regex.Split(readData, SPLIT_RE);
        //readData = sr.ReadLine();
        //var valueType = Regex.Split(readData, SPLIT_RE);

        //List<string> rowItems = new List<string>();
        //for (var i = 0; i < header.Length; i++)
        //{
        //    Debug.Log("item " + valueType[i]);
        //    rowItems.Add(valueType[i]);
        //}

        var rowList = new List<Dictionary<string, object>>();
        bool endOfFile = false;

        while (!endOfFile)
        {
            string data = sr.ReadLine();
            if (data == null)
            {
                endOfFile = true;
                break;
            }
            //var values = Regex.Split(data, SPLIT_RE);
            var values = data.Split(',');
            if (values.Length == 0 || values[0] == "")
                continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                value = value.Replace("<br>", "\n");
                value = value.Replace("<c>", "c");
                //Debug.Log("value " + value);
                entry[header[j]] = value;
            }
            rowList.Add(entry);

        }
        return rowList;

    }

    public static Dictionary<string, string> GetTableSchema(string path)
    {
        Dictionary<string, string> retDic = new Dictionary<string, string>();
        string fileStr;

#if CsvTest
        if (Application.isPlaying)
        {
            fileStr = ResourceManager.Instance.GetTableAsset(path);
        }
        else
        {
            fileStr = File.ReadAllText(Path.Combine(GameConfig.LocalTableFolderPath, string.Format("{0}.csv", path)));
            TextAsset _txtFile = (TextAsset)Resources.Load(path) as TextAsset;
        }
#else
        fileStr = File.ReadAllText(Path.Combine(GameConfig.LocalTableFolderPath, string.Format("{0}.csv", path)));
#endif

        StringReader sr = new StringReader(fileStr);

        string readData = sr.ReadLine();
        var header = Regex.Split(readData, SPLIT_RE);   
        header = header.Where(item => !string.IsNullOrEmpty(item)).ToArray();

        //return File.ReadAllText(Path.Combine(mBundleDirectory, mDicInZipFilePath[string.Format("{0}.txt", _path)]));    //TODO : 중복로딩 방지 검토
        readData = sr.ReadLine();
        var valueType = Regex.Split(readData, SPLIT_RE);
        for (var i = 0; i < header.Length; i++)
        {
            if(valueType[i] == "text" || valueType[i] == "char")
                retDic.Add(header[i], "string");
            else if(valueType[i] == "int" || valueType[i] == "float" || valueType[i] == "string")
                retDic.Add(header[i], valueType[i]);
            else
                retDic.Add(header[i], valueType[i].ToUpper());
        }

        return retDic;
    }

    private static Dictionary<string, Dictionary<string, object>> ParseConfigTable(string path)
    {
        var rowDic = new Dictionary<string, Dictionary<string, object>>();
           
        string str = ResourceManager.Instance.GetTableAsset(path);
        StringReader sr = new StringReader(str);
        bool endOfFile = false;

        string readData = sr.ReadLine();
        if (readData == null)
        {
            Debug.Log("No Data");
            return null;
        }
        sr.ReadLine();

        var header = Regex.Split(readData, SPLIT_RE);
        while (!endOfFile)
        {
            string data = sr.ReadLine();
            if (data == null)
            {
                endOfFile = true;
                break;
            }
            //var values = Regex.Split(data, SPLIT_RE);
            var values = data.Split(',');
            if (values.Length == 0 || values[0] == "")
                continue;

            var entry = new Dictionary<string, object>();
            
            object finalValue = values[2];
            int n;
            float f;
            long l;
            switch (values[1])
            {
                case "int":
                    if (int.TryParse(values[2], out n))
                        finalValue = n;
                    break;
                case "float":
                    if (float.TryParse(values[2], out f))
                        finalValue = f;
                    break;
                case "long":
                    if (long.TryParse(values[2], out l))
                        finalValue = l;
                    break;
                default:
                    if (int.TryParse(values[2], out n))
                        finalValue = n;
                    break;
            }
            entry["index"] = values[0];
            entry["value"] = finalValue;
            rowDic[(string)entry["index"]] = entry;
        }
        return rowDic;
    }

    // 현재 사용하는 파서
    private static List<Dictionary<string, object>> ParseCSV3(string path)
    {
        var schemaDic = GetTableSchema(path);

        var rowList = new List<Dictionary<string, object>>();
        string fileStr = ResourceManager.Instance.GetTableAsset(path);
        StringReader sr = new StringReader(fileStr);
        //StreamReader sr = new StreamReader(string.Format("{0}/{1}", Application.dataPath, path));
        bool endOfFile = false;
        
        string readData = sr.ReadLine();
        if (readData == null)
        {
            Debug.Log("No Data");
            return null;
        }
        sr.ReadLine();
        //sr.ReadLine();

        var header = Regex.Split(readData, SPLIT_RE);   
        header = header.Where(item => !string.IsNullOrEmpty(item)).ToArray();

        while (!endOfFile)
        {
            string data = sr.ReadLine();

            try
            {
                if (data == null)
                {
                    endOfFile = true;
                    break;
                }
                //var values = Regex.Split(data, SPLIT_RE);
                var values = data.Split(',');
                if (values.Length == 0 || values[0] == "")
                    continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    value = value.Replace("<br>", "\n");
                    value = value.Replace("<c>", "c");

                    object finalValue = value;
                    int n;
                    float f;
                    long l;
                    try
                    {
                        switch (schemaDic[header[j]])
                        {
                            case "int":
                                if (int.TryParse(value, out n))
                                    finalValue = n;
                                break;
                            case "float":
                                if (float.TryParse(value, out f))
                                    finalValue = f;
                                break;
                            case "long":
                                if (long.TryParse(value, out l))
                                    finalValue = l;
                                break;
                            case "char":
                            case "text":
                            case "string":
                                break;
                            default:
                                if (int.TryParse(value, out n))
                                    finalValue = n;
                                break;
                        }
                        entry[header[j]] = finalValue;
                    }
                    catch
                    {
                        Debug.LogError($"parse failed {value}");
                    }
                }
                rowList.Add(entry);
            }
            catch {
                Debug.LogError($"readLine failed {data}");
            }
        }
        return rowList;
    }

    private static List<string> SeperateStrWithComma(string value)
    {
        
        bool inQuotes = false;
        char delimeter = ',';
        List<string> strings = new List<string>();

        StringBuilder sb = new StringBuilder();
        foreach (char c in value)
        {
            if(c=='\'' || c == '"')
            {
                if (!inQuotes)
                    inQuotes = true;
                else
                    inQuotes = false;
            }

            if (c == delimeter)
            {
                if (!inQuotes)
                {
                    strings.Add(sb.Replace("'", string.Empty).Replace("\"", string.Empty).ToString());
                    sb.Remove(0, sb.Length);
                }
                else
                {
                    sb.Append(c);
                }
            }
            else
            {
                sb.Append(c);
            }
        }

        strings.Add(sb.Replace("'", string.Empty).Replace("\"", string.Empty).ToString());
        return strings;
    }
}


