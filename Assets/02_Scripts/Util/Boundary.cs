using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary
{
    private float _xLimit = -1;
    private float _yLimit = -1;

    public float XLimit { 
        get {
            CalculateLimit();
            return _xLimit; 
        } 
    }
    public float YLimit { 
        get {
            CalculateLimit();
            return _yLimit; 
        } 
    }

    void CalculateLimit()
    {
        if (_xLimit == -1 || _yLimit == -1)
        {
            _yLimit = Camera.main.orthographicSize;
            _xLimit = _yLimit * Screen.width / Screen.height;
        }
    }
}
