using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FT;
using UniRx;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class BaseObj : Boids2D
{
    public class Driver
    {
        public StateEvent Update;
        public StateEvent FixedUpdate;
    }

    public int TID { get; set; }
    // Component
    private Canvas canvas;
    private Slider hpBar;

    protected Animator animator;
    protected AnimationLink animationLink;
    private Transform renderRoot;

    protected StateMachine<UnitStates, Driver> fsm;
    private AbsPathFinder pathFinder;
    List<PathNode> pathList;
    private int targetNodeIndex;
    private int currNodeIndex;
    private GridMap gridMap;
    private TileObject startTile;
    private TileObject endTile;
    private bool isActive;
    public long UnitUID { get { return unitData.uid; } }
    public int currTileX { get; private set; }
    public int currTileY { get; private set; }
    private CompositeDisposable compositeDisposable;
    protected BaseObj targetObj;      // null???? endTile
    private float attackDelay;
    protected SS.UnitData unitData;
    protected bool isHero;

    

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animationLink = animator.GetComponent<AnimationLink>();
            renderRoot = animator.transform;
        }
        canvas = GetComponentInChildren<Canvas>();
        hpBar = canvas.GetComponentInChildren<Slider>();
    }
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

    protected void PlayAni(string str)
    {
        if (animator == null)
            return;
        //ResetTrigger();
        //animator.SetTrigger(str);

        animator.Play(str);
        animator.Update(0);
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
        else if(targetObj == null)
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
            if (!isActive)
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
    protected void Move_Exit()
    {
        compositeDisposable?.Clear();
    }
    //protected void Move_FixedUpdate()
    //{
    //    Debug.Log("Move_FixedUpdate");
    //}
    protected void Attack_Enter()
    {
        Debug.Log("Attack_Enter");
        attackDelay = 0f;
        PlayAni("Attack");
        FlipRenderers(transform.position.x <= targetObj.transform.position.x);
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

    protected void DoAttack()
    {
        if (targetObj == null)
        {
            return;
        }
        if (isHero)
        {
            SS.GameManager.Instance.HeroAttackEnemy(targetObj.UnitUID);
            if (SS.UserData.Instance.GetEnemyData(targetObj.UnitUID) == null)
            {
                fsm.ChangeState(UnitStates.Idle);
            }
        }
        else
        {
            SS.GameManager.Instance.EnemyAttackHero(targetObj.UnitUID);
            if (SS.UserData.Instance.GetHeroData(targetObj.UnitUID) == null)
            {
                fsm.ChangeState(UnitStates.Idle);
            }
        }
    }

    private void SetAStarPath(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        pathFinder.Clear();
        pathFinder.SetStartNode(_startX, _startY);
        pathFinder.SetEndNode(_endX, _endY);

        foreach (var item in gridMap.Tiles)
        {
            if (item.X == _endX && item.Y == _endY)
            {
                pathFinder.RefreshWalkable(item.X, item.Y, true);
            }
            else
            {
                switch (item.tileType)
                {
                    case TileType.Block:
                        pathFinder.RefreshWalkable(item.X, item.Y, false);
                        break;
                    case TileType.Building:
                        pathFinder.RefreshWalkable(item.X, item.Y, _passBuilding);
                        break;
                    default:
                        pathFinder.RefreshWalkable(item.X, item.Y, true);
                        break;
                }
            }   
        }
    }

    private bool HasPath(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        SetAStarPath(_startX, _startY, _endX, _endY, _passBuilding);
        return pathFinder.FindPath().Count > 0;
    }
    private void RefreshPath(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        SetAStarPath(_startX, _startY, _endX, _endY, _passBuilding);
        startTile = gridMap.Tiles[_startX, _startY];
        startTile.SetCurrNodeMark(true);
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
    public void InitData(bool _isHero, long _unitUID, GridMap _mapCreator, Vector2Int _startTile, Vector2Int _endTile)
    {
        isHero = _isHero;
        if (_isHero)
        {
            unitData = SS.UserData.Instance.GetHeroData(_unitUID);
        }
        else
        {
            unitData = SS.UserData.Instance.GetEnemyData(_unitUID);
        }
        
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
        UpdateUI();
        if (unitData.refData.unit_type != UNIT_TYPE.BUILDING)
        {
            fsm = new StateMachine<UnitStates, Driver>(this);
            fsm.ChangeState(UnitStates.Idle);
        }
        else
        {
            var currTile = gridMap.Tiles[currTileX, currTileY];
            currTile.SetTileType(TileType.Building);
        }
    }


    private void DrawPathLine()
    {
        for (int i = 0; i < pathList.Count - 1; i++)
        {
            Debug.DrawLine(pathList[i].location, pathList[i + 1].location, Color.red);
        }
    }

    //private UnitObj SearchTarget()
    //{
    //    UnitObj obj = SearchNearestOpponent();
    //    if (obj != null && !HasPath(currTileX, currTileY, obj.currTileX, obj.currTileY))
    //    {
    //        obj = SearchNearestOpponent(true);
    //    }
    //    return obj;
    //}

    private BaseObj SearchNearestOpponent(bool _includeBuilding)
    {
        BaseObj targetObj = default;
        float distTarget = 0;

        Dictionary<long, BaseObj> opponentObjDic;
        if (isHero)
        {
            opponentObjDic = SS.GameManager.Instance.EnemyObjDic;
        }
        else
        {
            opponentObjDic = SS.GameManager.Instance.HeroObjDic;
        }

        foreach (var opponentObj in opponentObjDic)
        {
            //if (opponentObj != null)

            if (isHero)
            {
                if (SS.UserData.Instance.GetEnemyData(opponentObj.Key) == null)
                {
                    Debug.LogError($"battleEnemyDataDic not found {opponentObj.Key}");
                    continue;
                }
            }
            else
            {
                if (SS.UserData.Instance.GetHeroData(opponentObj.Key) == null)
                {
                    Debug.LogError($"heroDataDic not found {opponentObj.Key}");
                    continue;
                }
            }

            if (!_includeBuilding)
            {
                if (opponentObj.Value.unitData.refData.unit_type == UNIT_TYPE.BUILDING)
                {
                    continue;
                }
            }
            

            float dist = Vector2.Distance(opponentObj.Value.transform.position, transform.position);
            if (targetObj == default)
            {
                targetObj = opponentObj.Value;
                distTarget = dist;
            }
            else
            {
                if (distTarget > dist)
                {
                    // change Target
                    targetObj = opponentObj.Value;
                    distTarget = dist;
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

    protected virtual void FlipRenderers(bool right)
    {
        if (right)
        {
            renderRoot.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            renderRoot.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void GetAttacked()
    {
        UpdateUI();
    }


    protected void UpdateUI()
    {
        if (hpBar == null)
            return;
        hpBar.value = (float)unitData.hp / unitData.maxHp;
    }

    public void DragToTarget(Vector2 _target, System.Action _callback)
    {

        //fsm = StateMachine<UnitStates>.Initialize(this, UnitStates.Drag);

        UniTask.Create(async () =>
        {
            while (Vector2.Distance(transform.position, _target) > 0.1f)
            {
                await UniTask.Yield();
                var newPos = Vector2.MoveTowards(transform.position, _target, 3f * Time.deltaTime);
                transform.position = newPos;
            }
            _callback?.Invoke();
        });
        transform.position = _target;
    }

}
