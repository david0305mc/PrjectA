using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MonsterLove.StateMachine;
using UniRx;
using UnityEngine;

namespace SS
{
    public class UnitObj : BaseObj
    {
        private CompositeDisposable compositeDisposable;
        private StateMachine<UnitStates, Driver> fsm;
        private bool DirtyPath { get; set; }
        private int currAggroTarget;
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
            //Debug.Log("Idle_Enter");
            PlayAni("Idle");
            DirtyPath = false;
        }
        protected void Idle_Update()
        {
            //if (TargetObj == null)
            {
                TargetObj = SearchTarget();
            }
            if (TargetObj != null)
            {
                currAggroTarget = TargetObj.UnitData.refData.aggroorder;
                if (GeneratePath())
                {
                    fsm.ChangeState(UnitStates.Move);
                }
            }
            else
            {
                if (isHero)
                {
                    var grid = gridMap.Pos2Node(OrgPos);
                    if (grid.x != currTileX || grid.y != currTileY)
                    {
                        fsm.ChangeState(UnitStates.MoveToHome);
                    }
                }
            }
        }

        protected bool GeneratePath()
        {
            int _startX = currTileX;
            int _startY = currTileY;
            TargetNodeIndex = 0;

            if (!IsHero)
            {
                SetAStarPath(_startX, _startY, TargetObj.currTileX, TargetObj.currTileY, true);
                PathList = pathFinder.FindPath();
                int buildingNodeIndex = GetBuildingIndexOnPath();
                if (buildingNodeIndex >= 0)
                {
                    TargetObj = SS.GameManager.Instance.GetBuildingObj(PathList[buildingNodeIndex].x, PathList[buildingNodeIndex].y);
                    SetAStarPath(_startX, _startY, TargetObj.currTileX, TargetObj.currTileY, false);
                    PathList = pathFinder.FindPath();
                    if (PathList.Count == 0)
                    {
                        ChangeIdleState();
                        return false;
                    }
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
                        return false;
                    }
                    if (gridMap.Tiles[PathList[0].x, PathList[0].y].tileType == TileType.Building)
                    {
                        return false;
                    }
                }
            }

            startTile = gridMap.Tiles[_startX, _startY];
            return true;
        }
        private BaseObj SearchTarget()
        {
            BaseObj target = SearchNearestOpponent(true);

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

        protected override void FlipRenderers(bool right)
        {
            base.FlipRenderers(right);
        }

        public override void SetUIState()
        {
            base.SetUIState();
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
                    if (TargetObj.currTileX == currTileX && TargetObj.currTileY == currTileY)
                    {
                        Debug.Log("Almost Finish");
                    }
                    else
                    {
                        //if (!HasPath(currTileX, currTileY, TargetObj.currTileX, TargetObj.currTileY, false))
                        //{
                        //    TargetObj = null;
                        //}
                        //ChangeIdleState();
                        DirtyPath = true;
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
        protected void MoveToHome_Enter()
        {
            var grid = gridMap.Pos2Node(OrgPos);
            SetAStarPath(currTileX, currTileY, grid.x, grid.y, false);
            PathList = pathFinder.FindPath();
            TargetNodeIndex = 0;
        }

        protected void MoveToHome_Update()
        {
            if (PathList.Count == 0 || TargetNodeIndex >= PathList.Count)
                return;

            DrawPathLine();

            var distToTarget = (Vector2)(PathList[TargetNodeIndex].location + randPosOffset) - _rigidbody2D.position;
            if (distToTarget.magnitude < 0.05f)
            {
                TargetNodeIndex++;
                if (TargetNodeIndex >= PathList.Count)
                {
                    MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(UnitUID, new Vector2Int(PathList[PathList.Count - 1].x, PathList[PathList.Count - 1].y)));
                    fsm.ChangeState(UnitStates.Idle);
                    return;
                }
                else if (gridMap.Tiles[PathList[TargetNodeIndex].x, PathList[TargetNodeIndex].y].IsBlock())
                {
                    ChangeIdleState();
                    Debug.Log("Next Tile Is Block");
                    return;
                }
                else
                {
                    MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(UnitUID, new Vector2Int(PathList[PathList.Count - 1].x, PathList[PathList.Count - 1].y)));
                }
            }

            var targetNode = PathList[TargetNodeIndex];
            Vector2 newPos = Vector2.MoveTowards(_rigidbody2D.position, (Vector2)(targetNode.location + randPosOffset), _forwardSpeed * Time.deltaTime);
            _rigidbody2D.MovePosition(newPos);
            FlipRenderers(_rigidbody2D.position.x <= targetNode.location.x + randPosOffset.x);
        }
        protected void MoveToHome_Exit()
        {

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

            int nextNodeIndex = TargetNodeIndex + 1;
            if (nextNodeIndex >= PathList.Count)
            {
                return new Vector2Int(-1, -1);
            }
            return new Vector2Int(PathList[nextNodeIndex].x, PathList[nextNodeIndex].y);
        }

        private void MoveEvent()
        {
            if (PathList.Count == 0 || TargetNodeIndex >= PathList.Count)
                return;

            DrawPathLine();

            var distToTarget = (Vector2)(PathList[TargetNodeIndex].location + randPosOffset) - _rigidbody2D.position;
            if (distToTarget.magnitude < 0.05f)
            {
                TargetNodeIndex++;
                if (TargetNodeIndex >= PathList.Count)
                {
                    MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(UnitUID, new Vector2Int(PathList[PathList.Count - 1].x, PathList[PathList.Count - 1].y)));
                    fsm.ChangeState(UnitStates.Attack);
                    return;
                }
                else if (DirtyPath)
                {
                    ChangeIdleState();
                }
                else if (gridMap.Tiles[PathList[TargetNodeIndex].x, PathList[TargetNodeIndex].y].IsBlock())
                {
                    ChangeIdleState();
                    Debug.Log("Next Tile Is Block");
                    return;
                }
                else
                {
                    MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(UnitUID, new Vector2Int(PathList[PathList.Count - 1].x, PathList[PathList.Count - 1].y)));
                }
            }

            var targetNode = PathList[TargetNodeIndex];
            Vector2 newPos;
            if (isBoidsAlgorithm)
            {
                var velocity = CalculateBoidsAlgorithm((Vector2)targetNode.location + (Vector2)randPosOffset);
                newPos = Vector2.MoveTowards(_rigidbody2D.position, _rigidbody2D.position + velocity, _forwardSpeed * Time.deltaTime);
            }
            else
            {
                newPos = Vector2.MoveTowards(_rigidbody2D.position, (Vector2)(targetNode.location + randPosOffset), _forwardSpeed * Time.deltaTime);
            }
            _rigidbody2D.MovePosition(newPos);
            FlipRenderers(_rigidbody2D.position.x <= targetNode.location.x + randPosOffset.x);
        }

        private bool HasTileInPath(Vector2Int _tile)
        {
            for (int i = TargetNodeIndex; i < PathList.Count; i++)
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

}
