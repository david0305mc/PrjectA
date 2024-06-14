using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RxTest : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
    //    Subject<int> intTest = new Subject<int>();
    //    intTest.Subscribe((value) => {
    //        Debug.Log(value);
    //    });
    //    intTest.OnNext(1);
    //    intTest.Dispose();
    //    intTest.OnNext(3);
    //    intTest.OnNext(4);
    //}
    public int index;
    public CompositeDisposable disposables = new CompositeDisposable();
    public ReactiveProperty<int> intTest = new ReactiveProperty<int>();
    //public Subject<int> intTest = new Subject<int>();

    void Start()
    {
        intTest.Subscribe((value) =>
        {
            Debug.Log($"{index} - {value}" );
        }).AddTo(this);
    }

}

public class RxC
{
    public int index;
    public ReactiveProperty<int> intTest = new ReactiveProperty<int>();
    public RxC()
    {
        intTest.Subscribe((value) =>
        {
            Debug.Log($"{index} - {value}");
        });
    }
}
