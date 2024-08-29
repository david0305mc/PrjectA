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

namespace SS
{
    public class BaseObj : Boids2D, IDamagable
    {
        public class Driver
        {
            public StateEvent Update;
            public StateEvent FixedUpdate;
        }

        // Component
        protected Canvas canvas;
        protected SortingGroup sortingGroup;
        private Slider hpBar;

        protected Animator animator;
        protected AnimationLink animationLink;
        private Transform renderRoot;

        protected AbsPathFinder pathFinder;
        protected AbsPathFinder pathFinderPassBuilding;
        protected List<PathNode> PathList { get; set; }
        protected int TargetNodeIndex { get; set; }
        protected GridMap gridMap;
        protected TileObject startTile;
        private System.Action<AttackData2> getAttackAction;
        protected Vector3 randPosOffset { get; set; }
        protected Vector3 OrgPos { get; private set; }

        public long UnitUID { get { return  UnitData.uid; } }
        public int currTileX
        {
            get
            {
                return gridMap.Pos2Node(transform.position).x;
            }
        }
        public int currTileY
        {
            get
            {
                return gridMap.Pos2Node(transform.position).y;
            }
        }

        protected BaseObj TargetObj { get; set; }      // null???? endTile
        private float attackDelay;
        public SS.UnitData UnitData { get; set; }
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
        protected virtual void Attack_Update()
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
            if (TargetObj == null || TargetObj.UnitData.state == UnitDataStates.Dead)
            {
                ChangeIdleState();
                return;
            }

            if (UnitData.refData.unit_type == UNIT_TYPE.ARCHER || UnitData.refData.unit_type == UNIT_TYPE.BUILDING)
            {
                SS.GameManager.Instance.LauchProjectile(this, TargetObj.UnitUID);
                return;
            }

            if (isHero)
            {
                SS.GameManager.Instance.HeroAttackEnemy(UnitUID, TargetObj.UnitUID);
                Vector2Int targetTile = new Vector2Int(TargetObj.currTileX, TargetObj.currTileY);
                var enemyData = SS.UserDataManager.Instance.GetEnemyData(TargetObj.UnitUID);
                if (enemyData == null || enemyData.state == UnitDataStates.Dead)
                {
                    MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(UnitUID, targetTile));
                    ChangeIdleState();
                }
            }
            else
            {
                SS.GameManager.Instance.EnemyAttackHero(UnitUID, TargetObj.UnitUID);
                Vector2Int targetTile = new Vector2Int(TargetObj.currTileX, TargetObj.currTileY);
                var heroData = SS.UserDataManager.Instance.GetBattleHeroData(TargetObj.UnitUID);
                if (heroData == null || heroData.state == UnitDataStates.Dead)
                {
                    MessageDispather.Publish(EMessage.UpdateTile, new EventParm<long, Vector2Int>(UnitUID, targetTile));
                    ChangeIdleState();
                }
            }
        }

        protected void SetAStarPath(int _startX, int _startY, int _endX, int _endY, bool _passBuilding)
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

        protected int GetBuildingIndexOnPath()
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
        public virtual void InitData(UnitData _unitData)
        {
            gameObject.layer = LayerMask.NameToLayer(Game.GameConfig.UnitLayerName);
            isHero = !_unitData.IsEnemy;
            UnitData = _unitData;
        }

        public void InitBattleData(GridMap _mapCreator, Vector2Int _startTile, System.Action<AttackData2> _getAttackedAction)
        {
            getAttackAction = _getAttackedAction;
            gridMap = _mapCreator;
            pathFinder = new AStarPathFinder(this);
            pathFinder.SetNode2Pos(gridMap.Node2Pos);
            pathFinder.InitMap(gridMap.gridCol, gridMap.gridRow);

            pathFinderPassBuilding = new AStarPathFinder(this);
            pathFinderPassBuilding.SetNode2Pos(gridMap.Node2Pos);
            pathFinderPassBuilding.InitMap(gridMap.gridCol, gridMap.gridRow);

            TargetObj = null;

            if (UnitData.refData.unit_type == UNIT_TYPE.BUILDING)
            {
                transform.position = (Vector2)gridMap.Node2Pos(_startTile.x, _startTile.y);
                randPosOffset = Vector2.zero;
            }
            else
            {
                randPosOffset = new Vector2(Random.Range(GameDefine.RandPosOffsetMin, GameDefine.RandPosOffsetMax), Random.Range(GameDefine.RandPosOffsetMin, GameDefine.RandPosOffsetMax));
                transform.position = gridMap.Node2Pos(_startTile.x, _startTile.y) + randPosOffset;
            }
            OrgPos = transform.position;

            UpdateUI();
            ChangeIdleState();
            SetBattleMode();
            if (UnitData.refData.unit_type == UNIT_TYPE.BUILDING)
            {
                var currTile = gridMap.Tiles[currTileX, currTileY];
                currTile.SetTileType(TileType.Building);
            }
        }


        protected void DrawPathLine()
        {
            for (int i = 0; i < PathList.Count - 1; i++)
            {
                Debug.DrawLine(PathList[i].location + randPosOffset, PathList[i + 1].location + randPosOffset, Color.red);
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
            var colliders = Physics2D.OverlapCircleAll(transform.position, 10f, GameDefine.LayerMaskUnit);
            int maxAggro = -999;
            var baseObjList = colliders.Select(item => item.GetComponent<BaseObj>()).ToList();

            List<BaseObj> tempObjList = new List<BaseObj>();
            foreach (var opponentObj in baseObjList)
            {
                if (opponentObj == default || opponentObj == this)
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
                    if (opponentObj.UnitData.refData.unit_type == UNIT_TYPE.BUILDING)
                    {
                        continue;
                    }
                }
                int aggro = opponentObj.UnitData.refData.aggroorder;
                if (aggro >= maxAggro)
                {
                    maxAggro = aggro;
                    tempObjList.Add(opponentObj);
                }
            }
            foreach (var item in tempObjList)
            {
                float dist = Vector2.Distance(item.transform.position, transform.position);
                if (distTarget > dist)
                {
                    targetObj = item;
                    distTarget = dist;
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

            if (TargetObj.UnitData.state == UnitDataStates.Dead)
            {
                ChangeIdleState();
            }

            float dist = Vector2.Distance(TargetObj.transform.position, transform.position);
            int range = UnitData.refData.unit_type == UNIT_TYPE.TANKER ? 1 : 7;

            if (dist < range)
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
            hpBar.value = (float)UnitData.hp / UnitData.maxHp;
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
        public virtual void SetUIState()
        {
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

        public void GetDamaged(AttackData2 _attackData)
        {
            getAttackAction?.Invoke(_attackData);
        }

        public bool IsEnemy()
        {
            return !IsHero; 
        }
    }

}
