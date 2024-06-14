using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] private GameObject rxObject;
    private RxTest rxTest;
    private int Count;

    private List<RxC> rxList = new List<RxC>();
    //IEnumerator CreateRxCoroutine(int num)
    //{   
    //    var rx = GameObject.Instantiate(rxObject).GetComponent<RxTest>();
    //    rx.index = num;
    //    int count = 0;
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1f);
    //        rx.intTest.Value = count++;
    //        //rxTest.intTest.OnNext(num++);
    //    }
    //}
    IEnumerator CreateRxCoroutine(int num)
    {
        RxC rx = new RxC();
        rx.index = num;
        rxList.Add(rx);
        int count = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            rx.intTest.Value = count++;
            //rxTest.intTest.OnNext(num++);
        }
    }

    public void CreateRx()
    {
        StartCoroutine(CreateRxCoroutine(Count++));
    }

    public void RemoveRxc()
    {
        rxList.RemoveAt(0);
    }
    //[Button]
    //public void Destroy()
    //{
    //    Destroy(rxTest.gameObject);
    //}
    //[Button]
    //public void Dispose()
    //{
    //    rxTest.intTest.Dispose();
    //}
    //[Button]
    //public void DisposablesClear()
    //{
    //    rxTest.disposables.Clear();
    //}
}
