// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 21:01:54
// # Recently: 2025-01-09 21:01:54
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
    public partial class Service
    {
        public interface Helper
        {
            bool assetPackMode { get; }
            string assetPlatform { get; }
            string assetPackPath { get; }
            string assetPackName { get; }
            string assetRemotePath { get; }
            string assemblyName { get; }
            string scriptDataPath { get; }
            string assetDataPath { get; }
            void CreateAsset(Object assetData, string assetPath);
            void CreateProgress(string assetPath, float progress);
            Task<KeyValuePair<int, string>> LoadRequest(string persistentData, string streamingAssets);
            Object LoadByAssetPack(string assetPath, Type assetType, AssetBundle assetPack);
            Object LoadByResources(string assetPath, Type assetType);
            Object LoadBySimulates(string assetPath, Type assetType);
        }
    }
}