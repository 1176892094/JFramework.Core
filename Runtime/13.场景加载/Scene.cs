// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace JFramework
{
    public static partial class Service
    {
        public static partial class Asset
        {
            public static async void LoadScene(string assetPath)
            {
                try
                {
                    if (helper == null) return;
                    var assetData = await LoadSceneAsset(GetScenePath(assetPath));
                    if (assetData != null)
                    {
                        Event.Invoke(new SceneAwakeEvent(assetPath));
                        var request = SceneManager.LoadSceneAsync(assetPath, LoadSceneMode.Single);
                        if (request != null)
                        {
                            while (!request.isDone && helper != null)
                            {
                                Event.Invoke(new SceneUpdateEvent(request.progress));
                                await Task.Yield();
                            }
                        }

                        Event.Invoke(new SceneCompleteEvent());
                        return;
                    }

                    Log.Warn(Text.Format("加载资源 {0} 为空!", assetPath));
                }
                catch (Exception e)
                {
                    Log.Warn(Text.Format("加载场景 {0} 失败!\n{1}", assetPath, e));
                }
            }

            private static async Task<string> LoadSceneAsset(string assetPath)
            {
                if (pathHelper.assetPackMode)
                {
                    var assetPair = await LoadAssetPair(assetPath);
                    var assetPack = await LoadAssetPack(assetPair.Key);
                    var assetData = assetHelper.GetAllScenePaths(assetPack);
                    foreach (var data in assetData)
                    {
                        if (data == assetPair.Value)
                        {
                            return data;
                        }
                    }
                }

                return assetPath.Substring(assetPath.LastIndexOf('/') + 1);
            }
        }
    }
}