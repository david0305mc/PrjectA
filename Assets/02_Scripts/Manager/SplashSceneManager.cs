using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (BaseOperatorUnit.Instance != null)
             DestroyImmediate(BaseOperatorUnit.Instance.gameObject);
        SceneManager.LoadSceneAsync(SceneName.Title);
    }
}
