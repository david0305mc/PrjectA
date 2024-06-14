#pragma warning disable 114
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public partial class DataManager {
	public partial class Sheet1 {
		public int index;
		public int box_id;
		public string type;
		public int item_id;
		public int item_value;
		public QST_MISSION_TYPE rate;
	};
	public Sheet1[] Sheet1Array { get; private set; }
	public Dictionary<int, Sheet1> Sheet1Dic { get; private set; }
	public void BindSheet1Data(Type type, string text){
		var deserializaedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(Sheet1Array)).SetValue(this, deserializaedData, null);
		Sheet1Dic = Sheet1Array.ToDictionary(i => i.index);
	}
	public Sheet1 GetSheet1Data(int _index){
		if (Sheet1Dic.TryGetValue(_index, out Sheet1 value)){
			return value;
		}
		UnityEngine.Debug.LogError($"table doesnt contain id {_index}");
		return null;
	}
	public partial class Test1 {
		public int index;
		public int box_id;
		public string type;
		public int item_id;
		public int item_value;
		public int rate;
	};
	public Test1[] Test1Array { get; private set; }
	public Dictionary<int, Test1> Test1Dic { get; private set; }
	public void BindTest1Data(Type type, string text){
		var deserializaedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(Test1Array)).SetValue(this, deserializaedData, null);
		Test1Dic = Test1Array.ToDictionary(i => i.index);
	}
	public Test1 GetTest1Data(int _index){
		if (Test1Dic.TryGetValue(_index, out Test1 value)){
			return value;
		}
		UnityEngine.Debug.LogError($"table doesnt contain id {_index}");
		return null;
	}
};
