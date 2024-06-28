using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public GameObject currNodeMark;
    public TileType tileType;
    public SpriteRenderer sprite;

    public int X;
    public int Y;

    private void Awake()
    {
        UpdateTile();
    }

    public void SetCurrNodeMark(bool _value)
    {
        return;
        currNodeMark.SetActive(_value);
    }

    public void UpdateTile()
    {
        switch (tileType)
        {
            case TileType.Block:
                sprite.color = Color.red;
                break;
            default:
                sprite.color = Color.white;
                break;
        }
    }
    private void OnMouseDown()
    {
        tileType = TileType.Block;
        UpdateTile();
        MessageDispather.Publish(EMessage.UpdateTile, 1);
    }
}
