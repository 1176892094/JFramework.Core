// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:26
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public interface IPathHelper : IBaseHelper
    {
        bool assetPackMode { get; }
        string assetPlatform { get; }
        string assetPackPath { get; }
        string assetPackName { get; }
        string assetRemotePath { get; }
        string writeAssembly { get; }
        string writeTablePath { get; }
        string writeStructPath { get; }
        string writeEnumPath { get; }
        string readAssembly { get; }
        string readTableText { get; }
        string readStructText { get; }
        string readEnumText { get; }
        string streamingAssetsPath { get; }
        string persistentDataPath { get; }
    }
}