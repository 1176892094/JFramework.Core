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

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Event = UnityEngine.Event;

namespace JFramework
{
    internal sealed partial class Hierarchy
    {
        private static Hierarchy instance;
        private static EditorWindow window;
        private static readonly GUIContent content = new GUIContent();
        private static readonly HashSet<int> windows = new HashSet<int>();

        private Hierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
        }

        [InitializeOnLoadMethod]
        private static void Enable() => instance ??= new Hierarchy();

        private static void OnGUI(int id, Rect rect)
        {
            var cursor = Event.current.mousePosition;
            var target = (GameObject)EditorUtility.InstanceIDToObject(id);
            var status = cursor.x >= 0 && cursor.x <= rect.xMax + 16 && cursor.y >= rect.y && cursor.y < rect.yMax;

            switch (Event.current.type)
            {
                case EventType.Layout:
                    window = Reflection.GetHierarchy();
                    if (window != null)
                    {
                        var windowId = window.GetInstanceID();
                        if (!windows.Contains(windowId))
                        {
                            InitWindow(window, windowId);
                        }
                    }

                    break;
                case EventType.Repaint:
                    DrawTree(rect, target);
                    DrawIcon(rect, target);
                    break;
                case EventType.MouseDown:
                    DrawIcon(rect, target);
                    break;
            }

            if (status && target != null)
            {
                status = target.activeSelf;
                target.SetActive(EditorGUI.Toggle(new Rect(32f, rect.y, 16f, rect.height), target.activeSelf));
                if (status != target.activeSelf)
                {
                    EditorUtility.SetDirty(target);
                }
            }
        }

        private static void InitWindow(EditorWindow window, int windowId)
        {
            var element = window.rootVisualElement.parent.Query<IMGUIContainer>().First();
            element.onGUIHandler = HideHierarchyIcon + element.onGUIHandler;
            Reflection.HideIcon(window);
            windows.Add(windowId);

            void HideHierarchyIcon()
            {
                if (Event.current.type == EventType.Layout)
                {
                    Reflection.HideIcon(window);
                }
            }
        }
    }

    internal partial class Hierarchy
    {
        private static void DrawTree(Rect rect, GameObject target)
        {
            var itemRect = new Rect(rect.x, rect.y + (16 - rect.height) / 2, 16, 16);
            GUI.DrawTexture(itemRect, DrawIcon(target), ScaleMode.ScaleToFit);
            if (target == null) return;
            var x = Mathf.Max(28f, rect.x - 128f - 16f);
            var width = Mathf.Min(128f, rect.x - 28f - 16f);
            var treeRect = new Rect(x, rect.y, width, rect.height);
            var texCoords = new Rect(1 - width / 128f, 0, width / 128f, 1);
            GUI.DrawTextureWithTexCoords(treeRect, ProjectIcon.GetIcon(Tree.Normal), texCoords);
            var transform = target.transform;
            if (transform.childCount == 0)
            {
                var parent = transform.parent;
                var child = parent != null ? parent.childCount : target.scene.rootCount;
                var index = transform.GetSiblingIndex();
                var itemTree = Tree.Height;
                if (index != 0)
                {
                    itemTree = index == child - 1 ? Tree.Bottom : Tree.Middle;
                }

                treeRect.width = 16;
                treeRect.x = rect.x - 16;
                GUI.DrawTexture(treeRect, ProjectIcon.GetIcon(itemTree));
            }

            if (Mathf.FloorToInt((rect.y - 4) / 16 % 2) != 0)
            {
                itemRect = new Rect(32f, rect.y, rect.width + rect.x - 16, rect.height);
                EditorGUI.DrawRect(itemRect, Color.black * 0.05f);
            }

            rect.width += 16;
            rect.height = 1;
            rect.xMin = 32;
            rect.y += 15.5f;
            EditorGUI.DrawRect(rect, Color.black * 0.2f);
        }

        private static Texture DrawIcon(GameObject target)
        {
            if (target == null)
            {
                return Reflection.unityIcon.image;
            }

            Texture icon = AssetPreview.GetMiniThumbnail(target);

            if (icon.name is "d_Prefab Icon" or "Prefab Icon")
            {
                if (PrefabUtility.IsAnyPrefabInstanceRoot(target))
                {
                    return Reflection.prefabIcon.image;
                }
            }

            if (icon.name is "cs Script Icon" or "d_cs Script Icon")
            {
                return Reflection.targetIcon.image;
            }

            var components = target.GetComponents<Component>();
            Component component;
            if (components.Length > 1)
            {
                component = components[1];
                if (components.Length > 2 && component is CanvasRenderer)
                {
                    component = components[2];
                    if (components.Length > 3 && component is ICanvasRaycastFilter)
                    {
                        var image = components[3];
                        icon = AssetPreview.GetMiniThumbnail(image);
                        if (icon != null)
                        {
                            component = image;
                        }
                    }
                }
            }
            else
            {
                component = components[0];
            }

            icon = AssetPreview.GetMiniThumbnail(component);

            if (icon == null)
            {
                return Reflection.objectIcon.image;
            }

            if (icon.name is "cs Script Icon" or "d_cs Script Icon")
            {
                return Reflection.targetIcon.image;
            }

            return icon;
        }

        private static void DrawIcon(Rect rect, GameObject target)
        {
            if (target == null) return;
            content.text = target.name;
            rect.width = GUI.skin.label.CalcSize(content).x;
            var distance = rect.x + rect.width + 14;
            distance = (int)(distance / 14) * 14 + 14;
            var render = target.GetComponent<Renderer>();
            var objects = target.GetComponents<Component>().ToList<Object>();
            var shared = render != null && render.sharedMaterials != null;
            if (shared)
            {
                objects.AddRange(render.sharedMaterials);
            }

            for (var i = 0; i < objects.Count; ++i)
            {
                if (objects[i] != null)
                {
                    var itemRect = new Rect(distance, rect.y, 12f, rect.height);
                    if (shared && i == objects.Count - render.sharedMaterials.Length)
                    {
                        foreach (var material in render.sharedMaterials)
                        {
                            if (material != null)
                            {
                                itemRect = new Rect(distance, rect.y, 12f, rect.height);
                                ItemIcon(itemRect, material);
                                distance += 14f;
                            }
                        }

                        break;
                    }

                    ItemIcon(itemRect, objects[i]);
                    distance += 14f;
                }
            }
        }

        private static void ItemIcon(Rect rect, Object item)
        {
            if (Event.current.type == EventType.Repaint)
            {
                GUI.DrawTexture(rect, EditorGUIUtility.ObjectContent(item, item.GetType()).image, ScaleMode.ScaleToFit);
            }
            else if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition) && Event.current.button == 1)
            {
                Reflection.ShowMenu(rect, item);
                Event.current.Use();
            }
        }
    }
}