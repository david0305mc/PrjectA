using Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : SingletonMono<CameraManager>
{
    private static Vector3 PositiveInfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    private Camera mainCamera;

    private Vector3 newPos;
    private Vector3 oldPos;
    private Vector3 dragStartInputPos;
    private bool dragStarted;

    [SerializeField] private Transform worldRoot;
    private BaseObj selectedObject;
    public Vector2Int TestTarget = new Vector2Int(0, 0);

    private void Update()
    {
        UpdateOneTouch();
    }


    protected override void OnSingletonAwake()
    {
        base.OnSingletonAwake();
        mainCamera = Camera.main;
        newPos = transform.position;
        oldPos = newPos;

    }

    private void UpdateOneTouch()
    {
        if (Input.touchCount > 1)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            var uiUnitSlot = TryGetRayCastUIItem(Input.mousePosition, GameConfig.BattleSlotLayerName);
            if (uiUnitSlot != null)
            {
                //uiUnitSlot.UnitTID
                //selectedObject = hitObj;

                var unitData = SS.UserDataManager.Instance.GetHeroData(uiUnitSlot.UnitUID);
                dragStartInputPos = Input.mousePosition;
                dragStarted = true;
                GameObject unitPrefab = MResourceManager.Instance.GetPrefab(unitData.refData.prefabname);
                selectedObject = Lean.Pool.LeanPool.Spawn(unitPrefab, SS.GameManager.Instance.GridMap.ObjectField).GetComponent<BaseObj>();
                selectedObject.TID = unitData.refData.id;
                Vector3 hitPoint = TryGetRayCastHitPoint(Input.mousePosition, GameConfig.GroundLayerMask);
                selectedObject.transform.position = (Vector2)hitPoint;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (dragStarted)
            {
                newPos = Input.mousePosition - dragStartInputPos;
                //selectedObject.transform.position = (Vector2)newPos;
                Vector3 hitPoint = TryGetRayCastHitPoint(Input.mousePosition, GameConfig.GroundLayerMask);
                selectedObject.transform.position = (Vector2)hitPoint;

                //if (!hitPoint.Equals(PositiveInfinityVector))
                //{

                //    //newPos = transform.position + dragStartPos - hitPoint;
                //    //panVelocity = (previousPanPoint - newPos) * panInertiafactor;
                //    //previousPanPoint = newPos;
                //    //if (!newPos.Equals(oldPos))
                //    //{
                //    //    transform.position = newPos;
                //    //    oldPos = transform.position;
                //    //}
                //}
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragStarted)
            {
                dragStarted = false;
                // selectedObject Targeting Move
                var obj = TryGetRayCastObject(Input.mousePosition, GameConfig.TileLayerMask);
                if (obj != null)
                {
                    var tileObj = obj.GetComponent<TileObject>();
                    selectedObject.DragToTarget(tileObj.transform.position, ()=> {
                        SS.GameManager.Instance.AddBattleHeroObj(selectedObject, selectedObject.TID, tileObj.X, tileObj.Y);
                        selectedObject = null;
                    });
                }
                else
                {
                    Lean.Pool.LeanPool.Despawn(selectedObject);
                    selectedObject = null;
                }
            }
            else
            {
                var obj = TryGetRayCastObject(Input.mousePosition, GameConfig.TileLayerMask);
                if (obj != null)
                {
                    var tileObj = obj.GetComponent<TileObject>();
                    TestTarget = new Vector2Int(tileObj.X, tileObj.Y);
                    MessageDispather.Publish(EMessage.UpdateTile, 1);
                }
            }
            //groundDragStarted = false;
            //dragStartPos = PositiveInfinityVector;
        }

    }

    public Vector3 TryGetRayCastHitPoint(Vector2 _touchPoint, int _layerMask)
    {
        var mousePos = mainCamera.ScreenToWorldPoint(_touchPoint);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15f, layerMask: _layerMask);
        if (hit)
        {
            return hit.point;
        }
        else
        {
            return PositiveInfinityVector;
        }
    }


    public GameObject TryGetRayCastObject(Vector2 _touchPoint, int _layerMask)
    {
        var mousePos = mainCamera.ScreenToWorldPoint(_touchPoint);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15f, layerMask: _layerMask);
        if (hit && hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    public UIUnitSlot TryGetRayCastUIItem(Vector2 _touchPoint, string _layerName)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(_touchPoint.x, _touchPoint.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        var itemLayerList = results.Where(item => item.gameObject.layer == LayerMask.NameToLayer(_layerName) && item.gameObject.GetComponent<UIUnitSlot>() != null);
        if (itemLayerList.Count() > 0)
        {
            return itemLayerList.First().gameObject.GetComponent<UIUnitSlot>();
        }

        return null;
    }

    public bool IsUsingUI()
    {
        //return EventSystem.current.IsPointerOverGameObject();
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (results.Count > 0)
        {
            int count = 0;
            foreach (var item in results)
            {
                if (item.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    count++;
                }
                if (item.gameObject.layer == LayerMask.NameToLayer("HUD"))
                {
                    continue;
                }
            }
            return count > 0;
        }
        else
        {
            return false;
        }
    }
}
