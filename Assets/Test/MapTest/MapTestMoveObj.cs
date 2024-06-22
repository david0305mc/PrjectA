using FT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTestMoveObj : Boids2D
{
    private AbsPathFinder jpsPathFinder;
    List<PathNode> pathList;
    private int targetNode;
    private MapCreator mapCreator;

    public void InitData(MapCreator _mapCreator, int _startX, int _startY, int _endX, int _endY)
    {
        mapCreator = _mapCreator;
        jpsPathFinder = new JPSPathFinder(this);
        jpsPathFinder.SetNode2Pos(_mapCreator.Node2Pos);

        jpsPathFinder.InitMap(_mapCreator.gridCol, _mapCreator.gridRow);
        //jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
        //jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);
        jpsPathFinder.SetStartNode(_startX, _startY);
        jpsPathFinder.SetEndNode(_endX, _endY);
        var blockList =_mapCreator.GetBlockList();
        foreach (var item in blockList)
        {
            jpsPathFinder.RefreshWalkable(item.X, item.Y, false);
        }

        pathList = jpsPathFinder.FindPath();
        targetNode = 0;
        transform.position = (Vector2)mapCreator.Node2Pos(_startX, _startY) + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
    }

    private void FixedUpdate()
    {
        if (pathList.Count == 0 || targetNode >= pathList.Count)
            return;

        if (targetNode >= pathList.Count)
        {
            return;
        }

        var dist = (Vector2)pathList[targetNode].location - _rigidbody2D.position;
        if (dist.magnitude < 0.5f)
        {
            targetNode++;
            if (targetNode >= pathList.Count)
            {
                Debug.LogError("Complete");
                return;
            }
        }

        var target = pathList[targetNode];

        Vector2 newPos;
        if (isBoidsAlgorithm)
        {
            var velocity = CalculateBoidsAlgorithm((Vector2)target.location);
            newPos = Vector2.MoveTowards(_rigidbody2D.position, _rigidbody2D.position + velocity, _forwardSpeed * Time.fixedDeltaTime);
        }
        else
        {
            newPos = Vector2.MoveTowards(_rigidbody2D.position, (Vector2)target.location, _forwardSpeed * Time.fixedDeltaTime);
        }
        _rigidbody2D.MovePosition(newPos);
        //var movePos = rigidBody.position + (dist.normalized * speed * Time.fixedDeltaTime);

        //transform.position -= new Vector3(0, Time.fixedDeltaTime, 0);
    }
}
