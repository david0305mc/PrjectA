using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public enum TileType
    { 
        Normal,
        Start,
        End,
        Block,
    }

    public class MapTestObj : MonoBehaviour
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

}
