// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using UnityEngine;

namespace JFramework
{
    public partial class Service
    {
        private static string persistentDataPath => Application.persistentDataPath;

        private static string streamingAssetsPath => Application.streamingAssetsPath;

        private static string assetPackData => Text.Format("{0}.json", assetHelper.assetPackName);

        private static string assetPackPath => Text.Format("{0}/{1}", persistentDataPath, assetHelper.assetPackPath);

        private static string dataAssembly => Path.GetFileNameWithoutExtension(formHelper.Path("Assembly", FileAccess.Write));

        private static string GetScenePath(string assetName) => Text.Format("Scenes/{0}", assetName);

        private static string GetPanelPath(string assetName) => Text.Format("Prefabs/{0}", assetName);

        private static string GetTablePath(string assetName) => Text.Format("DataTable/{0}", assetName);

        private static string GetPacketPath(string fileName) => Path.Combine(assetPackPath, fileName);

        private static string GetServerPath(string fileName) => Path.Combine(assetHelper.assetRemotePath, GetPlatform(fileName));

        private static string GetClientPath(string fileName) => Path.Combine(streamingAssetsPath, GetPlatform(fileName));

        private static string GetPlatform(string fileName) => Path.Combine(assetHelper.assetPlatform, fileName);

        private static string GetJsonPath(string fileName)
        {
            var filePath = Path.Combine(streamingAssetsPath, Text.Format("{0}.json", fileName));
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(persistentDataPath, Text.Format("{0}.json", fileName));
            }

            return filePath;
        }
    }
}