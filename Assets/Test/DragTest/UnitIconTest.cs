using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class UnitIconTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private CancellationTokenSource cts;
    
    public void MoveToTarget(Vector2 _target)
    {
        cts?.Clear();
        cts = new CancellationTokenSource();

        UniTask.Create(async () =>
        {
            while (Vector2.Distance(transform.position, _target) > 0.1f)
            {
                await UniTask.Yield(cancellationToken: cts.Token);
                var newPos = Vector2.MoveTowards(transform.position, _target, moveSpeed * Time.deltaTime);
                transform.position = newPos;
            }
        });
        
        transform.position = _target;
    }
}
