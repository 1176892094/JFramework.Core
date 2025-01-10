// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-09 20:01:18
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class Service
    {
        public static class Panel
        {
            private static async Task<UIPanel> Load(string assetPath, Type assetType)
            {
                var obj = await Asset.Load<GameObject>(assetPath);
                var component = obj.GetComponent(assetType);
                if (component == null)
                {
                    component = obj.AddComponent(assetType);
                }

                var panel = (UIPanel)component;
                panelData.Add(assetType, panel);
                Surface(panel);
                return panel;
            }

            public static async void Show<T>(Action<T> assetAction = null) where T : UIPanel
            {
                if (helper == null) return;
                var assetPath = GetPanelPath(typeof(T).Name);
                if (!panelData.TryGetValue(typeof(T), out var panel))
                {
                    panel = await Load(assetPath, typeof(T));
                    panel.Show();
                }
                else if (Group.ShowInGroup(panel))
                {
                    panel.Show();
                }

                assetAction?.Invoke((T)panel);
            }

            public static void Hide<T>() where T : UIPanel
            {
                if (helper == null) return;
                if (panelData.TryGetValue(typeof(T), out var panel))
                {
                    if (panel.gameObject.activeInHierarchy)
                    {
                        panel.Hide();
                    }
                }
            }

            public static T Find<T>() where T : UIPanel
            {
                if (helper == null) return default;
                if (panelData.TryGetValue(typeof(T), out var panel))
                {
                    return (T)panel;
                }

                return default;
            }

            public static void Destroy<T>()
            {
                if (helper == null) return;
                if (panelData.TryGetValue(typeof(T), out var panel))
                {
                    Destroy(panel, typeof(T));
                }
            }

            public static async void Show(Type assetType, Action<UIPanel> assetAction = null)
            {
                if (helper == null) return;
                var assetPath = GetPanelPath(assetType.Name);
                if (!panelData.TryGetValue(assetType, out var panel))
                {
                    panel = await Load(assetPath, assetType);
                    panel.Show();
                }
                else if (Group.ShowInGroup(panel))
                {
                    panel.Show();
                }

                assetAction?.Invoke(panel);
            }

            public static void Hide(Type assetType)
            {
                if (helper == null) return;
                if (panelData.TryGetValue(assetType, out var panel))
                {
                    if (panel.gameObject.activeInHierarchy)
                    {
                        panel.Hide();
                    }
                }
            }

            public static UIPanel Find(Type assetType)
            {
                if (helper == null) return default;
                if (panelData.TryGetValue(assetType, out var panel))
                {
                    return panel;
                }

                return default;
            }

            public static void Destroy(Type assetType)
            {
                if (helper == null) return;
                if (panelData.TryGetValue(assetType, out var panel))
                {
                    Destroy(panel, assetType);
                }
            }

            public static void Clear()
            {
                var panelData = new List<Type>(Service.panelData.Keys);
                foreach (var assetType in panelData)
                {
                    if (Service.panelData.TryGetValue(assetType, out var panel))
                    {
                        if (panel.state != UIState.Stable)
                        {
                            Destroy(panel, assetType);
                        }
                    }
                }
            }

            public static void Surface(UIPanel panel, int layer = 1)
            {
                if (helper == null) return;
                if (canvas == null)
                {
                    canvas = new GameObject("UIManager").AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    Object.DontDestroyOnLoad(canvas.gameObject);

                    // var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
                    // scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    // scaler.referenceResolution = new Vector2(1920, 1080);
                    // scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    // scaler.matchWidthOrHeight = 0.5f;
                }

                if (!panelLayer.TryGetValue(layer, out var parent))
                {
                    var name = Text.Format("Layer-{0}", layer);
                    var child = new GameObject(name);
                    child.transform.SetParent(canvas.transform);
                    var renderer = child.AddComponent<Canvas>();
                    renderer.overrideSorting = true;
                    renderer.sortingOrder = layer;
                    parent = child.GetComponent<RectTransform>();
                    parent.anchorMin = Vector2.zero;
                    parent.anchorMax = Vector2.one;
                    parent.offsetMin = Vector2.zero;
                    parent.offsetMax = Vector2.zero;
                    parent.localScale = Vector3.one;
                    panelLayer.Add(layer, parent);
                    parent.SetSiblingIndex(layer);
                }

                var transform = (RectTransform)panel.transform;
                transform.SetParent(parent);
                transform.anchorMin = Vector2.zero;
                transform.anchorMax = Vector2.one;
                transform.offsetMin = Vector2.zero;
                transform.offsetMax = Vector2.zero;
                transform.localScale = Vector3.one;
            }

            private static void Destroy(UIPanel panel, Type assetType)
            {
                if (helper == null) return;
                if (Service.groupPanel.TryGetValue(panel, out var groupPanel))
                {
                    foreach (var group in groupPanel)
                    {
                        panelGroup.Remove(group);
                    }

                    groupPanel.Clear();
                    Service.groupPanel.Remove(panel);
                }

                panel.Hide();
                panelData.Remove(assetType);
                Object.Destroy(panel.gameObject);
            }

            internal static void Dispose()
            {
                panelData.Clear();
                panelLayer.Clear();
            }
        }
    }
}