using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FT;
using UniRx;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.Rendering;

public class BaseObj : Boids2D
{
    public class Driver
    {
        public StateEvent Update;
        public StateEvent FixedUpdate;
    }

    public int TID { get; set; }
    // Component
    protected Canvas canvas;
    protected SortingGroup sortingGroup;
    private Slider hpBar;

    protected Animator animator;
    protected AnimationLink animationLink;
    private Transform renderRoot;

    private AbsPathFinder pathFinder;
    protected List<PathNode> pathList;
    protected int targetNodeIndex;
    protected int currNodeIndex;
    protected GridMap gridMap;
    protected TileObject startTile;
    private TileObject endTile;
    
    public long UnitUID { get { return unitData.uid; } }
    public int currTileX { get; protected set; }
    public int currTileY { get; protected set; }

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
        sortingGroup = GetComponent<SortingGroup>();

        hpBar = canvas.GetComponentInChildren<Slider>();
    }

    protected virtual void ChangeIdleState() { }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
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
            if (SS.UserDataManager.Instance.GetEnemyData(targetObj.UnitUID) == null)
            {
                ChangeIdleState();
                
            }
        }
        else
        {
            SS.GameManager.Instance.EnemyAttackHero(targetObj.UnitUID);
            if (SS.UserDataManager.Instance.GetBattleHeroData(targetObj.UnitUID) == null)
            {
                ChangeIdleState();
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

    protected bool HasPath(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        SetAStarPath(_startX, _startY, _endX, _endY, _passBuilding);
        return pathFinder.FindPath().Count > 0;
    }

    protected void RefreshPath(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
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
            unitData = SS.UserDataManager.Instance.GetBattleHeroData(_unitUID);
        }
        else
        {
            unitData = SS.UserDataManager.Instance.GetEnemyData(_unitUID);
        }
        
        gridMap = _mapCreator;
        pathFinder = new AStarPathFinder(this);
        pathFinder.SetNode2Pos(_mapCreator.Node2Pos);
        currNodeIndex = -1;
        targetObj = null;

        endTile = gridMap.Tiles[_endTile.x, _endTile.y];
        pathFinder.InitMap(_mapCreator.gridCol, _mapCreator.gridRow);
        //jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
        //jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);

        currTileX = _startTile.x;
        currTileY = _startTile.y;
        if (unitData.refData.unit_type == UNIT_TYPE.BUILDING)
        {
            transform.position = (Vector2)gridMap.Node2Pos(_startTile.x, _startTile.y);
        }
        else
        {
            transform.position = (Vector2)gridMap.Node2Pos(_startTile.x, _startTile.y) + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        }
        
        UpdateUI();
        ChangeIdleState();
        if (unitData.refData.unit_type == UNIT_TYPE.BUILDING)
        {
            var currTile = gridMap.Tiles[currTileX, currTileY];
            currTile.SetTileType(TileType.Building);
        }
    }


    protected void DrawPathLine()
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

    protected BaseObj SearchNearestOpponent(bool _includeBuilding)
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
                if (SS.UserDataManager.Instance.GetEnemyData(opponentObj.Key) == null)
                {
                    Debug.LogError($"battleEnemyDataDic not found {opponentObj.Key}");
                    continue;
                }
            }
            else
            {
                if (SS.UserDataManager.Instance.GetBattleHeroData(opponentObj.Key) == null)
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

    protected bool CheckTargetRange()
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

    protected void ShowCanvas()
    {
        canvas?.gameObject.SetActive(true);
    }

    protected void HideCanvase()
    {
        canvas?.gameObject.SetActive(false);
    }

}
