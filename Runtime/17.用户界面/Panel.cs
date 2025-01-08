// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-24 02:12:55
// # Recently: 2025-01-08 17:01:23
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
            private static async Task<IPanel> Load(string assetPath, Type assetType)
            {
                var obj = await Asset.Load<GameObject>(assetPath);
                var component = obj.GetComponent(assetType);
                if (component == null)
                {
                    component = obj.AddComponent(assetType);
                }

                var panel = (IPanel)component;
                panelData.Add(assetType, panel);
                Surface(panel);
                return panel;
            }

            public static async void Show<T>(Action<T> assetAction = null) where T : IPanel
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

            public static void Hide<T>() where T : IPanel
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

            public static T Find<T>() where T : IPanel
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

            public static async void Show(Type assetType, Action<IPanel> assetAction = null)
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

            public static IPanel Find(Type assetType)
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

            public static void Surface(IPanel panel, int layer = 1)
            {
                if (helper == null) return;
                panelHelper.Surface(panel, layer);
            }

            private static void Destroy(IPanel panel, Type assetType)
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
            }
        }
    }
}