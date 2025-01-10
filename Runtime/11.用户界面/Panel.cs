// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-10 20:01:57
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
    public static partial class UIManager
    {
        private static async Task<UIPanel> Load(string assetPath, Type assetType)
        {
            var obj = await AssetManager.Load<GameObject>(assetPath);
            var component = obj.GetComponent(assetType);
            if (component == null)
            {
                component = obj.AddComponent(assetType);
            }

            var panel = (UIPanel)component;
            GlobalManager.panelData.Add(assetType, panel);
            Surface(panel);
            return panel;
        }

        public static async void Show<T>(Action<T> assetAction = null) where T : UIPanel
        {
            if (GlobalManager.helper == null) return;
            var assetPath = GlobalManager.GetPanelPath(typeof(T).Name);
            if (!GlobalManager.panelData.TryGetValue(typeof(T), out var panel))
            {
                panel = await Load(assetPath, typeof(T));
                panel.Show();
            }
            else if (ShowInGroup(panel))
            {
                panel.Show();
            }

            assetAction?.Invoke((T)panel);
        }

        public static void Hide<T>() where T : UIPanel
        {
            if (GlobalManager.helper == null) return;
            if (GlobalManager.panelData.TryGetValue(typeof(T), out var panel))
            {
                if (panel.gameObject.activeInHierarchy)
                {
                    panel.Hide();
                }
            }
        }

        public static T Find<T>() where T : UIPanel
        {
            if (GlobalManager.helper == null) return default;
            if (GlobalManager.panelData.TryGetValue(typeof(T), out var panel))
            {
                return (T)panel;
            }

            return default;
        }

        public static void Destroy<T>()
        {
            if (GlobalManager.helper == null) return;
            if (GlobalManager.panelData.TryGetValue(typeof(T), out var panel))
            {
                Destroy(panel, typeof(T));
            }
        }

        public static async void Show(Type assetType, Action<UIPanel> assetAction = null)
        {
            if (GlobalManager.helper == null) return;
            var assetPath = GlobalManager.GetPanelPath(assetType.Name);
            if (!GlobalManager.panelData.TryGetValue(assetType, out var panel))
            {
                panel = await Load(assetPath, assetType);
                panel.Show();
            }
            else if (ShowInGroup(panel))
            {
                panel.Show();
            }

            assetAction?.Invoke(panel);
        }

        public static void Hide(Type assetType)
        {
            if (GlobalManager.helper == null) return;
            if (GlobalManager.panelData.TryGetValue(assetType, out var panel))
            {
                if (panel.gameObject.activeInHierarchy)
                {
                    panel.Hide();
                }
            }
        }

        public static UIPanel Find(Type assetType)
        {
            if (GlobalManager.helper == null) return default;
            if (GlobalManager.panelData.TryGetValue(assetType, out var panel))
            {
                return panel;
            }

            return default;
        }

        public static void Destroy(Type assetType)
        {
            if (GlobalManager.helper == null) return;
            if (GlobalManager.panelData.TryGetValue(assetType, out var panel))
            {
                Destroy(panel, assetType);
            }
        }

        public static void Clear()
        {
            var panelData = new List<Type>(GlobalManager.panelData.Keys);
            foreach (var assetType in panelData)
            {
                if (GlobalManager.panelData.TryGetValue(assetType, out var panel))
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
            if (GlobalManager.helper == null) return;
            if (GlobalManager.canvas == null)
            {
                GlobalManager.canvas = new GameObject("UIManager").AddComponent<Canvas>();
                GlobalManager.canvas.renderMode = RenderMode.ScreenSpaceCamera;
                Object.DontDestroyOnLoad(GlobalManager.canvas.gameObject);

                // var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
                // scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                // scaler.referenceResolution = new Vector2(1920, 1080);
                // scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                // scaler.matchWidthOrHeight = 0.5f;
            }

            if (!GlobalManager.panelLayer.TryGetValue(layer, out var parent))
            {
                var name = Utility.Text.Format("Layer-{0}", layer);
                var child = new GameObject(name);
                child.transform.SetParent(GlobalManager.canvas.transform);
                var renderer = child.AddComponent<Canvas>();
                renderer.overrideSorting = true;
                renderer.sortingOrder = layer;
                parent = child.GetComponent<RectTransform>();
                parent.anchorMin = Vector2.zero;
                parent.anchorMax = Vector2.one;
                parent.offsetMin = Vector2.zero;
                parent.offsetMax = Vector2.zero;
                parent.localScale = Vector3.one;
                GlobalManager.panelLayer.Add(layer, parent);
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
            if (GlobalManager.helper == null) return;
            if (GlobalManager.groupPanel.TryGetValue(panel, out var groupPanel))
            {
                foreach (var group in groupPanel)
                {
                    GlobalManager.panelGroup.Remove(group);
                }

                groupPanel.Clear();
                GlobalManager.groupPanel.Remove(panel);
            }

            panel.Hide();
            GlobalManager.panelData.Remove(assetType);
            Object.Destroy(panel.gameObject);
        }
    }
}