using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MessagePipe;
using Cysharp.Threading.Tasks;

public class UIMain : MonoBehaviour
{
    [SerializeField] private Button levelUpBtn;
    [SerializeField] private Button buffUpBtn;
    [SerializeField] private Button acqireBoldBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button rightBtn;

    [SerializeField] private Text levelText;
    [SerializeField] private Text buffText;


    private void Start()
    {
        levelUpBtn.onClick.AddListener(() =>
        {
            UserDataStore.Instance.AccountData.Level.Value++;
        });

        buffUpBtn.onClick.AddListener(() =>
        {
            UserDataStore.Instance.AccountData.Buff.Value++;
        });
        acqireBoldBtn.onClick.AddListener(() => {
            UserDataStore.Instance.AccountData.Gold.Value += 1000;
        });
        restartBtn.onClick.AddListener(() => {
            UserDataStore.Instance.AccountData.DisposalbleClear();
            SceneManager.LoadSceneAsync(SceneName.Splash);
        });
        rightBtn.onClick.AddListener(() => {
            OnClickBtnShop().Forget();
        });
        UserDataStore.Instance.AccountData.Level.SubscribeToText(levelText, item => $"Level {item}").AddTo(this);
        UserDataStore.Instance.AccountData.Buff.SubscribeToText(buffText, item => $"Buff {item}").AddTo(this);
    }
    
    public async UniTaskVoid OnClickBtnShop()
    {
        //await DataManager.Instance.LoadDataAsync();
        //foreach (var item in DataManager.Instance.Sheet1Dic)
        //{
        //    Debug.Log($"item {item.Value.index}");
        //    Debug.Log($"item {item.Value.test}");
        //}
        //return;
        
        var popup = PopupManager.Instance.ShowSystemTwoBtnPopup("test", "test2", () =>
        {
            Debug.Log("Test25");
        });
        //PopupManager.Instance.Show<CommonPopup>(()=> {
        //    Debug.Log("Hide Popup Test");
        //});
    }
}
