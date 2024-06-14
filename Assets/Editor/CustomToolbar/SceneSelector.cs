using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EditorUI
{
    public class SceneSelector : EditorWindowBase
    {
        const string RootPath = "Assets";
        const string BasePath = "01_Scenes";

        Dictionary<string, string> scenes = new Dictionary<string, string>();
        readonly string[] paths = { string.IsNullOrEmpty(BasePath) ? RootPath : $"{RootPath}/{BasePath}" };
        string lastPath = BasePath;
        Vector2 scrollPos = Vector2.zero;

        GUIStyle stylePathLable = null;
        GUIStyle stylePathText = null;
        GUIStyle styleResetButton = null;



        [MenuItem("Tools/Scene selector")]
        public static void OpenWindow()
        {
            GetWindow<SceneSelector>(nameof(SceneSelector));
        }

        void OnEnable()
        {
            stylePathLable = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset { top = 3 },
            };
            stylePathText = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset { top = 3, right = 5 },
                fixedHeight = 18,
            };
            styleResetButton = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset { left = 2, bottom = 1 },
                fontSize = 12,
                fixedWidth = 18,
                fixedHeight = 18,
            };

            LoadSceneList();
            guiDraw = GUIDraw;
        }



        void GUIDraw()
        {
            var current = Event.current;
            if (current.type == EventType.KeyDown)
            {
                switch (current.keyCode)
                {
                case KeyCode.Escape:
                    Close();
                    return;
                }
            }

            EditorGUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Path", stylePathLable, GUILayout.MaxWidth(30));
                    var oldPath = lastPath;
                    lastPath = EditorGUILayout.TextField(oldPath, stylePathText, GUILayout.MinWidth(50));
                    if (string.Compare(oldPath, lastPath) != 0)
                    {
                        paths[0] = string.IsNullOrEmpty(lastPath) ? RootPath : $"{RootPath}/{lastPath}";
                        LoadSceneList();
                    }

                    if (GUILayout.Button("C", styleResetButton))
                    {
                        GUIUtility.keyboardControl = 0;
                        lastPath = BasePath;
                        paths[0] = string.IsNullOrEmpty(BasePath) ? RootPath : $"{RootPath}/{lastPath}";
                        LoadSceneList();
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, new GUILayoutOption[0]);
                {
                    if (0 < scenes.Count)
                    {
                        var iter = scenes.GetEnumerator();
                        while (iter.MoveNext())
                        {
                            var curScene = EditorSceneManager.GetActiveScene();

                            if (string.Compare($"{curScene.name}.unity", iter.Current.Key) == 0)
                            {
                                using (new BackgroundColorScope(Color.cyan))
                                    GUILayout.Button(iter.Current.Key, GUILayout.Height(50));
                            }
                            else
                            {
                                if (GUILayout.Button(iter.Current.Key, GUILayout.Height(50)))
                                    LoadScenes(iter.Current.Value);
                            }
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        void LoadSceneList()
        {
            scenes.Clear();
            var fileGUIDs = AssetDatabase.FindAssets("t:Scene", paths);
            for (int index = 0; index < fileGUIDs.Length; index++)
            {
                var path = AssetDatabase.GUIDToAssetPath(fileGUIDs[index]);
                var arrPath = path.Split('/');

                scenes[arrPath[arrPath.Length - 1]] = path;
            }
        }

        void LoadScenes(string path)
        {
            bool sceneSaveCheckBool = true;
            if (EditorSceneManager.GetActiveScene().isDirty)
                sceneSaveCheckBool = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            if (sceneSaveCheckBool)
            {
                EditorSceneManager.OpenScene(path);
                Close();
            }
        }
    }
}