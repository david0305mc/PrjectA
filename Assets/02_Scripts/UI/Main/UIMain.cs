using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

public class UIMain : SingletonMono<UIMain>
{
    public enum BottomTab
    {
        Shop,
        Arrangement,
        Worldmap,
        Pvp,
        Event,
    }

    [SerializeField] private UIDamageText uiDamageTextPref;

    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private UIMainBottomTabGroup tabGruop;
    [SerializeField] private UIPanelUnitSelect unitSelectPanel;
    [SerializeField] private UIPanelStageInfo stageInfoPanel;

    public BattleUI battleUI;
    public SS.WorldUI worldUI;
    [SerializeField] private Button testAddGoldBtn;
    [SerializeField] private Button testLevelUpBtn;

    protected override void OnSingletonAwake()
    {
        base.OnSingletonAwake();
        battleUI.gameObject.SetActive(false);

        testAddGoldBtn.onClick.AddListener(() =>
        {
            SS.GameManager.Instance.TestAddGold(100);
        });
        testLevelUpBtn.onClick.AddListener(() =>
        {
            SS.GameManager.Instance.TestLevelUp();
        });
    }

    public void InitUI()
    { 
        
    }

    public void SetUIWorldPosToCameraPos(RectTransform ui, Vector2 _worldPos)
    {
        RectTransform canvasRect = worldCanvas.GetComponent<RectTransform>();
        // convert screen coords
        Vector2 adjustedPosition = worldCanvas.worldCamera.WorldToScreenPoint(_worldPos);
        adjustedPosition.x *= canvasRect.rect.width / (float)worldCanvas.worldCamera.pixelWidth;
        adjustedPosition.y *= canvasRect.rect.height / (float)worldCanvas.worldCamera.pixelHeight;
        ui.anchoredPosition = adjustedPosition - canvasRect.sizeDelta / 2f;
    }

    public void ShowDamageText(Vector2 _pos, int _damage)
    {
        UIDamageText damageText = Lean.Pool.LeanPool.Spawn(uiDamageTextPref, _pos, Quaternion.identity, worldCanvas.transform);
        damageText.SetData(_damage);
    }

    private void OnDrawGizmos()
    {
        if (Application.IsPlaying(this))
        {
            Gizmos.color = new Color(1.0f, 0, 0, 1f);
            Gizmos.matrix = transform.worldToLocalMatrix;
            
            var boundaries = MCameraManager.Instance.GetBoundary();
            Gizmos.DrawLine(boundaries.Item1, boundaries.Item2);
            Gizmos.DrawLine(boundaries.Item2, boundaries.Item3);
            Gizmos.DrawLine(boundaries.Item3, boundaries.Item4);
            Gizmos.DrawLine(boundaries.Item4, boundaries.Item1);
        }
    }

    public void Broadcast(string funcName)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
#endif
        {
            gameObject.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void InitTabGroup()
    {
        tabGruop.InitTabGroup((int)BottomTab.Worldmap, (_index) =>
        {
            OnBottomTabClicked(_index);
        });
    }
    private void OnBottomTabClicked(int _index)
    {
        switch ((BottomTab)_index)
        {
            case BottomTab.Worldmap:
                {
                }
                break;

            case BottomTab.Arrangement:
                {
                    ShowArrangementUI();
                }
                break;
            case BottomTab.Event:
                {
                }
                break;
            case BottomTab.Pvp:
                {
                }
                break;
            case BottomTab.Shop:
                {
                }
                break;
        }
    }
    public void SelectTab(BottomTab _tab)
    {
        tabGruop.SelectTab((int)_tab);
    }

    public void ShowUnitInfo(int _uid)
    {
        unitSelectPanel.ShowUnitInfoPopup(_uid);
    }

    public void ShowArrangementUI()
    {
        unitSelectPanel.SetActive(true);
        unitSelectPanel.InitUI();
    }

    public void HideArrangementUI()
    {
        unitSelectPanel.SetActive(false);
    }
}
