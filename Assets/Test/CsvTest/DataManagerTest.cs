using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class TableAttribute : System.Attribute
{
    public string name { get; private set; }
    public TableAttribute(string name) { this.name = name; }
}

public partial class DataManagerTest : Singleton<DataManagerTest>
{
    private Dictionary<string, PropertyInfo> _properties;
    private Dictionary<string, PropertyInfo> _dicProperties;


    //[Table("Abyss_item")]
    //public RefItem[] tblItem { get; private set; }
    //[Table("Abyss_stone")]
    //public RefStone[] tblStone { get; private set; }
    //[Table("Abyss_artifact")]
    //public RefArtifact[] tblArtifact { get; private set; }

    [Table("Abyss_item")]
    public Dictionary<int, RefItem> dicItem { get; private set; } = new Dictionary<int, RefItem>();

    [Table("Abyss_stone")]
    public RefStone[] tblStone { get; private set; }
    public void Initialize()
    {
        InitProperty();
    }
    void InitProperty()
    {
        _properties = new Dictionary<string, PropertyInfo>();
        _dicProperties = new Dictionary<string, PropertyInfo>();
        PropertyInfo[] props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < props.Length; ++i)
        {
            if (props[i].Name.StartsWith(Datatable.DataTablePrefix))
            {
                object[] attrs = props[i].GetCustomAttributes(typeof(TableAttribute), false);
                string tableName = attrs.Length != 0 ? ((TableAttribute)attrs[0]).name : props[i].Name.Replace(Datatable.DataTablePrefix, string.Empty);
                _properties.Add(tableName, props[i]);
            }

            if (props[i].Name.StartsWith(Datatable.DicDataTablePrefix))
            {
                object[] attrs = props[i].GetCustomAttributes(typeof(TableAttribute), false);
                string tableName = attrs.Length != 0 ? ((TableAttribute)attrs[0]).name : props[i].Name.Replace(Datatable.DataTablePrefix, string.Empty);
                _dicProperties.Add(tableName, props[i]);
                //_dicProperties.Add(((TableAttribute)attrs[0]).name, props[i]);
            }
        }
    }
    public void LoadDataAsync()
    {
        //var itr = _properties.GetEnumerator();
        //while (itr.MoveNext())
        //{
        //    string tableName = itr.Current.Key;
        //    SetCSVData(tableName, LoadFromFile(string.Format("{0}.csv", tableName)));
        //}

        var itr2 = _dicProperties.GetEnumerator();
        while (itr2.MoveNext())
        {
            string tableName = itr2.Current.Key;
            SetCSVData(tableName, LoadFromFile(string.Format("{0}.csv", tableName)));
        }
    }

    public void SetCSVData(string tableName, string text)
    {
        //string className = propName.Replace(DataTablePrefix, Datatable.ClassPrefix);
        MethodInfo method = GetType().GetMethod(string.Format("BindData{0}", tableName));
        method.Invoke(this, new object[] { text });
    }

    object CSVDeserialize(string text, Type type, bool hasSkipLine = true)
    {
        List<string[]> rows = CSVSerializer.ParseCSV(text, '|');
        if (hasSkipLine)
            rows.RemoveAt(1);
        return CSVSerializer.Deserialize(rows, type);
    }

    string LoadFromFile(string fileName)
    {
        string text;
        using (FileStream fs = new FileStream(string.Format("{0}/{1}/{2}", PathInfo.DataPath, ServerSettings.serverName, fileName), FileMode.Open))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                text = sr.ReadToEnd();
            }
        }
        return text;
    }
}
