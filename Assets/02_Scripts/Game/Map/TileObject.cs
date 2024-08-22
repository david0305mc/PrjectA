using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public GameObject currNodeMark;
    public TileType tileType;
    public SpriteRenderer sprite;

    private SpriteRenderer nodeSprite;
    public int X;
    public int Y;

    private void Awake()
    {
        nodeSprite = currNodeMark.GetComponent<SpriteRenderer>();
    }
    public bool IsBlock()
    {
        return tileType == TileType.Block || tileType == TileType.Building;
    }

    public void SetCurrNodeMark(bool _value, Color? _color = default)
    {
        currNodeMark.SetActive(_value);
        if (_value)
        {
            nodeSprite.color = _color?? Color.green;
        }
    }
    //private void OnMouseDown()
    //{
    //    tileType = TileType.Block;
    //    UpdateTile();
    //    MessageDispather.Publish(EMessage.UpdateTile, 1);
    //}

    public void SetTileType(TileType _type)
    {
        if (tileType != _type)
        {
            tileType = _type;
            MessageDispather.Publish(EMessage.UpdateTile, 1);
        }
    }
}
