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
using EnumTable = System.Collections.Generic.Dictionary<System.Enum, JFramework.IData>;
using ItemTable = System.Collections.Generic.Dictionary<int, JFramework.IData>;
using NameTable = System.Collections.Generic.Dictionary<string, JFramework.IData>;
using AgentData = System.Collections.Generic.Dictionary<System.Type, UnityEngine.ScriptableObject>;

namespace JFramework
{
    public partial class GlobalManager : MonoBehaviour
    {
        internal static Helper helper;
        
        internal static Canvas canvas;

        internal static GameObject entity;
        
        internal static GameObject manager;

        internal static AudioSource musicSource;

        internal static AssetBundleManifest manifest;

        internal static readonly AudioSetting setting = new AudioSetting();

        internal static readonly List<ITimer> timerData = new List<ITimer>();

        internal static readonly HashSet<AudioSource> audioData = new HashSet<AudioSource>();

        internal static readonly Dictionary<Type, UIPanel> panelData = new Dictionary<Type, UIPanel>();

        internal static readonly Dictionary<Type, ItemTable> itemTable = new Dictionary<Type, ItemTable>();

        internal static readonly Dictionary<Type, NameTable> nameTable = new Dictionary<Type, NameTable>();

        internal static readonly Dictionary<Type, EnumTable> enumTable = new Dictionary<Type, EnumTable>();

        internal static readonly Dictionary<string, PackData> clientPacks = new Dictionary<string, PackData>();

        internal static readonly Dictionary<string, PackData> serverPacks = new Dictionary<string, PackData>();

        internal static readonly Dictionary<string, AssetData> assetData = new Dictionary<string, AssetData>();

        internal static readonly Dictionary<string, AssetBundle> assetPack = new Dictionary<string, AssetBundle>();

        internal static readonly Dictionary<string, Task<AssetBundle>> assetTask = new Dictionary<string, Task<AssetBundle>>();

        internal static readonly Dictionary<string, IPool> poolData = new Dictionary<string, IPool>();

        internal static readonly Dictionary<string, GameObject> poolGroup = new Dictionary<string, GameObject>();

        internal static readonly Dictionary<GameObject, AgentData> agentData = new Dictionary<GameObject, AgentData>();

        internal static readonly Dictionary<int, RectTransform> panelLayer = new Dictionary<int, RectTransform>();

        internal static readonly Dictionary<string, HashSet<UIPanel>> panelGroup = new Dictionary<string, HashSet<UIPanel>>();

        internal static readonly Dictionary<UIPanel, HashSet<string>> groupPanel = new Dictionary<UIPanel, HashSet<string>>();
    }
}