using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArrangeChildren : MonoBehaviour
{
    public float spacingX = 1.0f; // x축 간격
    public float spacingY = 1.0f; // y축 간격
    public int objectsPerRow = 5; // 한 줄에 배치할 오브젝트 개수

#if UNITY_EDITOR
    [CustomEditor(typeof(ArrangeChildren))]
    public class ArrangeChildrenEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ArrangeChildren arrangeScript = (ArrangeChildren)target;
            if (GUILayout.Button("Arrange Children"))
            {
                arrangeScript.Arrange();
            }
        }
    }
#endif

    void Arrange()
    {
        int childCount = transform.childCount;
        float xOffset = 0.0f;
        float yOffset = 0.0f;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 newPos = new Vector3(xOffset, yOffset, 0); // x, y 축으로 배치

            child.localPosition = newPos;
            xOffset += spacingX;

            // 다음 줄로 넘어갈 때 xOffset을 초기화하고 yOffset을 증가시킵니다.
            if ((i + 1) % objectsPerRow == 0)
            {
                xOffset = 0;
                yOffset -= spacingY;
            }
        }
    }
}
