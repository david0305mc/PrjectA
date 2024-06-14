using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public class CodeGenerator2
{
    static string DATATABLE_DEF_PATH = "Assets/Test/CsvTest/DatatableBind.cs";

    public static void GenDatatable()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("#pragma warning disable 114\n");
        sb.Append("using System;\n");
        sb.Append("using System.Collections;\n");
        sb.Append("using System.Collections.Generic;\n");

        sb.AppendLine("public partial class DataManagerTest {");

        GenTableData(sb);
        sb.Append("};");
        WriteCode(DATATABLE_DEF_PATH, sb.ToString());
    }
    
    private static void GenTableData(StringBuilder sb)
    {
        PropertyInfo[] props = typeof(DataManagerTest).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < props.Length; ++i)
        {
            string propName = props[i].Name;
            Debug.Log(propName);
            if (!propName.StartsWith(Datatable.DicDataTablePrefix))
                continue;
            string className = propName.Replace(Datatable.DicDataTablePrefix, Datatable.ClassPrefix);

            object[] attrs = props[i].GetCustomAttributes(typeof(TableAttribute), false);
            string tableName = attrs.Length != 0 ? ((TableAttribute)attrs[0]).name : props[i].Name.Replace(Datatable.DataTablePrefix, string.Empty);

            var properties = props[i].PropertyType;
            var arg = properties.GetGenericArguments();
            
            if (arg.Length > 1)
            {
                var targetField = Type.GetType(arg[1].Name).GetFields();
                
                var indexName = targetField[0].Name;

                sb.AppendLine($"\tpublic void BindData{tableName}(string csv)");
                sb.AppendLine("\t{");
                sb.AppendLine($"\t\t var param = CSVDeserialize(csv, typeof({className}));");

                sb.AppendLine($"\t\t foreach (var item in (Array)param)");
                sb.AppendLine("\t\t {");
                sb.AppendLine($"\t\t\t {className} data = ({className})item;");
                sb.AppendLine($"\t\t\t {propName}[data.{indexName}] = data;");
                sb.AppendLine("\t\t }");
                sb.AppendLine("\t}");


                //sb.AppendLine($"\tpublic {className} Get{className}({targetField[0].GetType()} id)");
                //sb.AppendLine("\t{");


                //sb.AppendLine("\t}");


                //public RefFishUnlockCondition GetAbyss_fish_unlockcondition(int id)
                //{
                //    if (dicFishUnlockCondition.TryGetValue(id, out var data))
                //    {
                //        return data;
                //    }
                //    return default;
                //}

                //public RefFishUnlockCondition GetAbyss_fish_unlockcondition(int id)
                //{
                //    if (dicFishUnlockCondition.TryGetValue(id, out var data))
                //    {
                //        return data;
                //    }
                //    return default;
                //}

            }
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
