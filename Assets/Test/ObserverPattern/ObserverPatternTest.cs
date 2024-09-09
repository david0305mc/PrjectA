using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverPatternTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DoSomething();
    }

    public void DoSomething()
    {
        var fbObservable = new NotificationProvider("Facebook");
        var githubObservable = new NotificationProvider("GitHub");

        var observer = new NotificationSubscriber("Florin");
        observer.Subscribe(fbObservable);
        //observer.Unsubscribe();

        observer.Subscribe(githubObservable);
        //observer.Unsubscribe();

        var observer2 = new NotificationSubscriber("Piagio");
        observer2.Subscribe(fbObservable);

        fbObservable.EventNotification("Event notification 1 !");
        githubObservable.EventNotification("Event notification!");
    }
}

public class NotificationProvider : IObservable<SomeEvent>
{

    public string ProviderName { get; private set; }
    // Maintain a list of observers
    private List<IObserver<SomeEvent>> _observers;

    public NotificationProvider(string _providerName)
    {
        ProviderName = _providerName;
        _observers = new List<IObserver<SomeEvent>>();
    }

    // Define Unsubscriber class
    private class Unsubscriber : IDisposable
    {

        private List<IObserver<SomeEvent>> _observers;
        private IObserver<SomeEvent> _observer;

        public Unsubscriber(List<IObserver<SomeEvent>> observers,
                            IObserver<SomeEvent> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (!(_observer == null)) _observers.Remove(_observer);
        }
    }

    // Define Subscribe method
    public IDisposable Subscribe(IObserver<SomeEvent> observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    // Notify observers when event occurs
    public void EventNotification(string description)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(new SomeEvent(ProviderName, description,
                            DateTime.Now));
        }
    }
}
public class NotificationSubscriber : IObserver<SomeEvent>
{
    public string SubscriberName { get; private set; }
    private IDisposable _unsubscriber;

    public NotificationSubscriber(string _subscriberName)
    {
        SubscriberName = _subscriberName;
    }

    public virtual void Subscribe(IObservable<SomeEvent> provider)
    {
        // Subscribe to the Observable
        if (provider != null)
            _unsubscriber = provider.Subscribe(this);
    }

    public virtual void OnCompleted()
    {
        Debug.Log("Done");
    }

    public virtual void OnError(Exception e)
    {
        Debug.Log($"Error: {e.Message}");
    }

    public virtual void OnNext(SomeEvent ev)
    {
        Debug.Log($"Hey {SubscriberName} -> you received {ev.EventProviderName} {ev.Description} @ {ev.Date} ");
    }

    public virtual void Unsubscribe()
    {
        _unsubscriber.Dispose();
    }
}
public class SomeEvent
{
    public string EventProviderName { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }

    public SomeEvent(string _eventProviderName, string _description, DateTime _date)
    {
        EventProviderName = _eventProviderName;
        Description = _description;
        Date = _date;
    }
}