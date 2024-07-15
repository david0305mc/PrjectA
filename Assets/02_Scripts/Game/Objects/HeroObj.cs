using Cysharp.Threading.Tasks;
using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HeroObj : MoveObj
{
    public int TileX;
    public int TileY;

    public static int spawnCount = 0;

    public void DragToTarget(Vector2 _target, int _tileX, int _tileY)
    {
        Debug.Log($"STest drag Count {++spawnCount}");
        TileX = _tileX;
        TileY = _tileY;

        //fsm = StateMachine<UnitStates>.Initialize(this, UnitStates.Drag);

        UniTask.Create(async () =>
        {
            while (Vector2.Distance(transform.position, _target) > 0.1f)
            {
                await UniTask.Yield();
                var newPos = Vector2.MoveTowards(transform.position, _target, 3f * Time.deltaTime);
                transform.position = newPos;
            }
            Debug.Log($"STest spawn Count {spawnCount}");
            SS.GameManager.Instance.AddHeroObj(this);
        });
        transform.position = _target;
    }

}
