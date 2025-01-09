// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-12 17:12:30
// # Recently: 2024-12-22 20:12:26
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JFramework
{
    internal sealed class InspectorItem : Inspector
    {
        private static InspectorItem instance;
        private static VisualElement inspector;

        private InspectorItem()
        {
            EditorManager.OnInitialized += OnInitialized;
            EditorManager.OnWindowMaximized += OnWindowMaximized;
            Selection.selectionChanged += SelectionChanged;
        }

        [InitializeOnLoadMethod]
        private static void Enable() => instance ??= new InspectorItem();

        protected override void InitInspector(EditorWindow window, VisualElement parent, VisualElement element)
        {
            if (element.parent[0].name == "JFramework Inspector")
            {
                element.parent.RemoveAt(0);
            }

            if (element.childCount != 0)
            {
                return;
            }

            inspector ??= InitInspector();
            element.parent.Insert(0, inspector);
        }

        private static VisualElement InitInspector()
        {
            var element = new VisualElement
            {
                name = "JFramework Inspector",
            };
            InitTitle(element);
            InitTools(element);
            InitSettings(element);
            InitPackages(element);
            return element;
        }

        private void OnInitialized()
        {
            if (Selection.activeObject == null)
            {
                SelectionChanged();
            }
        }

        private static void InitTitle(VisualElement element)
        {
            element.Add(new Label(element.name)
            {
                style =
                {
                    height = 21f,
                    fontSize = 14f,
                    borderBottomWidth = 1f,
                    borderBottomColor = Color.black * 0.6f,
                    backgroundColor = Color.gray * 0.6f,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            });
        }

        private static void InitSettings(VisualElement parent)
        {
            SetLabel(parent, "Settings");
            var element = SetElement(parent);
            SetButton(element, "File/Build Profiles", "Build Settings");
            SetButton(element, "Edit/Project Settings...", "Project Settings");
            SetButton(element, "Unity/Settings...", "Preferences");
        }

        private static void InitTools(VisualElement parent)
        {
            VisualElement element = null;
            foreach (var submenu in Unsupported.GetSubmenus("Tools"))
            {
                var index = submenu.LastIndexOf('/');
                if (submenu.Contains("JFramework"))
                {
                    if (element == null)
                    {
                        SetLabel(parent, "JFramework");
                        element = SetElement(parent);
                    }

                    SetButton(element, submenu, submenu.Substring(index + 1));
                }
            }
        }

        private static void InitPackages(VisualElement parent)
        {
            SetLabel(parent, "Packages");
            var element = SetElement(parent);
            SetButton(element, "Assets/Import Package/Custom Package...", "Import Custom Package");
            var status = false;
            var window = string.Empty;
            foreach (var submenu in Unsupported.GetSubmenus("Window"))
            {
                if (!status)
                {
                    if (submenu.ToUpper() != "WINDOW/PACKAGE MANAGER")
                    {
                        continue;
                    }

                    status = true;
                }

                var index = submenu.LastIndexOf('/');
                var itemText = submenu.Substring(0, index);
                var nameText = submenu.Substring(index + 1);
                index = itemText.LastIndexOf('/');

                if (index == -1)
                {
                    SetButton(element, submenu, nameText);
                    continue;
                }

                if (window != itemText)
                {
                    window = itemText;
                    SetLabel(parent, itemText.Substring(index + 1));
                    element = SetElement(parent);
                }

                SetButton(element, submenu, nameText);
            }
        }

        private static void SetLabel(VisualElement parent, string text)
        {
            parent.Add(new Label(text)
            {
                style =
                {
                    fontSize = 12f,
                    marginTop = 3f,
                    marginLeft = 3f,
                    marginRight = 3f,
                    paddingLeft = 5f
                }
            });
        }

        private static void SetButton(VisualElement parent, string submenu, string text)
        {
            parent.Add(new ToolbarButton(() => EditorApplication.ExecuteMenuItem(submenu))
            {
                text = text,
                style =
                {
                    height = 20,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    left = 0,
                    borderLeftWidth = 0,
                    borderRightWidth = 0
                }
            });
        }

        private static VisualElement SetElement(VisualElement parent)
        {
            var element = new VisualElement
            {
                style =
                {
                    borderTopWidth = 1f,
                    borderBottomWidth = 1f,
                    borderTopColor = Color.black * 0.6f,
                    borderBottomColor = Color.black * 0.6f,
                    marginTop = 2f
                }
            };
            parent.Add(element);
            return element;
        }
    }
}