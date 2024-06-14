using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TableEditor : EditorWindow
{
    [MenuItem("Tool/Design/Table Window")]
    public static void OpenWindow()
    {
        TableEditor window = EditorWindow.GetWindow<TableEditor>();
        window.Show();
    }
}
