using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserInfo
{
    public string token;
    public string username;
    public string name;
}
[Serializable]
public class RequestSignInData
{
    public string username;
    public string password;

    public RequestSignInData(string newUsername, string newPassword)
    {
        username = newUsername;
        password = newPassword;
    }
}

public class NetworkTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RequestSignInData signIn = new RequestSignInData("test", "test");
        Login(signIn).Forget();
    }

    public async UniTask Login(RequestSignInData data)
    {
        string json = JsonUtility.ToJson(data);
        UserInfo info = await Network.NetworkManager.SendToServer<UserInfo>("/login", Network.SENDTYPE.POST, json);
        Debug.Log(info);
    }
}
