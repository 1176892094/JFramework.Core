// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 19:12:28
// # Recently: 2024-12-22 20:12:26
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.Linq;

namespace JFramework
{
    public static partial class Service
    {
        public static class Group
        {
            public static void Listen(string group, IPanel panel)
            {
                if (!Service.panelGroup.TryGetValue(group, out var panelGroup))
                {
                    panelGroup = new HashSet<IPanel>();
                    Service.panelGroup.Add(group, panelGroup);
                }

                if (!Service.groupPanel.TryGetValue(panel, out var groupPanel))
                {
                    groupPanel = new HashSet<string>();
                    Service.groupPanel.Add(panel, groupPanel);
                }

                if (panelGroup.Add(panel))
                {
                    groupPanel.Add(group);
                }
            }

            public static void Remove(string group, IPanel panel)
            {
                if (!Service.panelGroup.TryGetValue(group, out var panelGroup))
                {
                    panelGroup = new HashSet<IPanel>();
                    Service.panelGroup.Add(group, panelGroup);
                }

                if (!Service.groupPanel.TryGetValue(panel, out var groupPanel))
                {
                    groupPanel = new HashSet<string>();
                    Service.groupPanel.Add(panel, groupPanel);
                }

                if (panelGroup.Remove(panel))
                {
                    groupPanel.Remove(group);
                }
            }

            public static void Show(string group)
            {
                if (Service.panelGroup.TryGetValue(group, out var panelGroup))
                {
                    foreach (var panel in panelGroup)
                    {
                        if (!IsActive(panel))
                        {
                            panel.Show();
                        }
                    }
                }
            }

            public static void Hide(string group)
            {
                if (Service.panelGroup.TryGetValue(group, out var panelGroup))
                {
                    foreach (var panel in panelGroup)
                    {
                        if (IsActive(panel))
                        {
                            panel.Hide();
                        }
                    }
                }
            }

            internal static bool ShowInGroup(IPanel panel)
            {
                if (!Service.groupPanel.TryGetValue(panel, out var groupPanel))
                {
                    groupPanel = new HashSet<string>();
                    Service.groupPanel.Add(panel, groupPanel);
                }

                foreach (var group in groupPanel)
                {
                    if (!Service.panelGroup.TryGetValue(group, out var panelGroup))
                    {
                        continue;
                    }

                    foreach (var other in panelGroup)
                    {
                        if (panel != other && IsActive(other))
                        {
                            Pool.Hide(other);
                        }
                    }
                }

                return !IsActive(panel);
            }

            internal static void Dispose()
            {
                var groupPanel = Service.groupPanel.Keys.ToList();
                foreach (var panel in groupPanel)
                {
                    if (Service.groupPanel.TryGetValue(panel, out var group))
                    {
                        group.Clear();
                    }
                }

                Service.groupPanel.Clear();

                var panelGroup = Service.panelGroup.Keys.ToList();
                foreach (var group in panelGroup)
                {
                    if (Service.panelGroup.TryGetValue(group, out var panel))
                    {
                        panel.Clear();
                    }
                }

                Service.panelGroup.Clear();
            }
        }
    }
}