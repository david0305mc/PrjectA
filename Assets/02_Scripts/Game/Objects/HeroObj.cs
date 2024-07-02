using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HeroObj : MoveObj
{
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
                var newPos = Vector2.MoveTowards(transform.position, _target, 3f * Time.deltaTime);
                transform.position = newPos;
            }
        });

        transform.position = _target;
    }


}
