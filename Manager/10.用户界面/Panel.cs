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

            obj.name = assetPath;
            var panel = (UIPanel)component;
            GlobalManager.panelData.Add(assetType, panel);
            Surface(panel.transform, panel.layer);
            return panel;
        }

        public static async void Show<T>(Action<T> assetAction = null) where T : UIPanel
        {
            if (!GlobalManager.Instance) return;
            var assetPath = GlobalSetting.GetPanelPath(typeof(T).Name);
            if (!GlobalManager.panelData.TryGetValue(typeof(T), out var panel))
            {
                panel = await Load(assetPath, typeof(T));
                panel.Show();
            }

            if (ShowInGroup(panel))
            {
                panel.Show();
            }

            assetAction?.Invoke((T)panel);
        }

        public static void Hide<T>() where T : UIPanel
        {
            if (!GlobalManager.Instance) return;
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
            if (!GlobalManager.Instance) return default;
            if (GlobalManager.panelData.TryGetValue(typeof(T), out var panel))
            {
                return (T)panel;
            }

            return default;
        }

        public static void Destroy<T>()
        {
            if (!GlobalManager.Instance) return;
            if (GlobalManager.panelData.TryGetValue(typeof(T), out var panel))
            {
                Destroy(panel, typeof(T));
            }
        }

        public static async void Show(Type assetType, Action<UIPanel> assetAction = null)
        {
            if (!GlobalManager.Instance) return;
            var assetPath = GlobalSetting.GetPanelPath(assetType.Name);
            if (!GlobalManager.panelData.TryGetValue(assetType, out var panel))
            {
                panel = await Load(assetPath, assetType);
                panel.Show();
            }

            if (ShowInGroup(panel))
            {
                panel.Show();
            }

            assetAction?.Invoke(panel);
        }

        public static void Hide(Type assetType)
        {
            if (!GlobalManager.Instance) return;
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
            if (!GlobalManager.Instance) return default;
            if (GlobalManager.panelData.TryGetValue(assetType, out var panel))
            {
                return panel;
            }

            return default;
        }

        public static void Destroy(Type assetType)
        {
            if (!GlobalManager.Instance) return;
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

        public static void Surface(Transform panel, UILayer layer)
        {
            if (!GlobalManager.Instance) return;
            if (!GlobalManager.panelLayer.TryGetValue(layer, out var parent))
            {
                var name = Service.Text.Format("Pool - Canvas/{0}", layer);
                var child = new GameObject(name, typeof(RectTransform));
                child.transform.SetParent(GlobalManager.canvas.transform);
                parent = child.GetComponent<RectTransform>();
                parent.anchorMin = Vector2.zero;
                parent.anchorMax = Vector2.one;
                parent.offsetMin = Vector2.zero;
                parent.offsetMax = Vector2.zero;
                parent.localScale = Vector3.one;
                parent.localPosition = Vector3.zero;
                GlobalManager.panelLayer.Add(layer, parent);
                parent.SetSiblingIndex((byte)layer);
            }

            var transform = (RectTransform)panel;
            transform.SetParent(parent);
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.offsetMin = Vector2.zero;
            transform.offsetMax = Vector2.zero;
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }

        private static void Destroy(UIPanel panel, Type assetType)
        {
            if (!GlobalManager.Instance) return;
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