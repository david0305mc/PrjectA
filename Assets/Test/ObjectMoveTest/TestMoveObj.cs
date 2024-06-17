using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FT;

public class TestMoveObj : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private int speed;
    List<PathNode> pathList;
    private int targetNode;
    
    public void InitData(List<PathNode> _pathNodeList)
    {
        pathList = _pathNodeList;
        targetNode = 0;
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
        if (dist.magnitude < 0.3f)
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
