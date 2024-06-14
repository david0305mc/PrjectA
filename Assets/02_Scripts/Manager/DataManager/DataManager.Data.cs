#pragma warning disable 114
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public partial class DataManager {
	public partial class Localization {
		public string id;
		public string ko;
		public string en;
		public string jp;
	};
	public Localization[] LocalizationArray { get; private set; }
	public Dictionary<string, Localization> LocalizationDic { get; private set; }
	public void BindLocalizationData(Type type, string text){
		var deserializaedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(LocalizationArray)).SetValue(this, deserializaedData, null);
		LocalizationDic = LocalizationArray.ToDictionary(i => i.id);
	}
	public Localization GetLocalizationData(string _id){
		if (LocalizationDic.TryGetValue(_id, out Localization value)){
			return value;
		}
		UnityEngine.Debug.LogError($"table doesnt contain id {_id}");
		return null;
	}
	public partial class Level {
		public int id;
		public int level;
		public int exp;
		public int unlockslot;
		public int goldreward;
	};
	public Level[] LevelArray { get; private set; }
	public Dictionary<int, Level> LevelDic { get; private set; }
	public void BindLevelData(Type type, string text){
		var deserializaedData = CSVDeserialize(text, type);
		GetType().GetProperty(nameof(LevelArray)).SetValue(this, deserializaedData, null);
		LevelDic = LevelArray.ToDictionary(i => i.id);
	}
	public Level GetLevelData(int _id){
		if (LevelDic.TryGetValue(_id, out Level value)){
			return value;
		}
		UnityEngine.Debug.LogError($"table doesnt contain id {_id}");
		return null;
	}
};
