using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RxTestManager : MonoBehaviour
{
    private Dictionary<int, ReactiveProperty<CollectionData>> CollectionDic;
    [SerializeField] private Text text;
    //[SerializeField] private RxCountDownTimer rxCountDownTimer;

    public IObservable<int> GameStartCountDownObservable { get; private set; }
    public IObservable<int> BattleCountDownObservable { get; private set; }

    private IObservable<int> CreateCountDownObservable(int countTime) =>
     Observable
         .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
         .Select(x => (int)(countTime - x))
         .TakeWhile(x => x > 0);

    private void Start()
    {
        TestReactiveDictionaryInit();   
    }

    private void TestReactiveDictionaryInit()
    {
        CollectionDic = new Dictionary<int, ReactiveProperty<CollectionData>>();

        int collection_idx = 1;
        int collection_type = 2;
        ReactiveProperty<CollectionData> collectionData;
        if (!CollectionDic.TryGetValue(collection_idx, out collectionData))
        {
            collectionData = new ReactiveProperty<CollectionData>(new CollectionData());
            collectionData.Value.Init(collection_idx);
            CollectionDic[collection_idx] = collectionData;
        }

        collectionData.Value.collection_idx = collection_idx;
        collectionData.Value.collection_type = collection_type;

        CollectionValueData collectionValue = new CollectionValueData();
        collectionValue.idx = 11;
        collectionValue.cnt.Value = 1;
        collectionData.Value.collection_value.Add(collectionValue);
        collectionData.Value.collection_value[0].cnt.Subscribe((val) =>
        {
            Debug.Log($"val {val}");
        });

        collectionData.Subscribe((value)=>{
            Debug.Log("Subscribe " + value);
        });
        collectionData.Value.collection_value[0].cnt.Value = 4;
        collectionData.Value.collection_value[0].cnt.Value = 5;
    }

    public void TestReactiveDictionaryFunc()
    {
        
    }

    private void TestReactiveCollection()
    {
        ReactiveCollection<string> names = new ReactiveCollection<string> { "sdafsdaf", "sdagsdgdfgd" };

        names.ObserveAdd()
            .Subscribe(name => Debug.Log("Add: " + name))
            .AddTo(this);

        names.ObserveRemove()
            .Subscribe(name => Debug.Log("Remove: " + name))
            .AddTo(this);

        names.ObserveCountChanged()
            .Subscribe(count => Debug.Log("Count: " + count))
            .AddTo(this);

        names.Add("zsdafsdfsa");       //Add: メタルスライム      Count: 4

        names.Remove("sdagsdgdfgd");          //Remove: スライム         Count: 3
    }

    private void TestReactiveDictoinary()
    {
        ReactiveDictionary<string, string> monsters = new ReactiveDictionary<string, string>
        {
            {"1", "a"},
            {"2", "b"},
            {"3", "c"}
        };

        //monsters.ObserveAdd()
        //    .Subscribe(x => Debug.Log("Add: " + x.Value))
        //    .AddTo(this);

        //monsters.ObserveRemove()
        //    .Subscribe(x => Debug.Log("Remove: " + x.Value))
        //    .AddTo(this);            
        //monsters.ObserveCountChanged()
        //    .Subscribe(count => Debug.Log("Count: " + count))
        //    .AddTo(this);

        monsters.ObserveEveryValueChanged(a => a.Count).Subscribe(b =>
        {
            Debug.Log($"ObserveEveryValueChanged{b}");
        }).AddTo(this);
        

        monsters["1"] = "d";
        monsters.Add("4", "e");       
        monsters["1"] = "f";
        monsters.Remove("1");         
    }

    private void TestTimer()
    {

        var startConnectableObservable = CreateCountDownObservable(3).Publish();
        GameStartCountDownObservable = startConnectableObservable;

        var battleConnectableObservable = CreateCountDownObservable(5).Publish();
        BattleCountDownObservable = battleConnectableObservable;

        GameStartCountDownObservable.Subscribe(_ => {

        }, () => {
            Debug.Log("Battle Start");
            battleConnectableObservable.Connect();
        });

        BattleCountDownObservable.Concat(CreateCountDownObservable(5)).Subscribe(_ =>
        {
        }, () =>
        {
            Debug.Log("Battle End");
        }).AddTo(gameObject);
        startConnectableObservable.Connect();
    }

    //private void Start()
    //{
    //    StartCoroutine(StartTimer());
    //}

    //private IEnumerator StartTimer()
    //{
    //    yield return new WaitForSeconds(3);
    //    rxCountDownTimer.CountDownObservable.Subscribe(time =>
    //    {
    //        text.text = $"Timeleft {time}";
    //    }, () =>
    //    {
    //        text.text = string.Empty;
    //    }).AddTo(gameObject);
    //    rxCountDownTimer.CountDownObservable.First(timer => timer <= 57).Subscribe(_ => text.color = Color.red);
    //}

    //private void Start()
    //{
    //    var timer = Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1)).Select(t => 60 - t).TakeWhile(x => x > 0);
    //    timer.Subscribe(x => text.text = $"time left {x}");
    //    timer.First(x => x <= 55).Subscribe(_ => text.color = Color.red);
    //}
}

public class CollectionData
{
    // new
    public int collection_idx;
    public int collection_type;
    public List<CollectionValueData> collection_value = new List<CollectionValueData>();
    public int receipt_check;
    public int id;
    public void Init(int id)
    {
        this.id = id;
    }
}

public class CollectionValueData
{
    public int typ;
    public int idx;
    public ReactiveProperty<int> cnt = new ReactiveProperty<int>(0);


}

