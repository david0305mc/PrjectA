using System;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using UniRx;
using UnityEngine;

public class UnitObj : BaseObj
{

    private CompositeDisposable compositeDisposable;
    private Vector2Int targetTile;
    private StateMachine<UnitStates, Driver> fsm;


    protected override void Awake()
    {
        base.Awake();
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
            // 타겟이 있는데 경로는 없는 경우, 건물까지 검색
            targetObj = SearchNearestOpponent(true);
        }
        else if (targetObj == null)
        {
            // 타겟이 없는 경우, 견물까지 검색
            targetObj = SearchNearestOpponent(true);
        }

        if (targetObj != default)
        {

            // GetOuterCells
            // finding nearest outer cell
            targetTile = GetNearestOutTile(currTileX, currTileY, targetObj.currTileX, targetObj.currTileY, false);
            RefreshPath(currTileX, currTileY, targetTile.x, targetTile.y, false);
            fsm.ChangeState(UnitStates.Move);
        }
    }

    private Vector2Int GetNearestOutTile(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        int shortPathCount = int.MaxValue;
        Vector2Int targetTile = new Vector2Int(GameDefine.OuterTile[0, 0], GameDefine.OuterTile[0, 1]);
        for (int i = 0; i < GameDefine.OuterTile.GetLength(0); i++)
        {
            var pathCount = GetPathCount(_startX, _startY, _endX + GameDefine.OuterTile[i, 0], _endY + GameDefine.OuterTile[i, 1], _passBuilding);
            if (shortPathCount > pathCount)
            {
                shortPathCount = pathCount;
                targetTile = new Vector2Int(_endX + GameDefine.OuterTile[i, 0], _endY + GameDefine.OuterTile[i, 1]);
            }
        }
        return targetTile;
    }

    public void SetUIMode(int _sortingOrder)
    {
        sortingGroup.sortingLayerName = Game.GameConfig.UILayerName;
        sortingGroup.sortingOrder = _sortingOrder;
        
        fsm.ChangeState(UnitStates.Idle);
        HideCanvase();
        transform.SetScale(200f);
        PlayAni("Idle");
    }
    public void SetBattleMode()
    {
        sortingGroup.sortingLayerName = Game.GameConfig.ForegroundLayerName;
        sortingGroup.sortingOrder = 0;
        //attackDelay = 0f;
        HideCanvase();
        //SetSelected(false);
        transform.SetScale(1f);
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
                if (SS.UserDataManager.Instance.GetBattleHeroData(UnitUID) == default)
                    return;
                if (SS.UserDataManager.Instance.GetEnemyData(targetObj.UnitUID) == default)
                    return;
            }
            else
            {
                if (SS.UserDataManager.Instance.GetEnemyData(UnitUID) == default)
                    return;
                if (SS.UserDataManager.Instance.GetBattleHeroData(targetObj.UnitUID) == default)
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
                    targetTile = GetNearestOutTile(currTileX, currTileY, targetObj.currTileX, targetObj.currTileY, false);
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
        MoveEvent();
        //if (CheckTargetRange())
        //{
        //    fsm.ChangeState(UnitStates.Attack);
        //}
        //else
        //{
        //    MoveEvent();
        //}
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
                //fsm.ChangeState(UnitStates.Idle);
                fsm.ChangeState(UnitStates.Attack);
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
            newPos = Vector2.MoveTowards(_rigidbody2D.position, (Vector2)targetNode.location + randPosOffset, _forwardSpeed * Time.deltaTime);
        }
        _rigidbody2D.MovePosition(newPos);
        FlipRenderers(_rigidbody2D.position.x <= targetNode.location.x);
        //var movePos = rigidBody.position + (dist.normalized * speed * Time.fixedDeltaTime);

        //transform.position -= new Vector3(0, Time.fixedDeltaTime, 0);
    }

}
