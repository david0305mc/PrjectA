using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObj : MoveObj
{
    private long battleUID;
    public void InitObject(long _battleUID)
    {
        battleUID = _battleUID;
    }

}
