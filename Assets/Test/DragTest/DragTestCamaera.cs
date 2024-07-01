using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.EventSystems;
using System.Linq;

public class DragTestCamaera : SingletonMono<DragTestCamaera>
{
    private static Vector3 PositiveInfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    private Camera mainCamera;

    private Vector3 newPos;
    private Vector3 oldPos;
    private Vector3 dragStartInputPos;
    private bool dragStarted;
    
    [SerializeField] private UnitIconTest unitIconPrefab;
    [SerializeField] private Transform worldRoot;
    private UnitIconTest selectedObject;


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
            GameObject hitObj = TryGetRayCastUIItem(Input.mousePosition);
            if (hitObj != null)
            {
                //selectedObject = hitObj;
                
                dragStartInputPos = Input.mousePosition;
                dragStarted = true;

                selectedObject = Lean.Pool.LeanPool.Spawn(unitIconPrefab, this.transform);
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
                var tileObj = TryGetRayCastObject(Input.mousePosition, GameConfig.TileLayerMask);
                if (tileObj != null)
                {
                    selectedObject.MoveToTarget(tileObj.transform.position);
                }
                else
                {
                    Lean.Pool.LeanPool.Despawn(selectedObject);
                }
                selectedObject = null;
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

    public GameObject TryGetRayCastUIItem(Vector2 _touchPoint)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        var itemLayerList = results.Where(item => item.gameObject.layer == LayerMask.NameToLayer("Item"));
        if (itemLayerList.Count() > 0)
        {
            return itemLayerList.First().gameObject;
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
