using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class UserData : Singleton<UserData>
{
    private static readonly string LocalFilePath = Path.Combine(Application.persistentDataPath, "LocalData");
    private static readonly string InBuildDataImportPath = "InBuildData";
    private static readonly string InBuildDataExportPath = Path.Combine(Application.persistentDataPath, "InBuildData");

    public GameUserData GameData { get; set; }
}
