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
    private bool isToChangeTarget;

    protected override void Awake()
    {
        base.Awake();
        fsm = new StateMachine<UnitStates, Driver>(this);
    }

    public override void InitData(bool _isHero, long _unitUID, GridMap _mapCreator, Vector2Int _startTile, Vector2Int _endTile)
    {
        base.InitData(_isHero, _unitUID, _mapCreator, _startTile, _endTile);
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
        //Debug.Log("Idle_Enter");
        isToChangeTarget = false;
        PlayAni("Walk");
    }
    protected void Idle_Update()
    {
        //UnitObj targetEnemy = SearchTarget();
        
        targetObj = SearchNearestOpponent(false);

        if (isHero)
        {
            if (targetObj != default)
            {
                // GetOuterCells
                // finding nearest outer cell
                targetTile = GetNearestOutTile(currTileX, currTileY, targetObj.currTileX, targetObj.currTileY, false);
                if (!targetTile.Equals(new Vector2Int(-1, -1)))
                {
                    RefreshPath(currTileX, currTileY, targetTile.x, targetTile.y, false);
                    fsm.ChangeState(UnitStates.Move);
                }
            }
        }
        else
        {
            if (targetObj != null && !HasPath(currTileX, currTileY, targetObj.currTileX, targetObj.currTileY, false))
            {
                // ?????? ?????? ?????? ???? ????, ???????? ????
                targetObj = SearchNearestOpponent(true);
            }
            else if (targetObj == null)
            {
                // ?????? ???? ????, ???????? ????
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
    }

    private Vector2Int GetNearestOutTile(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        int shortPathCount = int.MaxValue;
        Vector2Int targetTile = new Vector2Int(-1, -1);
        for (int i = 0; i < GameDefine.OuterTile.GetLength(0); i++)
        {
            var pathCount = GetPathCount(_startX, _startY, _endX + GameDefine.OuterTile[i, 0], _endY + GameDefine.OuterTile[i, 1], _passBuilding);
            if (pathCount > 0 && shortPathCount > pathCount)
            {
                shortPathCount = pathCount;
                targetTile = new Vector2Int(_endX + GameDefine.OuterTile[i, 0], _endY + GameDefine.OuterTile[i, 1]);
            }
        }
        return targetTile;
    }

    public override void SetUIMode(int _sortingOrder)
    {
        base.SetUIMode(_sortingOrder);
        fsm.ChangeState(UnitStates.Idle);
    }
    public override void SetBattleMode()
    {
        base.SetBattleMode();
    }

    protected void Move_Enter()
    {
        //Debug.Log("Move_Enter");
        PlayAni("Walk");
        compositeDisposable?.Clear();
        compositeDisposable = new CompositeDisposable();
        MessageDispather.Receive<Vector2Int>(EMessage.UpdateTile).Subscribe(tile =>
        {
            if (targetObj == null)
                return;
            
            if (!HasTileInPath(tile))
            {
                Debug.Log("!HasTileInPath");
                return;
            }
            
           
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

                if (currNodeIndex < pathList.Count - 1)
                {
                    //fsm.ChangeState(UnitStates.Idle);
                    isToChangeTarget = true;
                }
                else
                {
                    Debug.Log("Almost Finish");
                }
                //if (targetObj != null && !HasPath(currTileX, currTileY, targetObj.currTileX, targetObj.currTileY, false))
                //{
                //    targetObj = SearchNearestOpponent(true);
                //}
                //if (targetObj != null)
                //{
                //    targetTile = GetNearestOutTile(currTileX, currTileY, targetObj.currTileX, targetObj.currTileY, false);
                //    RefreshPath(x, y, targetObj.currTileX, targetObj.currTileY, false);
                //}
                //else
                //{
                //    fsm.ChangeState(UnitStates.Idle);
                //}
                //fsm.ChangeState(UnitStates.Idle);
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

    private Vector2Int GetNextTile()
    {
        if (pathList == null || pathList.Count == 0)
        {
            return new Vector2Int(-1, -1);
        }
        
        int nextNodeIndex = targetNodeIndex + 1;
        if (nextNodeIndex >= pathList.Count)
        {
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(pathList[nextNodeIndex].x, pathList[nextNodeIndex].y);
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
            if (targetNodeIndex >= pathList.Count)
            {
                //Debug.LogError("Complete");
                //Lean.Pool.LeanPool.Despawn(gameObject);
                //isActive = false;
                //fsm.ChangeState(UnitStates.Idle);
                MessageDispather.Publish(EMessage.UpdateTile, new Vector2Int(pathList[pathList.Count - 1].x, pathList[pathList.Count - 1].y));
                fsm.ChangeState(UnitStates.Attack);
                return;
            }
            else if (gridMap.Tiles[pathList[targetNodeIndex].x, pathList[targetNodeIndex].y].IsBlock())
            {
                Debug.Log("Next Tile Is Block");
                fsm.ChangeState(UnitStates.Idle);
                return;
            }
            else if (isToChangeTarget)
            {
                Debug.Log("isToChangeTarget");
                fsm.ChangeState(UnitStates.Idle);
            }
            else
            {
                MessageDispather.Publish(EMessage.UpdateTile, new Vector2Int(pathList[targetNodeIndex].x, pathList[targetNodeIndex].y));
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
        _rigidbody2D.MovePosition(newPos);
        FlipRenderers(_rigidbody2D.position.x <= targetNode.location.x);
        //var movePos = rigidBody.position + (dist.normalized * speed * Time.fixedDeltaTime);

        //transform.position -= new Vector3(0, Time.fixedDeltaTime, 0);
    }

    private bool HasTileInPath(Vector2Int _tile)
    {
        for (int i = targetNodeIndex; i < pathList.Count; i++)
        {
            if (_tile.x == pathList[i].x && _tile.y == pathList[i].y)
            {
                return true;
            }
        }
        return false;
    }
}
