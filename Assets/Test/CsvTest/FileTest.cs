using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string fileName = "ObjTable";
        string path = System.IO.Path.Combine("Data", $"{fileName}");
        var req = Resources.Load<TextAsset>(path);
        Debug.Log($"req.text {req.text}");
        
    }
}
