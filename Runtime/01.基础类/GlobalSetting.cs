// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-11 00:01:01
// # Recently: 2025-01-11 00:01:01
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public abstract class GlobalSetting : ScriptableObject
    {
        public static GlobalSetting Runtime;

        public AssetPlatform assetPlatform = AssetPlatform.StandaloneWindows;

        [SerializeField] protected string smtpServer = "smtp.qq.com";

        [SerializeField] protected int smtpPort = 587;

        [SerializeField] protected string smtpUsername = "1176892094@qq.com";

        [SerializeField] protected string smtpPassword;

        public AssetMode assetPackMode = AssetMode.Simulate;

        [SerializeField] protected string assetPackName = "AssetPacket";

        [SerializeField] protected string assetBuildPath = "Assets/StreamingAssets";

        [SerializeField] protected string assetRemotePath = "http://192.168.0.3:8000/AssetPackets";

        [SerializeField] protected string assetAssembly = "HotUpdate.Data";

        protected abstract string scriptDataPath { get; }

        protected abstract string assetDataPath { get; }

        internal static string platformPath => Runtime.assetPlatform.ToString();

        internal static bool assetLoadMode => Runtime.assetPackMode == AssetMode.Authentic;

        internal static string assetPath => Runtime.assetDataPath + "/{0}DataTable.asset";

        internal static string assemblyName => Path.GetFileNameWithoutExtension(assemblyPath);

        internal static string assemblyPath => Utility.Text.Format("{0}/{1}.asmdef", Runtime.scriptDataPath, Runtime.assetAssembly);

        internal static string enumPath => Runtime.scriptDataPath + "/01.枚举类/{0}.cs";

        internal static string structPath => Runtime.scriptDataPath + "/02.结构体/{0}.cs";

        internal static string tablePath => Runtime.scriptDataPath + "/03.数据表/{0}.cs";

        internal static string assemblyData => Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[0].text;

        internal static string enumData => Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[1].text;

        internal static string structData => Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[2].text;

        internal static string tableData => Resources.LoadAll<TextAsset>(nameof(GlobalSetting))[3].text;

        internal static string assetPackData => Utility.Text.Format("{0}.json", Runtime.assetPackName);

        internal static string assetPackPath => Utility.Text.Format("{0}/{1}", Application.persistentDataPath, Runtime.assetBuildPath);

        internal static string GetScenePath(string assetName) => Utility.Text.Format("Scenes/{0}", assetName);

        internal static string GetPanelPath(string assetName) => Utility.Text.Format("Prefabs/{0}", assetName);

        internal static string GetTablePath(string assetName) => Utility.Text.Format("DataTable/{0}", assetName);

        internal static string GetPlatform(string fileName) => Path.Combine(platformPath, fileName);

        internal static string GetPacketPath(string fileName) => Path.Combine(Runtime.assetBuildPath, fileName);

        internal static string GetServerPath(string fileName) => Path.Combine(Runtime.assetRemotePath, GetPlatform(fileName));

        internal static string GetClientPath(string fileName) => Path.Combine(Application.streamingAssetsPath, GetPlatform(fileName));

        public abstract void MulticastLock(bool multicast);

        public abstract void CreateAsset(Object assetData, string assetPath);

        public abstract void CreateProgress(string assetPath, float progress);

        public abstract Object LoadByAssetPack(string assetPath, Type assetType, AssetBundle assetPack);

        public abstract Object LoadByResources(string assetPath, Type assetType);

        public abstract Object LoadBySimulates(string assetPath, Type assetType);

        public abstract Task<KeyValuePair<int, string>> LoadRequest(string persistentData, string streamingAssets);

        public Utility.MailData SendMail(string mailBody)
        {
            return new Utility.MailData
            {
                smtpServer = smtpServer,
                smtpPort = smtpPort,
                senderName = "JFramework",
                senderAddress = smtpUsername,
                senderPassword = smtpPassword,
                targetAddress = smtpUsername,
                mailName = "来自《JFramework》的调试日志:",
                mailBody = mailBody
            };
        }

        public enum AssetMode : byte
        {
            Simulate,
            Authentic
        }

        public enum AssetPlatform : byte
        {
            StandaloneOSX = 2,
            StandaloneWindows = 5,
            IOS = 9,
            Android = 13,
            WebGL = 20
        }
    }
}