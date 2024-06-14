using System;
using UnityEditor;
using UnityEngine;

namespace EditorUI
{
    public class EditorWindowBase : EditorWindow
    {
        protected Action guiDraw = null;

        protected void OnGUI()
        {
            guiDraw?.Invoke();

            var evt = Event.current;
            switch (evt.type)
            {
                case EventType.KeyDown:
                    if (evt.keyCode == KeyCode.Escape)
                        Close();
                    break;
            }
        }
    }

    public class BackgroundColorScope : GUI.Scope
    {
        readonly Color color;



        public BackgroundColorScope(Color color)
        {
            this.color = GUI.backgroundColor;
            GUI.backgroundColor = color;
        }

        protected override void CloseScope() => GUI.backgroundColor = color;
    }
}