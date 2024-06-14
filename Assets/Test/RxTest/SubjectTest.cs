using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SubjectTest : MonoBehaviour
{
    Subject<int> countTst = new Subject<int>();
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        countTst.Subscribe(count => {
            Debug.Log($"count {count}");
        });
    }

    public void OnClickBtnAdd()
    {
        countTst.OnNext(count++);
    }
    public void OnClickBtnNormal()
    {
        countTst.OnNext(count);
    }
}
