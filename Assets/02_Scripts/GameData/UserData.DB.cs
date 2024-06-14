using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UserData : Singleton<UserData>
{
    public void LoadGameData()
    {
        int newUser = PlayerPrefs.GetInt("IsNewUser", 0);
        if (newUser == 1)
        {
            try
            {
                var localData = Utill.LoadFromFile(LocalFilePath);
                //localData = Utill.EncryptXOR(localData);
                GameData = JsonUtility.FromJson<GameUserData>(localData);
            }
            catch
            {
                // NewGame
                InitNewGameData();
                return;
            }

            GameData.UpdateRefData();
            //if (IsOnTutorial())
            //{
            //    // Restart 
            //    InitNewGameData();
            //}
        }
        else
        {
            PlayerPrefs.SetInt("NewUser", 1);
            // NewGame
            InitNewGameData();
        }
    }
    public void SaveGameData()
    {
        //LocalData.LastLoginTime = GameTime.Get();
        var saveData = JsonUtility.ToJson(GameData);
        //saveData = Utill.EncryptXOR(saveData);
        Utill.SaveFile(LocalFilePath, saveData);
        if (PlayerPrefs.GetInt("NewUser", 0) == 0)
        {
            PlayerPrefs.SetInt("NewUser", 1);
        }
    }

    private void InitNewGameData()
    {
        GameData = new GameUserData();
        SaveGameData();
    }
}
