using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalizationManager))]
public class LocalizationManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LocalizationManager _target = (LocalizationManager)target;
        if (GUILayout.Button("Get Localize Text"))
        {
            _target.GetLocalizationText();
            EditorUtility.SetDirty(_target);
        }
    }

}
