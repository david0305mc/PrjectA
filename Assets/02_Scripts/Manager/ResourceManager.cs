using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager : SingletonMono<ResourceManager>
{
    private Dictionary<string, string> mTableAssetDic = new Dictionary<string, string>();


    public void AddTableAsset(string _id, string _data)
    {
        _id = Path.GetFileNameWithoutExtension(_id.ToLower());
        mTableAssetDic.Add(_id, _data);
    }

    public string GetTableAsset(string _id)
    {
        _id = Path.GetFileNameWithoutExtension(_id.ToLower());
        if (mTableAssetDic.ContainsKey(_id))
            return mTableAssetDic[_id];

        Debug.LogError(_id);
        return null;
    }
}
