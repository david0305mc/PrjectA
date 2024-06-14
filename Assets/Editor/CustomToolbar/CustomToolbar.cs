using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;


namespace EditorUI
{
    [InitializeOnLoad]
    public static class CustomToolbar
    {
        const BindingFlags ReadFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private static readonly Type containterType = typeof(IMGUIContainer);
        private static readonly Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static readonly Type guiViewType = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");
#if UNITY_2020_1_OR_NEWER
        private static readonly Type backendType = typeof(Editor).Assembly.GetType("UnityEditor.IWindowBackend");

        private static readonly PropertyInfo guiBackend = guiViewType.GetProperty("windowBackend", ReadFlags);
        private static readonly PropertyInfo visualTree = backendType.GetProperty("visualTree", ReadFlags);
#else
        private static readonly PropertyInfo visualTree = guiViewType.GetProperty("visualTree", ReadFlags);
#endif
        private static readonly FieldInfo onGuiHandler = containterType.GetField("m_OnGUIHandler", ReadFlags);
        private static UnityEngine.Object toolbar = null;

        static float fromToolsOffset_Left = 400.0f;
        static float fromStripOffset_Left = 65.0f;
        static float fromToolsOffset_Right = 370.0f;
        static float fromStripOffset_Right = 50.0f;



        static CustomToolbar()
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        static void OnUpdate()
        {
            if (toolbar == null)
            {
                var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);
                if (toolbars == null || toolbars.Length == 0)
                    return;

                toolbar = toolbars[0];
            }

#if UNITY_2020_1_OR_NEWER
            var backend = guiBackend.GetValue(toolbar);
            var elements = visualTree.GetValue(backend, null) as VisualElement;
#else
            var elements = visualTree.GetValue(toolbar, null) as VisualElement;
#endif

#if UNITY_2019_1_OR_NEWER
            var container = elements[0];
#else
            var container = elements[0] as IMGUIContainer;
#endif

            var handler = onGuiHandler.GetValue(container) as Action;
            handler -= OnGUI;
            handler += OnGUI;
            onGuiHandler.SetValue(container, handler);
            EditorApplication.update -= OnUpdate;
        }

        static void OnGUI()
        {
            var screenWidth = EditorGUIUtility.currentViewWidth;
            LeftSide(screenWidth);
            RightSide(screenWidth);
        }

        static void LeftSide(float screenWidth)
        {
            var rect = new Rect(0, 0, screenWidth, Style.rowHeight);
            rect.xMin = fromToolsOffset_Left;
            rect.xMax = screenWidth * 0.5f - fromStripOffset_Left;
            rect.xMin += Style.spacing;
            rect.xMax -= Style.spacing;
            rect.yMin += Style.topPadding;
            rect.yMax -= Style.botPadding;

            if (0 < rect.width)
            {
                GUILayout.BeginArea(rect);
                GUILayout.BeginHorizontal();
                {
                    bool hasSymbol = GameUtil.IsSimbol("TEST_DEF", EditorUserBuildSettings.selectedBuildTargetGroup);
                    GUIStyle style = new GUIStyle()
                    {
                        normal = new GUIStyleState
                        {
                            background = hasSymbol ? Texture2D.whiteTexture : Texture2D.grayTexture
                        }
                    };
                    if (GUILayout.Button("TEST_DEF", style))
                    {
                        if (hasSymbol)
                        {
                            GameUtil.RemoveSimbol("TEST_DEF", EditorUserBuildSettings.selectedBuildTargetGroup);
                        }
                        else
                        {
                            GameUtil.AddSimbol("TEST_DEF", EditorUserBuildSettings.selectedBuildTargetGroup);
                        }

                    }

                    GUILayout.Label("2D gizmo"); Physics2D.alwaysShowColliders = EditorGUILayout.Toggle(Physics2D.alwaysShowColliders, GUILayout.MaxWidth(15));
                    GUILayout.Label("Time scale"); Time.timeScale = EditorGUILayout.Slider(Time.timeScale, 0, 100);
                }
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }

        static void RightSide(float screenWidth)
        {
            var rect = new Rect(0, 0, screenWidth, Style.rowHeight);
            rect.xMin = screenWidth * 0.5f + fromStripOffset_Right;
            rect.xMax = screenWidth - fromToolsOffset_Right;
            rect.xMin += Style.spacing;
            rect.xMax -= Style.spacing;
            rect.yMin += Style.topPadding;
            rect.yMax -= Style.botPadding;

            if (0 < rect.width)
            {
                GUILayout.BeginArea(rect);
                GUILayout.BeginHorizontal();
                {
                    //GUILayout.Label(EditorGUIUtility.fieldWidth.ToString());
                    //GUILayout.Label(EditorGUIUtility.labelWidth.ToString());
                    //GUILayout.Label(EditorGUIUtility.currentViewWidth.ToString());

                    if (GUILayout.Button("M", GUILayout.Width(25f))) 
                        LoadScene("Assets/01_Scenes/0_Splash.unity");
                    if (GUILayout.Button("S", GUILayout.Width(25f))) 
                        SceneSelector.OpenWindow();
                    //if (GUILayout.Button("Symbols")) SymbolSelector.OpenWindow();
                    //if (GUILayout.Button("Settings")) SettingsService.OpenProjectSettings("Project");
                    //if (GUILayout.Button("TestDefine"))
                    //{
                    //    if (GameClient_Utill.IsSimbol("TESTDEFINE", EditorUserBuildSettings.selectedBuildTargetGroup))
                    //    {
                    //        GameClient_Utill.RemoveSimbol("TESTDEFINE", EditorUserBuildSettings.selectedBuildTargetGroup);
                    //    }
                    //    else
                    //    {
                    //        GameClient_Utill.AddSimbol("TESTDEFINE", EditorUserBuildSettings.selectedBuildTargetGroup);
                    //    }

                    //}
                }
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }

        static void LoadScene(string path)
        {
            bool sceneSaveCheckBool = true;
            if (EditorSceneManager.GetActiveScene().isDirty)
                sceneSaveCheckBool = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            if (sceneSaveCheckBool)
            {
                EditorSceneManager.OpenScene(path);
            }
        }

        static class Style
        {
            internal static readonly float rowHeight = 30.0f;
            internal static readonly float spacing = 15.0f;
            internal static readonly float topPadding = 5.0f;
            internal static readonly float botPadding = 3.0f;
        }
    }
}