// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-08 00:12:59
// # Recently: 2024-12-22 20:12:32
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal static class Reflection
    {
        private const BindingFlags Instance = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private const BindingFlags Static = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        
        private static readonly Assembly editor;

        public static readonly Type toolbarType;
        public static readonly Type importerType;
        public static readonly Type inspectorType;
        public static readonly Type hierarchyType;

        private static readonly FieldInfo iconWidth;
        private static readonly FieldInfo iconLabel;
        private static readonly FieldInfo toolbarRoot;
        private static readonly FieldInfo itemTreeViewA;
        private static readonly FieldInfo itemTreeViewB;
        private static readonly FieldInfo selectProject;
        private static readonly FieldInfo selectHierarchy;
        private static readonly FieldInfo activeHierarchy;
        private static readonly FieldInfo hierarchyTree;

        private static readonly MethodInfo showEditorWindow;
        private static readonly MethodInfo contextMenuMethod;
        private static readonly MethodInfo itemTreeViewE;
        private static readonly MethodInfo itemTreeViewF;

        private static readonly PropertyInfo itemTreeViewC;
        private static readonly PropertyInfo itemTreeViewD;
        private static readonly PropertyInfo editorElements;
        private static readonly PropertyInfo inspectorTracker;

        public static readonly GUIContent collapse;
        public static readonly GUIContent expansion;
        public static readonly GUIContent buildIcon;
        public static readonly GUIContent settingIcon;
        public static readonly GUIContent unityIcon;
        public static readonly GUIContent prefabIcon;
        public static readonly GUIContent objectIcon;
        public static readonly GUIContent targetIcon;
        public static readonly GUIContent customIcon;
        public static readonly GUIContent windowIcon;

        static Reflection()
        {
            editor = Assembly.Load("UnityEditor");

            toolbarType = GetEditorType("Toolbar");
            importerType = GetEditorType("PrefabImporter");
            inspectorType = GetEditorType("InspectorWindow");
            hierarchyType = GetEditorType("SceneHierarchyWindow");

            toolbarRoot = toolbarType.GetField("m_Root", Instance);
            activeHierarchy = hierarchyType.GetField("m_SceneHierarchy", Instance);
            selectHierarchy = hierarchyType.GetField("s_LastInteractedHierarchy", Static);
            inspectorTracker = inspectorType.GetProperty("tracker", Instance);

            var editorType = GetEditorAssembly("UIElements.EditorElement", "UnityEditor.CoreModule");
            editorElements = editorType.GetProperty("m_Editors", Instance);
            editorType = GetEditorType("SceneHierarchy");
            hierarchyTree = editorType.GetField("m_TreeView", Instance);
            editorType = GetEditorType("IMGUI.Controls.TreeViewGUI");
            iconWidth = editorType.GetField("k_IconWidth", Instance);
            iconLabel = editorType.GetField("k_SpaceBetweenIconAndText", Instance);
            editorType = GetEditorType("ProjectBrowser");
            selectProject = editorType.GetField("s_LastInteractedProjectBrowser", Static);
            itemTreeViewA = editorType.GetField("m_AssetTree", Instance);
            itemTreeViewB = editorType.GetField("m_FolderTree", Instance);
            editorType = GetEditorType("IMGUI.Controls.TreeViewController");
            itemTreeViewC = editorType.GetProperty("gui", Instance);
            itemTreeViewD = editorType.GetProperty("data", Instance);
            editorType = GetEditorType("ProjectBrowserColumnOneTreeViewDataSource");
            itemTreeViewE = editorType.GetMethod("GetRows", Instance);
            editorType = GetEditorType("AssetsTreeViewDataSource");
            itemTreeViewF = editorType.GetMethod("GetRows", Instance);
            editorType = GetAssembly("JFramework.Unity")?.GetType("JFramework.EditorSetting");
            showEditorWindow = editorType?.GetMethod("ShowWindow", Static);
            var types = new[] { typeof(Rect), typeof(Object), typeof(int) };
            contextMenuMethod = GetMethod(typeof(EditorUtility), Static, "DisplayObjectContextMenu", types);

            unityIcon = EditorGUIUtility.IconContent("UnityLogo");
            prefabIcon = EditorGUIUtility.IconContent("Prefab Icon");
            targetIcon = EditorGUIUtility.IconContent("Transform Icon");
            objectIcon = EditorGUIUtility.IconContent("GameObject Icon");

            buildIcon = EditorGUIUtility.IconContent("BuildSettings.Standalone");
            windowIcon = EditorGUIUtility.IconContent("UnityEditor.AnimationWindow");
            customIcon = EditorGUIUtility.IconContent("CustomTool");
            settingIcon = EditorGUIUtility.IconContent("SettingsIcon");

            var collapseIcon = EditorGUIUtility.IconContent("Download-Available");
            collapse = new GUIContent(collapseIcon.image, "Expand Components");
            var expansionIcon = EditorGUIUtility.IconContent("Toolbar Plus More");
            expansion = new GUIContent(expansionIcon.image, "Collapse Components");
        }

        private static Assembly GetAssembly(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.FirstOrDefault(assembly => assembly.GetName().Name == name);
        }

        private static Type GetEditorAssembly(string name, string assemblyName)
        {
            var editorAssembly = GetAssembly(assemblyName);
            return editorAssembly != null ? editorAssembly.GetType("UnityEditor." + name) : null;
        }

        private static Type GetEditorType(string name)
        {
            return editor.GetType("UnityEditor." + name);
        }

        private static MethodInfo GetMethod(Type type, BindingFlags flags, string name, Type[] types)
        {
            return type.GetMethod(name, flags, null, types, null);
        }

        public static VisualElement GetRoot(ScriptableObject toolbar)
        {
            return (VisualElement)toolbarRoot.GetValue(toolbar);
        }

        public static ActiveEditorTracker GetTracker(object obj)
        {
            return (ActiveEditorTracker)inspectorTracker.GetValue(obj);
        }

        public static UnityEditor.Editor[] GetEditors(object instance)
        {
            return editorElements.GetValue(instance) as UnityEditor.Editor[];
        }

        public static EditorWindow GetHierarchy()
        {
            return selectHierarchy.GetValue(null) as EditorWindow;
        }

        public static void ShowMenu(Rect position, Object context)
        {
            contextMenuMethod?.Invoke(null, new object[] { position, context, 0 });
        }

        public static void HideIcon(EditorWindow window)
        {
            if (window == null) return;
            var hierarchy = activeHierarchy.GetValue(window);
            if (hierarchy == null) return;
            var treeView = hierarchyTree.GetValue(hierarchy);
            if (treeView == null) return;
            var itemTree = itemTreeViewC.GetValue(treeView);
            if (itemTree == null) return;
            iconWidth.SetValue(itemTree, 0);
            iconLabel.SetValue(itemTree, 18);
        }

        public static bool HasChild(int assetId)
        {
            var window = selectProject.GetValue(null) as EditorWindow;
            if (window == null) return false;
            IEnumerable<TreeViewItem> items = null;
            var itemTree = itemTreeViewA.GetValue(window);
            if (itemTree != null)
            {
                var data = itemTreeViewD.GetValue(itemTree, null);
                items = (IEnumerable<TreeViewItem>)itemTreeViewE.Invoke(data, null);
            }

            itemTree = itemTreeViewB.GetValue(window);
            if (itemTree != null)
            {
                var data = itemTreeViewD.GetValue(itemTree, null);
                items = (IEnumerable<TreeViewItem>)itemTreeViewF.Invoke(data, null);
            }

            return items != null && items.Where(item => item.id == assetId).Any(item => item.hasChildren);
        }
        
        public static void ShowEditorWindow()
        {
            showEditorWindow?.Invoke(null, null);
        }
    }
}