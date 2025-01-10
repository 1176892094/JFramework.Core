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
    public partial class GlobalManager
    {
        internal static string assemblyName => Path.GetFileNameWithoutExtension(assemblyPath);

        internal static string assetPath => helper.assetDataPath + "/{0}DataTable.asset";

        internal static string assemblyPath => Utility.Text.Format("{0}/{1}.asmdef", helper.scriptDataPath, helper.assemblyName);

        internal static string enumPath => helper.scriptDataPath + "/01.枚举类/{0}.cs";

        internal static string structPath => helper.scriptDataPath + "/02.结构体/{0}.cs";

        internal static string tablePath => helper.scriptDataPath + "/03.数据表/{0}.cs";

        internal static string assemblyData => Resources.LoadAll<TextAsset>("GlobalSetting")[0].text;

        internal static string enumData => Resources.LoadAll<TextAsset>("GlobalSetting")[1].text;

        internal static string structData => Resources.LoadAll<TextAsset>("GlobalSetting")[2].text;

        internal static string tableData => Resources.LoadAll<TextAsset>("GlobalSetting")[3].text;

        private static string persistentDataPath => Application.persistentDataPath;

        private static string streamingAssetsPath => Application.streamingAssetsPath;

        internal static string assetPackData => Utility.Text.Format("{0}.json", helper.assetPackName);

        internal static string assetPackPath => Utility.Text.Format("{0}/{1}", persistentDataPath, helper.assetPackPath);

        internal static string GetScenePath(string assetName) => Utility.Text.Format("Scenes/{0}", assetName);

        internal static string GetPanelPath(string assetName) => Utility.Text.Format("Prefabs/{0}", assetName);

        internal static string GetTablePath(string assetName) => Utility.Text.Format("DataTable/{0}", assetName);

        internal static string GetPacketPath(string fileName) => Path.Combine(assetPackPath, fileName);

        internal static string GetServerPath(string fileName) => Path.Combine(helper.assetRemotePath, GetPlatform(fileName));

        internal static string GetClientPath(string fileName) => Path.Combine(streamingAssetsPath, GetPlatform(fileName));

        private static string GetPlatform(string fileName) => Path.Combine(helper.assetPlatform, fileName);
    }
}