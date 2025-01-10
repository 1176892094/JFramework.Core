// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:42
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
        public static partial class Asset
        {
            public static async void LoadAssetData()
            {
                var platform = await LoadAssetPack(helper.assetPlatform);
                manifest ??= platform.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
                Utility.Event.Invoke(new AssetAwakeEvent(manifest.GetAllAssetBundles()));

                var assetPacks = manifest.GetAllAssetBundles();
                foreach (var assetPack in assetPacks)
                {
                    _ = LoadAssetPack(assetPack);
                }

                await Task.WhenAll(assetTask.Values);
                Utility.Event.Invoke(new AssetCompleteEvent());
            }

            public static async Task<T> Load<T>(string assetPath) where T : Object
            {
                try
                {
                    if (helper == null) return default;
                    var assetData = await LoadAsset(assetPath, typeof(T));
                    if (assetData != null)
                    {
                        return (T)assetData;
                    }

                    Log.Warn(Utility.Text.Format("加载资源 {0} 为空!", assetPath));
                }
                catch (Exception e)
                {
                    Log.Warn(Utility.Text.Format("加载资源 {0} 失败!\n{1}", assetPath, e));
                }

                return default;
            }

            public static async void Load<T>(string assetPath, Action<T> assetAction) where T : Object
            {
                try
                {
                    if (helper == null) return;
                    var assetData = await LoadAsset(assetPath, typeof(T));
                    if (assetData != null)
                    {
                        assetAction.Invoke((T)assetData);
                        return;
                    }

                    Log.Warn(Utility.Text.Format("加载资源 {0} 为空!", assetPath));
                }
                catch (Exception e)
                {
                    Log.Warn(Utility.Text.Format("加载资源 {0} 失败!\n{1}", assetPath, e));
                }
            }

            private static async Task<Object> LoadAsset(string assetPath, Type assetType)
            {
                if (helper.assetPackMode)
                {
                    var assetPair = await LoadAssetPair(assetPath);
                    var assetPack = await LoadAssetPack(assetPair.Key);
                    var assetData = helper.LoadByAssetPack(assetPair.Value, assetType, assetPack);
                    assetData ??= helper.LoadByResources(assetPath, assetType);
                    return assetData;
                }
                else
                {
                    var assetData = helper.LoadBySimulates(assetPath, assetType);
                    assetData ??= helper.LoadByResources(assetPath, assetType);
                    return assetData;
                }
            }

            private static async Task<KeyValuePair<string, string>> LoadAssetPair(string assetPath)
            {
                if (!Service.assetData.TryGetValue(assetPath, out var assetData))
                {
                    var index = assetPath.LastIndexOf('/');
                    if (index < 0)
                    {
                        assetData = new KeyValuePair<string, string>(string.Empty, assetPath);
                    }
                    else
                    {
                        var assetPack = assetPath.Substring(0, index).ToLower();
                        assetData = new KeyValuePair<string, string>(assetPack, assetPath.Substring(index + 1));
                    }

                    Service.assetData.Add(assetPath, assetData);
                }

                var platform = await LoadAssetPack(helper.assetPlatform);
                manifest ??= platform.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));

                var assetPacks = manifest.GetAllDependencies(assetData.Key);
                foreach (var assetPack in assetPacks)
                {
                    _ = LoadAssetPack(assetPack);
                }

                return assetData;
            }

            private static async Task<AssetBundle> LoadAssetPack(string assetPath)
            {
                if (string.IsNullOrEmpty(assetPath))
                {
                    return null;
                }

                if (Service.assetPack.TryGetValue(assetPath, out var assetPack))
                {
                    return assetPack;
                }

                if (Service.assetTask.TryGetValue(assetPath, out var assetTask))
                {
                    return await assetTask;
                }

                var persistentData = GetPacketPath(assetPath);
                var streamingAssets = GetClientPath(assetPath);
                assetTask = Pack.LoadAssetRequest(persistentData, streamingAssets);
                Service.assetTask.Add(assetPath, assetTask);
                try
                {
                    assetPack = await assetTask;
                    Service.assetPack.Add(assetPath, assetPack);
                    Utility.Event.Invoke(new AssetUpdateEvent(assetPath));
                    return assetPack;
                }
                finally
                {
                    Service.assetTask.Remove(assetPath);
                }
            }

            internal static void Dispose()
            {
                assetData.Clear();
                assetTask.Clear();
                assetPack.Clear();
            }
        }
    }
}