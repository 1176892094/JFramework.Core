// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:17
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;

namespace JFramework
{
    public partial class Service
    {
        private static string persistentDataPath => pathHelper?.persistentDataPath;

        private static string streamingAssetsPath => pathHelper?.streamingAssetsPath;

        private static string assetPackData => Text.Format("{0}.json", pathHelper?.assetPackName);

        private static string assetPackPath => Text.Format("{0}/{1}", persistentDataPath, pathHelper?.assetPackPath);

        private static string GetScenePath(string assetName) => Text.Format("Scenes/{0}", assetName);

        private static string GetPanelPath(string assetName) => Text.Format("Prefabs/{0}", assetName);

        private static string GetTablePath(string assetName) => Text.Format("DataTable/{0}", assetName);

        private static string GetPacketPath(string fileName) => Path.Combine(assetPackPath, fileName);

        private static string GetServerPath(string fileName) => Path.Combine(pathHelper.assetRemotePath, GetPlatform(fileName));

        private static string GetClientPath(string fileName) => Path.Combine(streamingAssetsPath, GetPlatform(fileName));

        private static string GetPlatform(string fileName) => Path.Combine(pathHelper.assetPlatform, fileName);

        private static string GetJsonPath(string fileName)
        {
            var filePath = Path.Combine(streamingAssetsPath, Text.Format("{0}.json", fileName));
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(persistentDataPath, Text.Format("{0}.json", fileName));
            }

            return filePath;
        }

        internal static void Log(string message)
        {
            helper.Log(message);
        }

        internal static void Warn(string message)
        {
            helper.Warn(message);
        }

        internal static void Error(string message)
        {
            helper.Error(message);
        }

        internal static bool IsEntity(IEntity entity)
        {
            return poolHelper.IsEntity(entity);
        }

        internal static bool IsActive(IEntity entity)
        {
            return poolHelper.IsActive(entity);
        }
    }
}