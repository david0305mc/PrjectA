using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.StartGame();
    }
    
}
