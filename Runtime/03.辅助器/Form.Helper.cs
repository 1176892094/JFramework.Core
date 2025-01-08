// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-24 03:12:32
// # Recently: 2025-01-08 17:01:31
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;

namespace JFramework
{
    public interface IFormHelper : IBaseHelper
    {
        void CreateAsset(IDataTable assetData, string assetPath);

        void CreateProgress(string assetPath, float progress);

        string Path(string objectText, FileAccess fileAccess);
    }
}