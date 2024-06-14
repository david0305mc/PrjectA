#pragma warning disable 114
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public partial class DataManager {
	public partial class Unitinfo {
		public int id;
		public string test;
	};
	public Unitinfo[] UnitinfoArray { get; private set; }
	public Dictionary<int, Unitinfo> UnitinfoDic { get; private set; }
	public void BindUnitinfoData(Type type, string text){
		var deserializaedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(UnitinfoArray)).SetValue(this, deserializaedData, null);
		UnitinfoDic = UnitinfoArray.ToDictionary(i => i.id);
	}
	public Unitinfo GetUnitinfoData(int _id){
		if (UnitinfoDic.TryGetValue(_id, out Unitinfo value)){
			return value;
		}
		UnityEngine.Debug.LogError($"table doesnt contain id {_id}");
		return null;
	}
};
