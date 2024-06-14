#pragma warning disable 114
using System;
using System.Collections;
using System.Collections.Generic;
public partial class Datatable {
private static readonly Lazy<Datatable> _instance = new Lazy<Datatable>(() => new Datatable());
public static Datatable Instance { get { return _instance.Value; } }

	public static readonly string DataTablePrefix = "tbl";
	public static readonly string DicDataTablePrefix = "dic";
	public static readonly string ClassPrefix = "Ref";

	public class Randombox {
		public int index;
		public int box_id;
		public REWARD_TYPE type;
		public int item_id;
		public int item_value;
		public int rate;
	};
	public Dictionary<int, Randombox> dtRandombox = new Dictionary<int, Randombox>();
	public void LoadRandombox(List<Dictionary<string, object>> rowList) {
		dtRandombox = new Dictionary<int, Randombox>();
		foreach (var rowItem in rowList) {
			Randombox dicItem = new Randombox();
			foreach (var item in rowItem) {
				var field = typeof(Randombox).GetField(item.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				try { field.SetValue(dicItem, item.Value); }
				catch { UnityEngine.Debug.LogError(item); }
			}
			if (dtRandombox.ContainsKey(dicItem.index)) {
				UnityEngine.Debug.LogError("Duplicate Key in Randombox");
				UnityEngine.Debug.LogError(string.Format("Duplicate Key {0}", dicItem.index));
			}
			dtRandombox.Add(dicItem.index, dicItem);
		}
	}
	public Randombox GetRandomboxData(int _index) {
		if (!dtRandombox.ContainsKey(_index)){
			UnityEngine.Debug.LogError("Table Randombox");
			UnityEngine.Debug.LogError(string.Format("table doesn't contain id {0}", _index));
			return null;
		}
		return dtRandombox[_index];
	}
	public Dictionary<int, Randombox> GetRandomboxData() {
		return dtRandombox;
	}


	public class Text_ui {
		public string index;
		public string kr;
		public string en;
		public string jp;
		public string gan;
		public string bun;
	};
	public Dictionary<string, Text_ui> dtText_ui = new Dictionary<string, Text_ui>();
	public void LoadText_ui(List<Dictionary<string, object>> rowList) {
		dtText_ui = new Dictionary<string, Text_ui>();
		foreach (var rowItem in rowList) {
			Text_ui dicItem = new Text_ui();
			foreach (var item in rowItem) {
				var field = typeof(Text_ui).GetField(item.Key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				try { field.SetValue(dicItem, item.Value); }
				catch { UnityEngine.Debug.LogError(item); }
			}
			if (dtText_ui.ContainsKey(dicItem.index)) {
				UnityEngine.Debug.LogError("Duplicate Key in Text_ui");
				UnityEngine.Debug.LogError(string.Format("Duplicate Key {0}", dicItem.index));
			}
			dtText_ui.Add(dicItem.index, dicItem);
		}
	}
	public Text_ui GetText_uiData(string _index) {
		if (!dtText_ui.ContainsKey(_index)){
			UnityEngine.Debug.LogError("Table Text_ui");
			UnityEngine.Debug.LogError(string.Format("table doesn't contain id {0}", _index));
			return null;
		}
		return dtText_ui[_index];
	}
	public Dictionary<string, Text_ui> GetText_uiData() {
		return dtText_ui;
	}

};
