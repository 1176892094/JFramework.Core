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
using JFramework.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JFramework
{
    public static partial class AssetManager
    {
        public static async void LoadScene(string assetPath)
        {
            try
            {
                if (!GlobalSetting.Runtime) return;
                var assetData = await LoadSceneAsset(GlobalSetting.GetScenePath(assetPath));
                if (assetData != null)
                {
                    Service.Event.Invoke(new SceneAwakeEvent(assetPath));
                    var request = SceneManager.LoadSceneAsync(assetPath, LoadSceneMode.Single);
                    if (request != null)
                    {
                        while (!request.isDone && GlobalSetting.Runtime != null)
                        {
                            Service.Event.Invoke(new SceneUpdateEvent(request.progress));
                            await Task.Yield();
                        }
                    }

                    Service.Event.Invoke(new SceneCompleteEvent());
                    return;
                }

                Debug.LogWarning(Service.Text.Format("加载资源 {0} 为空!", assetPath));
            }
            catch (Exception e)
            {
                Debug.LogWarning(Service.Text.Format("加载场景 {0} 失败!\n{1}", assetPath, e));
            }
        }

        private static async Task<string> LoadSceneAsset(string assetPath)
        {
            if (GlobalSetting.assetLoadMode)
            {
                var assetPair = await LoadAssetPair(assetPath);
                var assetPack = await LoadAssetPack(assetPair.Key);
                var assetData = assetPack.GetAllScenePaths();
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