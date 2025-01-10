// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:37
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using AssetData = System.Collections.Generic.KeyValuePair<string, string>;
using AgentData = System.Collections.Generic.Dictionary<System.Type, JFramework.IAgent>;
using EnumTable = System.Collections.Generic.Dictionary<System.Enum, JFramework.IData>;
using ItemTable = System.Collections.Generic.Dictionary<int, JFramework.IData>;
using NameTable = System.Collections.Generic.Dictionary<string, JFramework.IData>;

namespace JFramework
{
    public static partial class Service
    {
        public static Canvas canvas;
        
        public static GameObject manager;
        
        internal static IEntity entity;
        
        private static Helper helper;
        
        private static AudioSource musicSource;
        
        private static AssetBundleManifest manifest;

        private static readonly AudioSetting setting = new AudioSetting();

        private static readonly List<ITimer> timerData = new List<ITimer>();

        private static readonly HashSet<AudioSource> audioData = new HashSet<AudioSource>();

        private static readonly Dictionary<Type, UIPanel> panelData = new Dictionary<Type, UIPanel>();

        private static readonly Dictionary<Type, ItemTable> itemTable = new Dictionary<Type, ItemTable>();

        private static readonly Dictionary<Type, NameTable> nameTable = new Dictionary<Type, NameTable>();

        private static readonly Dictionary<Type, EnumTable> enumTable = new Dictionary<Type, EnumTable>();

        private static readonly Dictionary<IEntity, AgentData> agentData = new Dictionary<IEntity, AgentData>();

        private static readonly Dictionary<string, PackData> clientPacks = new Dictionary<string, PackData>();

        private static readonly Dictionary<string, PackData> serverPacks = new Dictionary<string, PackData>();

        private static readonly Dictionary<string, AssetData> assetData = new Dictionary<string, AssetData>();

        private static readonly Dictionary<string, AssetBundle> assetPack = new Dictionary<string, AssetBundle>();

        private static readonly Dictionary<string, Task<AssetBundle>> assetTask = new Dictionary<string, Task<AssetBundle>>();

        private static readonly Dictionary<string, IPool> poolData = new Dictionary<string, IPool>();

        private static readonly Dictionary<string, GameObject> poolGroup = new Dictionary<string, GameObject>();

        private static readonly Dictionary<int, RectTransform> panelLayer = new Dictionary<int, RectTransform>();

        private static readonly Dictionary<string, HashSet<UIPanel>> panelGroup = new Dictionary<string, HashSet<UIPanel>>();

        private static readonly Dictionary<UIPanel, HashSet<string>> groupPanel = new Dictionary<UIPanel, HashSet<string>>();
    }
}