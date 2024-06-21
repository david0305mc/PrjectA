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
        public TileType tileType;
        public int X;
        public int Y;
    }

}
