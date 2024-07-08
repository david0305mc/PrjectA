using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FT;
using UniRx;
using MonsterLove.StateMachine;

public class MoveObj : Boids2D
{
    public class Driver
    {
        public StateEvent Update;
        public StateEvent FixedUpdate;
    }
    
    protected StateMachine<UnitStates, Driver> fsm;
    private AbsPathFinder pathFinder;
    List<PathNode> pathList;
    private int targetNodeIndex;
    private int currNodeIndex;
    private GridMap gridMap;
    private TileObject startTile;
    private TileObject endTile;
    private bool isActive;
    public long UnitUID;
    private int currTileX;
    private int currTileY;
    private CompositeDisposable compositeDisposable;
    protected MoveObj targetObj;      // null???? endTile
    private float attackDelay;

    private void Update()
    {
        if (fsm == null)
            return;
        fsm.Driver.Update.Invoke();
    }

    private void FixedUpdate()
    {
        if (fsm == null)
            return;
        fsm.Driver.FixedUpdate.Invoke();
    }
    protected void Idle_Enter()
    {
        Debug.Log("Idle_Enter");
    }
    protected void Idle_Update()
    {
        HeroObj targetEnemy = SearchEnemy();
        
        if (targetEnemy != default)
        {
            targetObj = targetEnemy;


            // GetOuterCells
            // finding nearest outer cell

            RefreshPath(currTileX, currTileY, targetEnemy.TileX, targetEnemy.TileY);
            fsm.ChangeState(UnitStates.Move);

            //compositeDisposable?.Clear();
            //compositeDisposable = new CompositeDisposable();
            //MessageDispather.Receive<int>(EMessage.UpdateTile).Subscribe(_ =>
            //{
            //    if (!isActive)
            //        return;

            //    int x = currNodeIndex == -1 ? startTile.X : pathList[currNodeIndex].x;
            //    int y = currNodeIndex == -1 ? startTile.Y : pathList[currNodeIndex].y;

            //    Debug.Log($"MessageDispather.Receive {currNodeIndex}");
            //    RefreshPath(x, y, endTile.X, endTile.Y);

            //}).AddTo(compositeDisposable);
        }
    }
    protected void Move_Enter()
    {
        Debug.Log("Move_Enter");
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
    //protected void Move_FixedUpdate()
    //{
    //    Debug.Log("Move_FixedUpdate");
    //}
    protected void Attack_Enter()
    {
        Debug.Log("Attack_Enter");
        attackDelay = 3f;
    }
    protected void Attack_Update()
    {
        attackDelay -= Time.deltaTime;
        if (attackDelay <= 0)
        {
            attackDelay = 1f;
            
            DoAttack();
        }
    }

    protected virtual void DoAttack()
    {
        Debug.Log("Attack");
        fsm.ChangeState(UnitStates.Idle);
    }

    private void RefreshPath(int _startX, int _startY, int _endX, int _endY)
    {
        pathFinder.Clear();
        pathFinder.SetStartNode(_startX, _startY);
        pathFinder.SetEndNode(_endX, _endY);

        startTile = gridMap.Tiles[_startX, _startY];
        startTile.SetCurrNodeMark(true);

        foreach (var item in gridMap.Tiles)
        {
            if (item.tileType == TileType.Block)
            {
                pathFinder.RefreshWalkable(item.X, item.Y, false);
            }
            else
            {
                pathFinder.RefreshWalkable(item.X, item.Y, true);
            }
        }

        pathList = pathFinder.FindPath();
        targetNodeIndex = 0;
        currNodeIndex = -1;

        if (targetNodeIndex < pathList.Count)
        {
            var distToTarget = (Vector2)pathList[targetNodeIndex].location - _rigidbody2D.position;
            Vector2 distBetweenNode;
            if (currNodeIndex == -1)
            {
                distBetweenNode = (Vector2)startTile.transform.position - (Vector2)pathList[0].location;
            }
            else
            {
                distBetweenNode = (Vector2)pathList[currNodeIndex].location - (Vector2)pathList[currNodeIndex + 1].location;
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
        }
    }

    public void InitData(long _unitUID, GridMap _mapCreator, Vector2Int _startTile, Vector2Int _endTile)
    {
        UnitUID = _unitUID;
        gridMap = _mapCreator;
        pathFinder = new AStarPathFinder(this);
        pathFinder.SetNode2Pos(_mapCreator.Node2Pos);
        currNodeIndex = -1;
        isActive = true;
        targetObj = null;

        endTile = gridMap.Tiles[_endTile.x, _endTile.y];
        pathFinder.InitMap(_mapCreator.gridCol, _mapCreator.gridRow);
        //jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
        //jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);

        currTileX = _startTile.x;
        currTileY = _startTile.y;
        transform.position = (Vector2)gridMap.Node2Pos(_startTile.x, _startTile.y) + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        fsm = new StateMachine<UnitStates, Driver>(this);
        fsm.ChangeState(UnitStates.Idle);
    }


    private void DrawPathLine()
    {
        for (int i = 0; i < pathList.Count - 1; i++)
        {
            Debug.DrawLine(pathList[i].location, pathList[i + 1].location, Color.red);
        }
    }

    private HeroObj SearchEnemy()
    {
        HeroObj targetObj = default;
        float distTarget = 0;
        //var detectedObjs = Physics2D.OverlapCircleAll(transform.position, 5, Game.GameConfig.UnitLayerMask);
        foreach (var enemyObj in SS.GameManager.Instance.HeroObjDic.Values)
        {
            if (enemyObj != null)
            {
                if (!SS.UserData.Instance.battleHeroDataDic.ContainsKey(enemyObj.UnitUID))
                {
                    Debug.LogError($"battleHeroDataDic not found {enemyObj.UnitUID}");
                    continue;
                }
                float dist = Vector2.Distance(enemyObj.transform.position, transform.position);
                if (targetObj == default)
                {
                    targetObj = enemyObj;
                    distTarget = dist;
                }
                else
                {
                    if (distTarget > dist)
                    {
                        // change Target
                        targetObj = enemyObj;
                        distTarget = dist;
                    }
                }
            }
        }
        return targetObj;
    }

    private bool CheckTargetRange()
    {
        if (targetObj == null)
            return false;

        float dist = Vector2.Distance(targetObj.transform.position, transform.position);
        if (dist < 2f)
        {
            return true;
        }
        return false;
    }
    private void MoveEvent()
    {
        if (pathList.Count == 0 || targetNodeIndex >= pathList.Count)
            return;

        if (!isActive)
        {
            Debug.LogError("is InActive");
            return;
        }
        DrawPathLine();


        var distToTarget = (Vector2)pathList[targetNodeIndex].location - _rigidbody2D.position;
        if (distToTarget.magnitude < 0.05f)
        {
            targetNodeIndex++;

            if (targetNodeIndex >= pathList.Count)
            {
                Debug.LogError("Complete");
                Lean.Pool.LeanPool.Despawn(gameObject);
                isActive = false;
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
        _rigidbody2D.MovePosition(newPos);
        //var movePos = rigidBody.position + (dist.normalized * speed * Time.fixedDeltaTime);

        //transform.position -= new Vector3(0, Time.fixedDeltaTime, 0);
    }
}
