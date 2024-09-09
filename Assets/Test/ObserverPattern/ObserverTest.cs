using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObserverTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Provider facebook = new Provider("Facebook");
        Provider youtube = new Provider("YouTube");

        Subscriber david = new Subscriber("David");
        Subscriber minju = new Subscriber("minju");
        Subscriber juha = new Subscriber("juha");
        david.SubScribe(facebook);
        david.SubScribe(youtube);
        david.UnSubScribe();
        minju.SubScribe(facebook);
        juha.SubScribe(youtube);

        facebook.Dispatch("news");
        youtube.Dispatch("big news");

    }

}


public class Provider : IObservable<EventA>
{ 
    public string Name { get; }

    public List<IObserver<EventA>> observerList = new List<IObserver<EventA>>();

    public Provider(string _name)
    {
        Name = _name;
    }

    public IDisposable Subscribe(IObserver<EventA> observer)
    {
        if(!observerList.Contains(observer))
            observerList.Add(observer);
        return new Unsubscribe(observerList, observer);
    }

    public void Dispatch(string _desc)
    {
        foreach (var item in observerList)
        {
            item.OnNext(new EventA(Name, _desc));
        }
    }
}

public class Unsubscribe : IDisposable
{

    private List<IObserver<EventA>> observerList;
    private IObserver<EventA> observer;

    public Unsubscribe(List<IObserver<EventA>> _observerList, IObserver<EventA> _observer) 
    {
        observerList = _observerList;
        observer = _observer;
    }
    public void Dispose()
    {
        if (observer != null)
        {
            observerList.Remove(observer);
        }
    }
}

public class Subscriber : IObserver<EventA>
{
    private IDisposable Unsubscribe;
    private string Name { get; set; }

    public Subscriber(string _name)
    {
        Name = _name;
    }
    public void SubScribe(IObservable<EventA> observable)
    {
        Unsubscribe = observable.Subscribe(this);
    }

    public void UnSubScribe()
    {
        Unsubscribe.Dispose();
    }
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(EventA value)
    {
        Debug.Log($"{Name} {value.Provider}  {value.Desc}");
    }
}

public class EventA
{
    public string Desc { get; }
    public string Provider { get; }
    public EventA(string _provider, string _desc)
    {
        Desc = _desc;
        Provider = _provider;
    }
}

