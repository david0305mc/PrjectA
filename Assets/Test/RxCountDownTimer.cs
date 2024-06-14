using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RxCountDownTimer : MonoBehaviour
{
    public IObservable<int> CountDownObservable => _countDownObservable.AsObservable();
    private IConnectableObservable<int> _countDownObservable;
    //public IObservable<int> CountDownObservable;

    void Start()
    {
        _countDownObservable = CreateCountDownObservable(60).Publish();
        _countDownObservable.Connect();
        //CountDownObservable = CreateCountDownObservable(60);
    }

    private IObservable<int> CreateCountDownObservable(int countTime)
    {
        return Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            .Select(t => (int)(countTime - t))
            .TakeWhile(t => t > 0);
    }
}
