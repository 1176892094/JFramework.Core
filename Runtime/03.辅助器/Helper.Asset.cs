// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:31
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public interface IAssetHelper : Helper
    {
        bool assetPackMode { get; }
        string assetPlatform { get; }
        string assetPackPath { get; }
        string assetPackName { get; }
        string assetRemotePath { get; }
        Task<string> LoadServerRequest(string packName, string packUri);
        Task<byte[]> LoadPacketRequest(string packName, string packUri);
        Task<string> LoadClientRequest(string persistentData, string streamingAssets);
        Task<AssetBundle> LoadAssetRequest(string persistentData, string streamingAssets);
        Object LoadByAssetPack(string assetPath, Type assetType, AssetBundle assetPack);
        Object LoadByResources(string assetPath, Type assetType);
        Object LoadBySimulates(string assetPath, Type assetType);
    }
}