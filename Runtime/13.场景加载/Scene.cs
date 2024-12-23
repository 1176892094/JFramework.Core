// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:27
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Linq;
using System.Threading.Tasks;

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
                        assetHelper.LoadScene(assetData);
                    }
                }
                catch (Exception e)
                {
                    Warn(Text.Format("加载场景 {0} 失败!\n{1}", assetPath, e));
                    return;
                }

                Warn(Text.Format("加载资源 {0} 为空!", assetPath));
            }

            private static async Task<string> LoadSceneAsset(string assetPath)
            {
                if (pathHelper.assetPackMode)
                {
                    var assetPair = await LoadAssetPair(assetPath);
                    var assetPack = await LoadAssetPack(assetPair.Key);
                    var assetData = assetHelper.GetAllScenePaths(assetPack);
                    return assetData.FirstOrDefault(data => data == assetPair.Value);
                }

                return assetPath.Substring(assetPath.LastIndexOf('/') + 1);
            }
        }
    }
}