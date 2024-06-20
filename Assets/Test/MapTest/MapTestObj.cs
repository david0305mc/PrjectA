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
    }

    public class MapTestObj : MonoBehaviour
    {
        [SerializeField] private TileType tileType;
        public int X;
        public int Y;
    }

}
