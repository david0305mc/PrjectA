using FT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTestMoveObj : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private int speed;
    private AbsPathFinder jpsPathFinder;
    List<PathNode> pathList;
    private int targetNode;

    public void InitData(MapCreator _mapCreator, int _startX, int _startY, int _endX, int _endY)
    {
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
        transform.position = _mapCreator.Node2Pos(_startX, _startY);
    }

    private void FixedUpdate()
    {
        if (pathList.Count == 0)
            return;

        if (targetNode >= pathList.Count)
        {
            return;
        }

        var dist = pathList[targetNode].location - rigidBody.position;
        if (dist.magnitude < 0.1f)
        {
            targetNode++;
            if (targetNode >= pathList.Count)
            {
                Debug.LogError("Complete");
            }
            return;
        }

        var movePos = rigidBody.position + (dist.normalized * speed * Time.fixedDeltaTime);
        rigidBody.MovePosition(movePos);
        //transform.position -= new Vector3(0, Time.fixedDeltaTime, 0);
    }
}
