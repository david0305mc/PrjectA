using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MonsterLove.StateMachine;
using UniRx;
using UnityEngine;

public class UnitObj : BaseObj
{

    private CompositeDisposable compositeDisposable;
    private StateMachine<UnitStates, Driver> fsm;
    private int currAggroTarget;
    protected override void Awake()
    {
        base.Awake();
        fsm = new StateMachine<UnitStates, Driver>(this);
    }

    public override void InitData(bool _isHero, long _unitUID)
    {
        base.InitData(_isHero, _unitUID);
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
        PlayAni("Walk");
    }
    protected void Idle_Update()
    {
        if (IsHero)
        {
            int test = 0;
        }
        else
        {
            int test = 0;
        }

        //if (TargetObj == null)
        {
            TargetObj = FindTarget();
        }
        if (TargetObj != null)
        {
            currAggroTarget = TargetObj.UnitData.refData.aggroorder;
            RefreshPath();
            fsm.ChangeState(UnitStates.Move);
        }
    }

    protected void RefreshPath()
    {
        int _startX = currTileX;
        int _startY = currTileY;
        targetNodeIndex = 0;
        currNodeIndex = -1;

        if (!IsHero)
        {
            SetAStarPath(_startX, _startY, TargetObj.currTileX, TargetObj.currTileY, true);
            //SetAStarPathWithBuilding(_startX, _startY, targetObj.currTileX, targetObj.currTileY, true);

            PathList = pathFinder.FindPath();
            //var pathListPassBuilding = pathFinderPassBuilding.FindPath();

            //if (pathListPassBuilding.Count + 4 < pathList.Count)
            //{
            //    // ???? ???? ???????? ???????? ?????? ?????? ??????.
            //    pathList = pathListPassBuilding;
            //}
            int buildingNodeIndex = GetBuildingIndexOnPath();
            if (buildingNodeIndex > 0)
            {
                TargetObj = SS.GameManager.Instance.GetBuildingObj(PathList[buildingNodeIndex].x, PathList[buildingNodeIndex].y);
                SetAStarPath(_startX, _startY, TargetObj.currTileX, TargetObj.currTileY, false);
                PathList = pathFinder.FindPath();
            }
        }
        else
        {
            SetAStarPath(_startX, _startY, TargetObj.currTileX, TargetObj.currTileY, false);
            PathList = pathFinder.FindPath();

            if (PathList.Count == 0)
            {
                SetAStarPath(_startX, _startY, TargetObj.currTileX, TargetObj.currTileY, true);
                PathList = pathFinder.FindPath();

                if (PathList.Count == 0)
                {
                    ChangeIdleState();
                    return;
                }
                //ChangeIdleState();

                //if (PathList.Count > 0)
                //{
                //    // To Do : Check Next Is Building
                //    if (gridMap.Tiles[PathList[0].x, PathList[0].y].tileType == TileType.Building)
                //    {
                //        isBlocked = true;
                //    }
                //}
                //else
                //{
                //    Debug.LogError("PathList.Count == 0");
                //    isBlocked = true;
                //}
            }
            //SetAStarPathWithBuilding(_startX, _startY, TargetObj.currTileX, TargetObj.currTileY, true);
            //var pathListPassBuilding = pathFinderPassBuilding.FindPath();
        }

        startTile = gridMap.Tiles[_startX, _startY];

        foreach (var item in PathList)
        {
            item.location += new Vector3(randPosOffset.x, randPosOffset.y, 0);
        }

        if (targetNodeIndex < PathList.Count)
        {
            var distToTarget = (Vector2)PathList[targetNodeIndex].location - _rigidbody2D.position;
            Vector2 distBetweenNode;
            if (currNodeIndex == -1)
            {
                distBetweenNode = (Vector2)startTile.transform.position - (Vector2)PathList[0].location;
            }
            else
            {
                distBetweenNode = (Vector2)PathList[currNodeIndex].location - (Vector2)PathList[currNodeIndex + 1].location;
            }

            if (distToTarget.magnitude < distBetweenNode.magnitude * 0.5f)
            {
                if (currNodeIndex < targetNodeIndex)
                {
                    currNodeIndex = targetNodeIndex;
                    currTileX = PathList[currNodeIndex].x;
                    currTileY = PathList[currNodeIndex].y;
                }
            }
        }
    }
    private BaseObj FindTarget()
    {
        BaseObj target = SearchNearestOpponent(true);

        //if (target != null && !HasPath(currTileX, currTileY, target.currTileX, target.currTileY, false))
        //{
        //    // ?????? ?????? ?????? ???? ????, ???????? ????
        //    target = SearchNearestOpponent(true);
        //}
        //else
        //if (target == null)
        //{
        //    // ?????? ???? ????, ???????? ????
        //    target = SearchNearestOpponent(true);
        //}

        if (!isHero)
        {
            if (target == default && SS.UserDataManager.Instance.HasMyBoss)
            {
                target = SS.GameManager.Instance.MyBossObj;
            }
        }
        return target;
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
        fsm.ChangeState(UnitStates.UI);
    }
    public override void SetBattleMode()
    {
        base.SetBattleMode();
    }

    protected void Move_Enter()
    {
        PlayAni("Walk");
        compositeDisposable?.Clear();
        compositeDisposable = new CompositeDisposable();
        MessageDispather.Receive<EMessage, EventParm<long, Vector2Int>>(EMessage.UpdateTile).Subscribe(_param =>
        {
            if (TargetObj == null)
            {
                ChangeIdleState();
                return;
            }
            
            if (_param.arg1 == UnitUID)
                return;

            if (!HasTileInPath(new Vector2Int(_param.arg2.x, _param.arg2.y)))
            {
                Debug.Log("!HasTileInPath");
                return;
            }

            if (isHero)
            {
                if (SS.UserDataManager.Instance.GetBattleHeroData(UnitUID) == default)
                    return;
                if (SS.UserDataManager.Instance.GetEnemyData(TargetObj.UnitUID) == default)
                    return;
            }
            else
            {
                if (SS.UserDataManager.Instance.GetEnemyData(UnitUID) == default)
                    return;
                if (SS.UserDataManager.Instance.GetBattleHeroData(TargetObj.UnitUID) == default)
                    return;
            }

            if (fsm != null)
            {
                //int x = currNodeIndex == -1 ? startTile.X : pathList[currNodeIndex].x;
                //int y = currNodeIndex == -1 ? startTile.Y : pathList[currNodeIndex].y;

                if (currNodeIndex < PathList.Count - 1)
                {
                    //fsm.ChangeState(UnitStates.Idle);
                    if (!HasPath(currTileX, currTileY, TargetObj.currTileX, TargetObj.currTileY, false))
                    {
                        TargetObj = null;
                    }
                    ChangeIdleState();
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
        if (IsHero)
        {
            int test = 0;
        }
        else
        {
            int test = 0;
        }
        if (CheckTargetRange())
        {
            fsm.ChangeState(UnitStates.Attack);
        }
        else
        {
            MoveEvent();
        }
    }

    protected override void Attack_Enter()
    {
        base.Attack_Enter();
    }

    protected override void Attack_Exit()
    {
        base.Attack_Exit();
        TargetObj = null;
    }
    protected override void DoAttack()
    {
        base.DoAttack();
    }

    private bool IsCloseDistanceTarget()
    {
        return false;
    }

    private Vector2Int GetNextTile()
    {
        if (PathList == null || PathList.Count == 0)
        {
            return new Vector2Int(-1, -1);
        }
        
        int nextNodeIndex = targetNodeIndex + 1;
        if (nextNodeIndex >= PathList.Count)
        {
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(PathList[nextNodeIndex].x, PathList[nextNodeIndex].y);
    }

    private void MoveEvent()
    {
        if (PathList.Count == 0 || targetNodeIndex >= PathList.Count)
            return;

        if (gridMap.Tiles[PathList[targetNodeIndex].x, PathList[targetNodeIndex].y].IsBlock())
            return;

        DrawPathLine();

        var distToTarget = (Vector2)PathList[targetNodeIndex].location - _rigidbody2D.position;
        if (distToTarget.magnitude < 0.05f)
        {
            targetNodeIndex++;
            if (targetNodeIndex >= PathList.Count)
            {
                //Debug.LogError("Complete");
                //Lean.Pool.LeanPool.Despawn(gameObject);
                //isActive = false;
                //fsm.ChangeState(UnitStates.Idle);
                MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(UnitUID, new Vector2Int(PathList[PathList.Count - 1].x, PathList[PathList.Count - 1].y)));
                fsm.ChangeState(UnitStates.Attack);
                return;
            }
            else if (gridMap.Tiles[PathList[targetNodeIndex].x, PathList[targetNodeIndex].y].IsBlock())
            {
                ChangeIdleState();
                Debug.Log("Next Tile Is Block");
                //targetNodeIndex--;
                //isBlocked = true;
                //TargetObj = null;
                //ChangeIdleState();
                return;
            }
            else
            {
                MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(UnitUID, new Vector2Int(PathList[PathList.Count - 1].x, PathList[PathList.Count - 1].y)));
            }

            distToTarget = (Vector2)PathList[targetNodeIndex].location - _rigidbody2D.position;
        }
        //currTile = mapCreator.Tiles[_startX, _startY];
        //currTile.currNodeMark.SetActive(true);

        Vector2 distBetweenNode;
        if (targetNodeIndex == 0)
        {
            distBetweenNode = (Vector2)startTile.transform.position - (Vector2)PathList[targetNodeIndex].location;
        }
        else
        {
            distBetweenNode = (Vector2)PathList[targetNodeIndex].location - (Vector2)PathList[targetNodeIndex - 1].location;
        }

        if (distToTarget.magnitude < distBetweenNode.magnitude * 0.5f)
        {
            if (currNodeIndex < targetNodeIndex)
            {
                currNodeIndex = targetNodeIndex;
                currTileX = PathList[currNodeIndex].x;
                currTileY = PathList[currNodeIndex].y;
            }
        }

        var targetNode = PathList[targetNodeIndex];
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
        for (int i = targetNodeIndex; i < PathList.Count; i++)
        {
            if (_tile.x == PathList[i].x && _tile.y == PathList[i].y)
            {
                return true;
            }
        }
        return false;
    }


    //public Vector3[] GetOuterCells()
    //{
    //    int sizeX = (int)this.GetSize().x;

    //    if (sizeX <= 1)
    //    {
    //        return new Vector3[0];
    //    }

    //    List<Vector3> cells = new List<Vector3>();
    //    for (int x = 0; x <= sizeX; x++)
    //    {
    //        for (int z = 0; z <= sizeX; z++)
    //        {
    //            if (x == sizeX || z == sizeX || x == 0 || z == 0)
    //            {
    //                Vector3 cellPos = this.GetPosition() + new Vector3(x, 0, z);
    //                if (!cells.Contains(cellPos))
    //                {
    //                    cells.Add(cellPos);
    //                }
    //            }
    //        }
    //    }

    //    return cells.ToArray();
    //}
}
