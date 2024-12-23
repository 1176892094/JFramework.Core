// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:39
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AssetData = System.Collections.Generic.KeyValuePair<string, string>;
using AgentData = System.Collections.Generic.Dictionary<System.Type, JFramework.IAgent>;
using EnumTable = System.Collections.Generic.Dictionary<System.Enum, JFramework.IData>;
using ItemTable = System.Collections.Generic.Dictionary<int, JFramework.IData>;
using NameTable = System.Collections.Generic.Dictionary<string, JFramework.IData>;

namespace JFramework
{
    public static partial class Service
    {
        private static IBaseHelper helper;

        private static readonly AudioSetting setting = new AudioSetting();

        private static readonly List<IUpdate> timerData = new List<IUpdate>();

        private static readonly Dictionary<Type, IPool> heapData = new Dictionary<Type, IPool>();

        private static readonly Dictionary<Type, IPool> eventData = new Dictionary<Type, IPool>();

        private static readonly Dictionary<Type, ItemTable> itemTable = new Dictionary<Type, ItemTable>();

        private static readonly Dictionary<Type, NameTable> nameTable = new Dictionary<Type, NameTable>();

        private static readonly Dictionary<Type, EnumTable> enumTable = new Dictionary<Type, EnumTable>();

        private static readonly Dictionary<string, PackData> clientPacks = new Dictionary<string, PackData>();

        private static readonly Dictionary<string, PackData> serverPacks = new Dictionary<string, PackData>();

        private static readonly Dictionary<string, object> assetPack = new Dictionary<string, object>();

        private static readonly Dictionary<string, AssetData> assetData = new Dictionary<string, AssetData>();

        private static readonly Dictionary<string, Task<object>> assetTask = new Dictionary<string, Task<object>>();

        private static readonly Dictionary<string, Type> cachedType = new Dictionary<string, Type>();

        private static readonly Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

        private static readonly Dictionary<string, IPool> poolData = new Dictionary<string, IPool>();

        private static readonly Dictionary<IEntity, AgentData> agentData = new Dictionary<IEntity, AgentData>();

        private static readonly Dictionary<string, HashSet<IPanel>> panelGroup = new Dictionary<string, HashSet<IPanel>>();

        private static readonly Dictionary<IPanel, HashSet<string>> groupPanel = new Dictionary<IPanel, HashSet<string>>();

        private static readonly Dictionary<Type, IPanel> panelData = new Dictionary<Type, IPanel>();

        private static IPathHelper pathHelper => (IPathHelper)helper;
        private static IJsonHelper jsonHelper => (IJsonHelper)helper;
        private static IPackHelper packHelper => (IPackHelper)helper;
        private static IPoolHelper poolHelper => (IPoolHelper)helper;
        private static IPanelHelper panelHelper => (IPanelHelper)helper;
        private static IAudioHelper audioHelper => (IAudioHelper)helper;
        private static IAssetHelper assetHelper => (IAssetHelper)helper;
    }
}