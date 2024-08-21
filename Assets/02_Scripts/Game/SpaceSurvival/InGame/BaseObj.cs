using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FT;
using UniRx;
using MonsterLove.StateMachine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.Rendering;
using System.Linq;

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
    private AbsPathFinder pathFinderPassBuilding;
    protected List<PathNode> PathList { get; set; }
    protected int targetNodeIndex;
    protected int currNodeIndex;
    protected GridMap gridMap;
    protected TileObject startTile;
    private TileObject endTile;
    protected Vector2 randPosOffset;        // ???? ?????? ???? ???? ??????
    protected bool isBlocked;

    public long UnitUID { get { return unitData.uid; } }
    public int currTileX { get; protected set; }
    public int currTileY { get; protected set; }

    protected BaseObj TargetObj { get; set; }      // null???? endTile
    private float attackDelay;
    public SS.UnitData unitData;
    protected bool isHero;
    public bool IsHero => isHero;
    

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
        randPosOffset = Vector2.zero;
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
    protected virtual void Attack_Enter()
    {
        //Debug.Log("Attack_Enter");
        attackDelay = 0f;
        PlayAni("Attack");
        FlipRenderers(transform.position.x <= TargetObj.transform.position.x);
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

    protected virtual void Attack_Exit()
    { 
    
    }

    protected virtual void DoAttack()
    {
        if (TargetObj == null)
        {
            return;
        }
        if (isHero)
        {
            SS.GameManager.Instance.HeroAttackEnemy(UnitUID, TargetObj.UnitUID);
            if (SS.UserDataManager.Instance.GetEnemyData(TargetObj.UnitUID) == null)
            {
                ChangeIdleState();
            }
        }
        else
        {
            SS.GameManager.Instance.EnemyAttackHero(UnitUID, TargetObj.UnitUID);
            Vector2Int targetTile = new Vector2Int(TargetObj.currTileX, TargetObj.currTileY);
            if (SS.UserDataManager.Instance.GetBattleHeroData(TargetObj.UnitUID) == null)
            {
                MessageDispather.Publish(EMessage.UpdateTile, targetTile);
                ChangeIdleState();
            }
        }
    }

    //protected List<PathNode> GeneratePath(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    //{
    //    pathFinder.SetStartNode(_startX, _startY);
    //    pathFinder.SetEndNode(_endX, _endY);

    //    foreach (var item in gridMap.Tiles)
    //    {
    //        switch (item.tileType)
    //        {
    //            case TileType.Block:
    //                pathFinder.RefreshWalkable(item.X, item.Y, false);
    //                break;
    //            case TileType.Building:
    //                pathFinder.RefreshWalkable(item.X, item.Y, _passBuilding);
    //                break;
    //            default:
    //                pathFinder.RefreshWalkable(item.X, item.Y, true);
    //                break;
    //        }
    //    }
    //    return pathFinder.FindPath();
    //}
    private void SetAStarPath(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        try
        {
            pathFinder.Clear();
        }
        catch
        {
            Debug.Log("check");
        }

        pathFinder.SetStartNode(_startX, _startY);
        pathFinder.SetEndNode(_endX, _endY);

        foreach (var item in gridMap.Tiles)
        {
            if (item.X == _endX && item.Y == _endY)
            {
                pathFinder.RefreshWalkable(item.X, item.Y, true);
                continue;
            }
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
    private void SetAStarPathWithBuilding(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        pathFinderPassBuilding.Clear();
        pathFinderPassBuilding.SetStartNode(_startX, _startY);
        pathFinderPassBuilding.SetEndNode(_endX, _endY);

        foreach (var item in gridMap.Tiles)
        {
            switch (item.tileType)
            {
                case TileType.Block:
                    pathFinderPassBuilding.RefreshWalkable(item.X, item.Y, false);
                    break;
                case TileType.Building:
                    pathFinderPassBuilding.RefreshWalkable(item.X, item.Y, _passBuilding);
                    break;
                default:
                    pathFinderPassBuilding.RefreshWalkable(item.X, item.Y, true);
                    break;
            }
        }
    }

    protected int GetPathCount(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        if (_startX < 0 || _startY < 0 || _endX < 0 || _endY < 0)
            return 0;

        if (_startX >= gridMap.gridCol || _endX >= gridMap.gridCol)
            return 0;
        if (_startY >= gridMap.gridRow || _endY >= gridMap.gridRow)
            return 0;

        try
        {
            SetAStarPath(_startX, _startY, _endX, _endY, _passBuilding);
        }
        catch
        {
            Debug.LogError($"_startX {_startX}, _startY {_startY}, _endX {_endX}, _endY {_endY}, _passBuilding");
        }
        
        return pathFinder.FindPath().Count;
    }

    protected bool HasPath(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
    {
        return GetPathCount(_startX, _startY, _endX, _endY, _passBuilding) > 0;
    }

    private int GetBuildingIndexOnPath()
    {
        for (int i = 0; i < PathList.Count; i++)
        {
            if (gridMap.Tiles[PathList[i].x, PathList[i].y].tileType == TileType.Building)
            {
                return i;
            }
        }
        return -1;
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

                if (PathList.Count > 0)
                {
                    // To Do : Check Next Is Building
                    if (gridMap.Tiles[PathList[0].x, PathList[0].y].tileType == TileType.Building)
                    {
                        isBlocked = true;
                    }
                }
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
    public virtual void InitData(bool _isHero, long _unitUID, GridMap _mapCreator, Vector2Int _startTile, Vector2Int _endTile)
    {
        gameObject.layer = LayerMask.NameToLayer(Game.GameConfig.UnitLayerName);
        isHero = _isHero;
        if (_isHero)
        {
            unitData = SS.UserDataManager.Instance.GetBattleHeroData(_unitUID);
        }
        else
        {
            unitData = SS.UserDataManager.Instance.GetEnemyData(_unitUID);
        }

        if (unitData == null)
        {
            Debug.LogError("unitData == null");
        }
            
        gridMap = _mapCreator;
        pathFinder = new AStarPathFinder(this);
        pathFinder.SetNode2Pos(gridMap.Node2Pos);
        pathFinder.InitMap(gridMap.gridCol, gridMap.gridRow);

        pathFinderPassBuilding  = new AStarPathFinder(this);
        pathFinderPassBuilding.SetNode2Pos(gridMap.Node2Pos);
        pathFinderPassBuilding.InitMap(gridMap.gridCol, gridMap.gridRow);

        currNodeIndex = -1;
        TargetObj = null;
        endTile = gridMap.Tiles[_endTile.x, _endTile.y];
        //jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
        //jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);

        currTileX = _startTile.x;
        currTileY = _startTile.y;
        if (unitData.refData.unit_type == UNIT_TYPE.BUILDING)
        {
            transform.position = (Vector2)gridMap.Node2Pos(_startTile.x, _startTile.y);
            randPosOffset = Vector2.zero;
        }
        else
        {
            randPosOffset = new Vector2(Random.Range(GameDefine.RandPosOffsetMin, GameDefine.RandPosOffsetMax), Random.Range(GameDefine.RandPosOffsetMin, GameDefine.RandPosOffsetMax));
            transform.position = (Vector2)gridMap.Node2Pos(_startTile.x, _startTile.y) + randPosOffset;
        }
        
        UpdateUI();
        ChangeIdleState();
        SetBattleMode();
        if (unitData.refData.unit_type == UNIT_TYPE.BUILDING)
        {
            var currTile = gridMap.Tiles[currTileX, currTileY];
            currTile.SetTileType(TileType.Building);
        }
    }


    protected void DrawPathLine()
    {
        for (int i = 0; i < PathList.Count - 1; i++)
        {
            Debug.DrawLine(PathList[i].location, PathList[i + 1].location, Color.red);
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
        float distTarget = float.MaxValue;
        var colliders = Physics2D.OverlapCircleAll(transform.position, 6f, GameDefine.LayerMaskUnit);
        int maxAggro = -999;
        var baseObjList = colliders.Select(item => item.GetComponent<BaseObj>()).ToList();

        foreach (var opponentObj in baseObjList)
        {
            if (opponentObj == default)
            {
                continue;
            }

            if (isHero)
            {
                if (opponentObj.IsHero)
                    continue;
                
                if (SS.UserDataManager.Instance.GetEnemyData(opponentObj.UnitUID) == null)
                {
                    Debug.LogError($"battleEnemyDataDic not found {opponentObj.UnitUID}");
                    continue;
                }
            }
            else
            {
                if (!opponentObj.IsHero)
                    continue;

                if (SS.UserDataManager.Instance.GetBattleHeroData(opponentObj.UnitUID) == null)
                {
                    Debug.LogError($"heroDataDic not found {opponentObj.UnitUID}");
                    continue;
                }
            }

            if (!_includeBuilding)
            {
                if (opponentObj.unitData.refData.unit_type == UNIT_TYPE.BUILDING)
                {
                    continue;
                }
            }
            
            int aggro = opponentObj.unitData.refData.aggroorder;
            if (aggro >= maxAggro)
            {
                maxAggro = aggro;
                float dist = Vector2.Distance(opponentObj.transform.position, transform.position);
                if (distTarget > dist)
                {
                    // change Target
                    targetObj = opponentObj;
                    distTarget = dist;
                }
            }
        }
        return targetObj;
    }

    protected bool CheckTargetRange()
    {
        if (TargetObj == null)
        {
            Debug.LogError("targetObj == null");
            return false;
        }
        
        float dist = Vector2.Distance(TargetObj.transform.position, transform.position);
        if (dist < 1.5f)
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

    public virtual void SetUIMode(int _sortingOrder)
    {
        sortingGroup.sortingLayerName = Game.GameConfig.UILayerName;
        sortingGroup.sortingOrder = _sortingOrder;

        HideCanvase();
        transform.SetScale(200f);
        PlayAni("Idle");
    }
    public virtual void SetBattleMode()
    {
        sortingGroup.sortingLayerName = Game.GameConfig.ForegroundLayerName;
        sortingGroup.sortingOrder = 0;
        //attackDelay = 0f;
        HideCanvase();
        //SetSelected(false);
        transform.SetScale(1f);
    }
}
