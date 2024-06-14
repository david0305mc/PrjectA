using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CodeGenerator
{
    static string DATATABLE_DEF_PATH = "Assets/02_Scripts/Data/Datatable.cs";
    static string TABLE_ENUM_DEF_PATH = "Assets/02_Scripts/Data/TableEnum.cs";
    static string CONFIG_TABLE_DEF_PATH = "Assets/02_Scripts/Data/ConfigTable.cs";

    //static string[] DT_BLACK_LIST = new string[]
    //{
    //    "dt/TypeList.json"
    //};

    public static void GenDatatable()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("#pragma warning disable 114\n");
        sb.Append("using System;\n");
        sb.Append("using System.Collections;\n");
        sb.Append("using System.Collections.Generic;\n");

        sb.AppendLine("public partial class Datatable {");
        sb.AppendLine("private static readonly Lazy<Datatable> _instance = new Lazy<Datatable>(() => new Datatable());");
        sb.AppendLine("public static Datatable Instance { get { return _instance.Value; } }");
        GenTableData(sb);

        sb.Append("};");
        WriteCode(DATATABLE_DEF_PATH, sb.ToString());
    }

    public static void GenConfigTable()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("#pragma warning disable 114\n");
        sb.Append("using System;\n");
        sb.Append("using System.Collections;\n");
        sb.Append("using System.Collections.Generic;\n");
        sb.Append("public class ConfigTable {\n");
        sb.AppendLine("private static readonly Lazy<ConfigTable> _instance = new Lazy<ConfigTable>(() => new ConfigTable());");
        sb.AppendLine("public static ConfigTable Instance { get { return _instance.Value; } }");
        GenConfigTableData(sb);
        sb.Append("};");
        
        WriteCode(CONFIG_TABLE_DEF_PATH, sb.ToString());
    }

    public static void GenTableEnum()
    {
        StringBuilder sb = new StringBuilder();
        GenTableEnum(sb);
        WriteCode(TABLE_ENUM_DEF_PATH, sb.ToString());
    }
    private static void GenTableEnum(StringBuilder sb)
    {
        var enumLIst = CsvReader.GetTableEnum(string.Format("{0}", CsvReader.tableEnumData));
        HashSet<string> keySet = new HashSet<string>();

        foreach (var item in enumLIst)
        {
            string enumType = item["type"].ToString().ToUpper();
            if (!keySet.Contains(enumType))
            {
                if (keySet.Count > 0)
                    sb.AppendFormat("}}\n");
                sb.AppendFormat("public enum {0} \n{{ \n", enumType);
                keySet.Add(enumType);
            }
            
            sb.AppendFormat("\t{0, -28} = {1, -10}", item["value"].ToString().ToUpper(), item["number"] + ",");
            sb.AppendFormat("\t// {0}", item["desc1"]);
            sb.AppendLine();
        }
        sb.AppendFormat("}}");
    }

    public static void GenConfigTableData(StringBuilder sb)
    {
        sb.AppendLine();
        var configTableDic = CsvReader.GetConfigTableData(CsvReader.configTable);

        foreach (var item in configTableDic)
        {
            sb.AppendLine($"\tpublic {item["type"]} {item["index"]};");
        }
        sb.AppendLine("\tpublic void LoadConfig(Dictionary<string, Dictionary<string, object>> rowList)");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tforeach (var rowItem in rowList)");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t\tvar field = typeof(ConfigTable).GetField(rowItem.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);");
        sb.AppendLine("\t\t\tfield.SetValue(this, rowItem.Value[\"value\"]);");
        sb.AppendLine("\t\t}");
        sb.AppendLine("\t}");
    }
    public static void GenTableData(StringBuilder sb)
    {
        foreach (var tablePath in CsvReader.tableData)
        {
            string tableName = Path.GetFileName(tablePath);

            var schemaDic = CsvReader.GetTableSchema(string.Format("{0}", tablePath));

            sb.AppendLine();
            sb.AppendFormat("\tpublic class {0} {{\n", tableName);

            foreach (var item in schemaDic)
            {
                sb.AppendFormat("\t\tpublic {0} {1};\n", item.Value, item.Key.ToLower());
            }
            sb.Append("\t};\n");

            sb.AppendFormat("\tpublic Dictionary<{1}, {0}> dt{0} = new Dictionary<{1}, {0}>();\n", tableName, schemaDic["index"]);
            sb.AppendFormat("\tpublic void Load{0}(List<Dictionary<string, object>> rowList) {{\n", tableName);

            sb.AppendFormat("\t\tdt{0} = new Dictionary<{1}, {0}>();\n", tableName, schemaDic["index"]);
            sb.AppendFormat("\t\tforeach (var rowItem in rowList) {{\n");

            sb.AppendFormat("\t\t\t{0} dicItem = new {0}();\n", tableName);
            sb.AppendFormat("\t\t\tforeach (var item in rowItem) {{\n");

            sb.AppendFormat("\t\t\t\tvar field = typeof({0}).GetField(item.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);\n", tableName);
            sb.AppendFormat("\t\t\t\ttry {{ field.SetValue(dicItem, item.Value); }}\n");
            sb.AppendFormat("\t\t\t\tcatch {{ UnityEngine.Debug.LogError(item); }}\n");
            sb.AppendFormat("\t\t\t}}\n");

            sb.AppendFormat("\t\t\tif (dt{0}.ContainsKey(dicItem.index)) {{\n", tableName);
            sb.AppendFormat("\t\t\t\tUnityEngine.Debug.LogError(\"Duplicate Key in {0}\");\n", tableName);
            sb.Append("\t\t\t\tUnityEngine.Debug.LogError(string.Format(\"Duplicate Key {0}\", dicItem.index));\n");
            sb.AppendFormat("\t\t\t}}\n");
            sb.AppendFormat("\t\t\tdt{0}.Add(dicItem.index, dicItem);\n", tableName);
            sb.AppendFormat("\t\t}}\n");
            sb.AppendFormat("\t}}\n");

            sb.AppendFormat("\tpublic {0} Get{0}Data({1} _index) {{\n", tableName, schemaDic["index"]);
            sb.AppendFormat("\t\tif (!dt{0}.ContainsKey(_index)){{\n", tableName);
            sb.AppendFormat("\t\t\tUnityEngine.Debug.LogError(\"Table {0}\");\n", tableName);
            sb.Append("\t\t\tUnityEngine.Debug.LogError(string.Format(\"table doesn't contain id {0}\", _index));\n");
            sb.AppendFormat("\t\t\treturn null;\n");
            sb.AppendFormat("\t\t}}\n");
            sb.AppendFormat("\t\treturn dt{0}[_index];\n", tableName);
            sb.AppendFormat("\t}}\n");

            sb.AppendFormat("\tpublic Dictionary<{1}, {0}> Get{0}Data() {{\n", tableName, schemaDic["index"]);
            sb.AppendFormat("\t\treturn dt{0};\n", tableName);
            sb.AppendFormat("\t}}\n");

            sb.AppendLine();
        }
    }
  
    public static void WriteCode(string filePath, string content)
    {
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            try
            {
                writer.WriteLine("{0}", content);
                Debug.LogWarningFormat("File {0} generated", filePath);
            }
            catch (System.Exception ex)
            {
                string msg = " threw:\n" + ex.ToString();
                Debug.LogError(msg);
            }
        }
    }

}