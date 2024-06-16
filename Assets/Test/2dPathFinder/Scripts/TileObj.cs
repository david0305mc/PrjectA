using System;
using UnityEngine;
using UnityEngine.UI;

public class TileObj : MonoBehaviour
{
    [NonSerialized]
    public int x;
    [NonSerialized]
    public int y;

    public Image img;
    public Button button;

    void Awake()
    {
        img.gameObject.SetActive(false);
    }

}
