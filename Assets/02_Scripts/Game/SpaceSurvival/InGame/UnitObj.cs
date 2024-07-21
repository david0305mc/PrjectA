using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UniRx;
using UnityEngine;

public class UnitObj : BaseObj
{

    private CompositeDisposable compositeDisposable;
    private StateMachine<UnitStates, Driver> fsm;

    protected override void InitFSM()
    {
        base.InitFSM();
        fsm = new StateMachine<UnitStates, Driver>(this);
    }

    protected override void ChangeIdleState()
    {
        base.ChangeIdleState();
        fsm.ChangeState(UnitStates.Idle);
    }

    protected override void Update()
    {
        if (fsm == null)
            return;
        fsm.Driver.Update.Invoke();
    }

    protected override void FixedUpdate()
    {
        if (fsm == null)
            return;
        fsm.Driver.FixedUpdate.Invoke();
    }

    protected void Idle_Enter()
    {
        Debug.Log("Idle_Enter");
        PlayAni("Walk");
    }
    protected void Idle_Update()
    {
        //UnitObj targetEnemy = SearchTarget();

        targetObj = SearchNearestOpponent(false);

        if (targetObj != null && !HasPath(currTileX, currTileY, targetObj.currTileX, targetObj.currTileY, false))
        {
            targetObj = SearchNearestOpponent(true);
        }
        else if (targetObj == null)
        {
            targetObj = SearchNearestOpponent(true);
        }

        if (targetObj != default)
        {
            // GetOuterCells
            // finding nearest outer cell
            RefreshPath(currTileX, currTileY, targetObj.currTileX, targetObj.currTileY, false);
            fsm.ChangeState(UnitStates.Move);
        }
    }

    protected void Move_Enter()
    {
        Debug.Log("Move_Enter");
        PlayAni("Walk");
        compositeDisposable?.Clear();
        compositeDisposable = new CompositeDisposable();
        MessageDispather.Receive<int>(EMessage.UpdateTile).Subscribe(_ =>
        {
            if (targetObj == null)
                return;

            if (isHero)
            {
                if (SS.UserData.Instance.GetHeroData(UnitUID) == default)
                    return;
                if (SS.UserData.Instance.GetEnemyData(targetObj.UnitUID) == default)
                    return;
            }
            else
            {
                if (SS.UserData.Instance.GetEnemyData(UnitUID) == default)
                    return;
                if (SS.UserData.Instance.GetHeroData(targetObj.UnitUID) == default)
                    return;
            }
            if (fsm != null)
            {
                int x = currNodeIndex == -1 ? startTile.X : pathList[currNodeIndex].x;
                int y = currNodeIndex == -1 ? startTile.Y : pathList[currNodeIndex].y;

                Debug.Log($"MessageDispather.Receive {currNodeIndex}");
                //RefreshPath(x, y, endTile.X, endTile.Y);

                if (targetObj != null && !HasPath(currTileX, currTileY, targetObj.currTileX, targetObj.currTileY, false))
                {
                    targetObj = SearchNearestOpponent(true);
                }
                if (targetObj != null)
                {
                    RefreshPath(x, y, targetObj.currTileX, targetObj.currTileY, false);
                }
                else
                {
                    fsm.ChangeState(UnitStates.Idle);
                }
            }
        }).AddTo(compositeDisposable);
    }

    protected void Move_Exit()
    {
        compositeDisposable?.Clear();
    }

    protected void Move_Update()
    {
        if (CheckTargetRange())
        {
            fsm.ChangeState(UnitStates.Attack);
        }
        else
        {
            MoveEvent();
        }
    }
    private void MoveEvent()
    {
        if (pathList.Count == 0 || targetNodeIndex >= pathList.Count)
            return;

        DrawPathLine();


        var distToTarget = (Vector2)pathList[targetNodeIndex].location - _rigidbody2D.position;
        if (distToTarget.magnitude < 0.05f)
        {
            targetNodeIndex++;
            MessageDispather.Publish(EMessage.UpdateTile, 1);
            if (targetNodeIndex >= pathList.Count)
            {
                Debug.LogError("Complete");
                //Lean.Pool.LeanPool.Despawn(gameObject);
                //isActive = false;
                fsm.ChangeState(UnitStates.Idle);
                return;
            }
            distToTarget = (Vector2)pathList[targetNodeIndex].location - _rigidbody2D.position;
        }
        //currTile = mapCreator.Tiles[_startX, _startY];
        //currTile.currNodeMark.SetActive(true);

        Vector2 distBetweenNode;
        if (targetNodeIndex == 0)
        {
            distBetweenNode = (Vector2)startTile.transform.position - (Vector2)pathList[targetNodeIndex].location;
        }
        else
        {
            distBetweenNode = (Vector2)pathList[targetNodeIndex].location - (Vector2)pathList[targetNodeIndex - 1].location;
        }

        if (distToTarget.magnitude < distBetweenNode.magnitude * 0.5f)
        {
            if (currNodeIndex < targetNodeIndex)
            {
                if (currNodeIndex == -1)
                {
                    startTile.SetCurrNodeMark(false);
                }
                else
                {
                    gridMap.Tiles[pathList[currNodeIndex].x, pathList[currNodeIndex].y].SetCurrNodeMark(false);
                }

                currNodeIndex = targetNodeIndex;
                currTileX = pathList[currNodeIndex].x;
                currTileY = pathList[currNodeIndex].y;
                gridMap.Tiles[pathList[currNodeIndex].x, pathList[currNodeIndex].y].SetCurrNodeMark(true);
            }
        }

        var targetNode = pathList[targetNodeIndex];
        Vector2 newPos;
        if (isBoidsAlgorithm)
        {
            var velocity = CalculateBoidsAlgorithm((Vector2)targetNode.location);
            newPos = Vector2.MoveTowards(_rigidbody2D.position, _rigidbody2D.position + velocity, _forwardSpeed * Time.deltaTime);
        }
        else
        {
            newPos = Vector2.MoveTowards(_rigidbody2D.position, (Vector2)targetNode.location, _forwardSpeed * Time.deltaTime);
        }
        FlipRenderers(_rigidbody2D.position.x <= targetNode.location.x);
        _rigidbody2D.MovePosition(newPos);
        //var movePos = rigidBody.position + (dist.normalized * speed * Time.fixedDeltaTime);

        //transform.position -= new Vector3(0, Time.fixedDeltaTime, 0);
    }

}
