using Cysharp.Threading.Tasks;
using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HeroObj : MoveObj
{
    private CancellationTokenSource cts;
    public int TileX;
    public int TileY;
    public void DragToTarget(Vector2 _target, int _tileX, int _tileY)
    {
        TileX = _tileX;
        TileY = _tileY;

        //fsm = StateMachine<UnitStates>.Initialize(this, UnitStates.Drag);

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
