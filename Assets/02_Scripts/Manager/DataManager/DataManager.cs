
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public class Table
{
	public int index;
}

public partial class DataManager : Singleton<DataManager>
{
	public static string[] tableNames =
		{
			"Unitinfo",
		};

	
	public async UniTask LoadDataAsync()
	{
		foreach (var tableName in tableNames)
		{
			var data = await LoadCSVAsync($"{tableName}.csv");
			MethodInfo method = GetType().GetMethod($"Bind{tableName}Data");
            method.Invoke(DataManager.Instance, new object[] { Type.GetType($"DataManager+{tableName}"), data });
        }
	}

	public async UniTask<string> LoadCSVAsync(string fileName)
	{
#if DEV
		return await Utill.LoadFromFileAsync($"{LOCAL_CSV_PATH}/dev/{fileName}");
#else
		
		var result = await Resources.LoadAsync<TextAsset>(Path.Combine("Data", $"{Path.GetFileNameWithoutExtension(fileName)}"));
		return ((TextAsset)result).text;
#endif

	}

	public static string LoadCSVSync(string fileName)
	{
#if DEV
		return Utill.LoadFromFile($"{LOCAL_CSV_PATH}/dev/{fileName}");
#else
		string path = Path.Combine("Data", $"{Path.GetFileNameWithoutExtension(fileName)}");
		var textAsset = Resources.Load<TextAsset>(path);
		return textAsset.text;
#endif
	}

	public async UniTask LoadConfigTable()
	{
		var data = await LoadCSVAsync(CONFIG_TABLE_NAME);
		List<string[]> rows = CSVSerializer.ParseCSV(data, '|');
		rows.RemoveRange(0, 2);
		foreach (var rowItem in rows)
		{
			var field = typeof(ConfigTable).GetField(rowItem[0], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			try
			{
				CSVSerializer.SetValue(ConfigTable.Instance, field, rowItem[2]);
			}
			catch 
			{
				Debug.Log($"{rowItem[0]} invalid");
			}
		}
    }


	object CSVDeserialize(string text, Type type, bool hasSkipLine = true)
	{
		List<string[]> rows = CSVSerializer.ParseCSV(text, '|');
		if (hasSkipLine)
			rows.RemoveAt(1);

		var ret = CSVSerializer.Deserialize(rows, type);
		return ret;
	}
}

