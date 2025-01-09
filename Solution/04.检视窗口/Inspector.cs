// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-08 00:12:53
// # Recently: 2024-12-22 20:12:29
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JFramework.Editor
{
    internal abstract class Inspector
    {
        protected void SelectionChanged()
        {
            foreach (var obj in Resources.FindObjectsOfTypeAll(Reflection.inspectorType))
            {
                if (obj is EditorWindow window)
                {
                    InitWindow(window);
                }
            }
        }

        private void InitWindow(EditorWindow window)
        {
            var parent = FindElement(window.rootVisualElement, "unity-inspector-main-container");
            var element = FindElement(window.rootVisualElement, "unity-inspector-editors-list");
            if (parent != null && parent.childCount >= 2)
            {
                InitInspector(window, parent, element);
            }
        }

        private static VisualElement FindElement(VisualElement parent, string className)
        {
            for (var i = 0; i < parent.childCount; i++)
            {
                var element = parent[i];
                if (element.ClassListContains(className))
                {
                    return element;
                }

                element = FindElement(element, className);
                if (element != null)
                {
                    return element;
                }
            }

            return null;
        }

        protected void OnWindowFocused(EditorWindow window)
        {
            if (window != null && window.GetType() == Reflection.inspectorType)
            {
                InitWindow(window);
            }
        }

        protected void OnWindowMaximized(EditorWindow window)
        {
            if (window != null && window.GetType() == Reflection.inspectorType)
            {
                InitWindow(window);
            }
        }

        protected abstract void InitInspector(EditorWindow window, VisualElement parent, VisualElement element);
    }
}