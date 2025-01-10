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
        private static string assemblyName => Path.GetFileNameWithoutExtension(assemblyPath);

        private static string assetPath => helper.assetDataPath + "/{0}DataTable.asset";

        private static string assemblyPath => Utility.Text.Format("{0}/{1}.asmdef", helper.scriptDataPath, helper.assemblyName);

        private static string enumPath => helper.scriptDataPath + "/01.枚举类/{0}.cs";

        private static string structPath => helper.scriptDataPath + "/02.结构体/{0}.cs";

        private static string tablePath => helper.scriptDataPath + "/03.数据表/{0}.cs";

        private static string assemblyData => Resources.LoadAll<TextAsset>("GlobalSetting")[0].text;

        private static string enumData => Resources.LoadAll<TextAsset>("GlobalSetting")[1].text;

        private static string structData => Resources.LoadAll<TextAsset>("GlobalSetting")[2].text;

        private static string tableData => Resources.LoadAll<TextAsset>("GlobalSetting")[3].text;

        private static string persistentDataPath => Application.persistentDataPath;

        private static string streamingAssetsPath => Application.streamingAssetsPath;

        private static string assetPackData => Utility.Text.Format("{0}.json", helper.assetPackName);

        private static string assetPackPath => Utility.Text.Format("{0}/{1}", persistentDataPath, helper.assetPackPath);

        private static string GetScenePath(string assetName) => Utility.Text.Format("Scenes/{0}", assetName);

        private static string GetPanelPath(string assetName) => Utility.Text.Format("Prefabs/{0}", assetName);

        private static string GetTablePath(string assetName) => Utility.Text.Format("DataTable/{0}", assetName);

        private static string GetPacketPath(string fileName) => Path.Combine(assetPackPath, fileName);

        private static string GetServerPath(string fileName) => Path.Combine(helper.assetRemotePath, GetPlatform(fileName));

        private static string GetClientPath(string fileName) => Path.Combine(streamingAssetsPath, GetPlatform(fileName));

        private static string GetPlatform(string fileName) => Path.Combine(helper.assetPlatform, fileName);

        private static string GetJsonPath(string fileName)
        {
            var filePath = Path.Combine(streamingAssetsPath, Utility.Text.Format("{0}.json", fileName));
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(persistentDataPath, Utility.Text.Format("{0}.json", fileName));
            }

            return filePath;
        }
    }
}