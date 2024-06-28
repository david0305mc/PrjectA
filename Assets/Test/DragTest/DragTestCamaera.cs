using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class DragTestCamaera : SingletonMono<DragTestCamaera>
{

    private static Vector3 PositiveInfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    private Camera mainCamera;

    private Vector3 newPos;
    private Vector3 oldPos;
    private Vector3 dragStartInputPos;
    private bool groundDragStarted;
    private GameObject selectedObject;

    private void Update()
    {
        UpdateOneTouch();
    }


    protected override void OnSingletonAwake()
    {
        base.OnSingletonAwake();
        mainCamera = GetComponent<Camera>();
        newPos = transform.position;
        oldPos = newPos;
    }

    private void UpdateOneTouch()
    {
        if (Input.touchCount > 1)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            GameObject hitObj = TryGetRayCastObject(Input.mousePosition, GameConfig.ItemLayerMask);
            if (hitObj != null)
            {
                selectedObject = hitObj;
                dragStartInputPos = Input.mousePosition;
                groundDragStarted = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (groundDragStarted)
            {
                newPos = Input.mousePosition - dragStartInputPos;
                selectedObject.transform.position = newPos;
                //Vector3 hitPoint = TryGetRayCastHitPoint(Input.mousePosition, GameConfig.GroundLayerMask);
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
            if (Vector3.Distance(dragStartInputPos, Input.mousePosition) <= 10f)
            {
                //touchAction?.Invoke();
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

}
