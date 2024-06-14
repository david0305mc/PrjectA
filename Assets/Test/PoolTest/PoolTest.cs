using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using Cysharp.Threading.Tasks;


public class PoolTest : MonoBehaviour
{
    [SerializeField] private GameObject testPrefab;


    public void OnClickBtnTest()
    {
        var obj = LeanPool.Spawn(testPrefab);
        UniTask.Create(async () => {
            await UniTask.Delay(1000);
            LeanPool.Despawn(obj);
        });
    }

    public void OnClickBtnRemove()
    { 
        
    }
}
