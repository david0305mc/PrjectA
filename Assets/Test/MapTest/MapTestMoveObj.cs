using FT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MapTestMoveObj : Boids2D
{
    private AbsPathFinder pathFinder;
    List<PathNode> pathList;
    private int targetNode;
    private MapCreator mapCreator;
    TEST.MapTestObj currTile;
    TEST.MapTestObj endTile;

    private void Awake()
    {
        MessageDispather.Receive<int>(EMessage.UpdateTile).Subscribe(_ =>
        {
            //RefreshPath();

            //var currNode = pathList[targetNode];
            //var endNode = pathList[pathList.Count - 1];
            
            RefreshPath(currTile.X, currTile.Y, endTile.X, endTile.Y);

        }).AddTo(gameObject);
    }
    private void RefreshPath(int _startX, int _startY, int _endX, int _endY)
    {
        pathFinder.Clear();
        pathFinder.SetStartNode(_startX, _startY);
        pathFinder.SetEndNode(_endX, _endY);

        foreach (var item in mapCreator.Tiles)
        {
            if (item.tileType == TEST.TileType.Block)
            {
                pathFinder.RefreshWalkable(item.X, item.Y, false);
            }
            else
            {
                pathFinder.RefreshWalkable(item.X, item.Y, true);
            }
        }
        
        pathList = pathFinder.FindPath();
        targetNode = 0;
    }

    public void InitData(MapCreator _mapCreator, int _startX, int _startY, int _endX, int _endY)
    {
        mapCreator = _mapCreator;
        pathFinder = new AStarPathFinder(this);
        pathFinder.SetNode2Pos(_mapCreator.Node2Pos);
        currTile = mapCreator.Tiles[_startX, _startY];
        endTile = mapCreator.Tiles[_endX, _endY];
        pathFinder.InitMap(_mapCreator.gridCol, _mapCreator.gridRow);
        //jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
        //jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);
        RefreshPath(_startX, _startY, _endX, _endY);
        
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
        if (dist.magnitude < 0.1f)
        {
            targetNode++;
            
            if (targetNode >= pathList.Count)
            {
                Debug.LogError("Complete");
                Lean.Pool.LeanPool.Despawn(gameObject);
                return;
            }
        }


        var target = pathList[targetNode];
        currTile = mapCreator.Tiles[target.x, target.y];
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
