using FT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MapTestMoveObj : Boids2D
{
    private AbsPathFinder pathFinder;
    List<PathNode> pathList;
    private int targetNodeIndex;
    private int currNodeIndex;
    private MapCreator mapCreator;
    TEST.MapTestObj startTile;
    TEST.MapTestObj endTile;

    private void Awake()
    {
        MessageDispather.Receive<int>(EMessage.UpdateTile).Subscribe(_ =>
        {
            //RefreshPath();

            //var currNode = pathList[targetNode];
            //var endNode = pathList[pathList.Count - 1];
            Debug.Log($"MessageDispather.Receive {currNodeIndex}");
            RefreshPath(pathList[currNodeIndex].x, pathList[currNodeIndex].y, endTile.X, endTile.Y);

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
                        startTile.currNodeMark.SetActive(false);
                    }
                    else
                    {
                        mapCreator.Tiles[pathList[currNodeIndex].x, pathList[currNodeIndex].y].currNodeMark.SetActive(false);
                    }
                    currNodeIndex = targetNodeIndex;
                    mapCreator.Tiles[pathList[currNodeIndex].x, pathList[currNodeIndex].y].currNodeMark.SetActive(true);
                }
            }
        }
    }

    public void InitData(MapCreator _mapCreator, int _startX, int _startY, int _endX, int _endY)
    {
        mapCreator = _mapCreator;
        pathFinder = new AStarPathFinder(this);
        pathFinder.SetNode2Pos(_mapCreator.Node2Pos);
        currNodeIndex = -1;
        
        startTile = mapCreator.Tiles[_startX, _startY];
        startTile.currNodeMark.SetActive(true);
        endTile = mapCreator.Tiles[_endX, _endY];
        pathFinder.InitMap(_mapCreator.gridCol, _mapCreator.gridRow);
        //jpsPathFinder.recorder.SetDisplayAction(DisplayRecord);
        //jpsPathFinder.recorder.SetOnPlayEndAction(OnPlayEnd);
        RefreshPath(_startX, _startY, _endX, _endY);
        
        transform.position = (Vector2)mapCreator.Node2Pos(_startX, _startY) + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
    }


    private void DrawPathLine()
    {
        for (int i = 0; i < pathList.Count - 1; i++)
        {
            Debug.DrawLine(pathList[i].location, pathList[i + 1].location, Color.red);
        }
        
    }

    private void FixedUpdate()
    {
        if (pathList.Count == 0 || targetNodeIndex >= pathList.Count)
            return;

        if (targetNodeIndex >= pathList.Count)
        {
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
                    startTile.currNodeMark.SetActive(false);
                }
                else
                {
                    mapCreator.Tiles[pathList[currNodeIndex].x, pathList[currNodeIndex].y].currNodeMark.SetActive(false);
                }
                
                currNodeIndex = targetNodeIndex;
                mapCreator.Tiles[pathList[currNodeIndex].x, pathList[currNodeIndex].y].currNodeMark.SetActive(true);
            }
        }

        var targetNode = pathList[targetNodeIndex];
        Vector2 newPos;
        if (isBoidsAlgorithm)
        {
            var velocity = CalculateBoidsAlgorithm((Vector2)targetNode.location);
            newPos = Vector2.MoveTowards(_rigidbody2D.position, _rigidbody2D.position + velocity, _forwardSpeed * Time.fixedDeltaTime);
        }
        else
        {
            newPos = Vector2.MoveTowards(_rigidbody2D.position, (Vector2)targetNode.location, _forwardSpeed * Time.fixedDeltaTime);
        }
        _rigidbody2D.MovePosition(newPos);
        //var movePos = rigidBody.position + (dist.normalized * speed * Time.fixedDeltaTime);

        //transform.position -= new Vector3(0, Time.fixedDeltaTime, 0);
    }
}
