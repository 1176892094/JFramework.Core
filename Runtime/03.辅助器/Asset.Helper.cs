// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:34
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public interface IAssetHelper : IBaseHelper
    {
        string[] GetAllDependency(object assetPack, string assetPath);
        string[] GetAllAssetPacks(object assetPack);
        string[] GetAllScenePaths(object assetPack);
        object LoadByAssetPack(string assetPath, Type assetType, object assetPack);
        object LoadByResources(string assetPath, Type assetType);
        object LoadBySimulates(string assetPath, Type assetType);
    }
}