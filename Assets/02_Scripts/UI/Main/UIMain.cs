using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

public class UIMain : SingletonMono<UIMain>
{
    [SerializeField] private UIDamageText uiDamageTextPref;

    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Button startSpaceAreaBtn;
    
    [SerializeField] private GameObject ingameUI;
    [SerializeField] private GameObject worldUI;
    [SerializeField] private Button testSpawnBtn;

    private void Awake()
    {
        ingameUI.SetActive(false);
        startSpaceAreaBtn.onClick.AddListener(() =>
        {
            SS.GameManager.Instance.StartSpaceSurvival("SurvivalMap/GridMap01.prefab");
            ingameUI.SetActive(true);
        });

        testSpawnBtn.onClick.AddListener(() =>
        {
            SS.GameManager.Instance.AddEnemyObj();
        });
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
}
