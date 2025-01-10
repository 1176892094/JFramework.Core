// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:34
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JFramework.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace JFramework
{
    internal static class PackManager
    {
        public static async void LoadAssetData()
        {
            if (!GlobalManager.Instance) return;
            if (!GlobalSetting.assetLoadMode)
            {
                Service.Event.Invoke(new PackCompleteEvent(false, "启动本地资源加载。"));
                return;
            }

            if (GlobalSetting.assetLoadMode && !Directory.Exists(GlobalSetting.assetPackPath))
            {
                Directory.CreateDirectory(GlobalSetting.assetPackPath);
            }

            var fileUri = GlobalSetting.GetServerPath(GlobalSetting.assetPackData);
            var serverRequest = await LoadServerRequest(GlobalSetting.assetPackData, fileUri);
            if (!string.IsNullOrEmpty(serverRequest))
            {
                var assetPacks = JsonManager.FromJson<List<PackData>>(serverRequest);
                foreach (var assetPack in assetPacks)
                {
                    GlobalManager.serverPacks.Add(assetPack.name, assetPack);
                }

                var sizes = new int[assetPacks.Count];
                for (var i = 0; i < sizes.Length; i++)
                {
                    sizes[i] = assetPacks[i].size;
                }

                Service.Event.Invoke(new PackAwakeEvent(sizes));
            }
            else
            {
                Service.Event.Invoke(new PackCompleteEvent(false, "没有连接到服务器!"));
                return;
            }

            var persistentData = GlobalSetting.GetPacketPath(GlobalSetting.assetPackData);
            var streamingAssets = GlobalSetting.GetClientPath(GlobalSetting.assetPackData);
            var clientRequest = await LoadClientRequest(persistentData, streamingAssets);
            if (!string.IsNullOrEmpty(clientRequest))
            {
                var assetPacks = JsonManager.FromJson<List<PackData>>(clientRequest);
                foreach (var assetPack in assetPacks)
                {
                    GlobalManager.clientPacks.Add(assetPack.name, assetPack);
                }
            }

            var fileNames = new HashSet<string>();
            foreach (var fileName in GlobalManager.serverPacks.Keys)
            {
                if (GlobalManager.clientPacks.TryGetValue(fileName, out var assetPack))
                {
                    if (assetPack != GlobalManager.serverPacks[fileName])
                    {
                        fileNames.Add(fileName);
                    }

                    GlobalManager.clientPacks.Remove(fileName);
                }
                else
                {
                    fileNames.Add(fileName);
                }
            }

            foreach (var clientPack in GlobalManager.clientPacks.Keys)
            {
                var filePath = GlobalSetting.GetPacketPath(clientPack);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            var status = await LoadPacketRequest(fileNames);
            if (status)
            {
                var filePath = GlobalSetting.GetPacketPath(GlobalSetting.assetPackData);
                File.WriteAllText(filePath, serverRequest);
            }

            Service.Event.Invoke(new PackCompleteEvent(status, status ? "更新完成!" : "更新失败!"));
        }

        private static async Task<bool> LoadPacketRequest(HashSet<string> fileNames)
        {
            var packNames = new HashSet<string>(fileNames);
            for (var i = 0; i < 5; i++)
            {
                foreach (var packName in packNames)
                {
                    var packUri = GlobalSetting.GetServerPath(packName);
                    var packData = await LoadPacketRequest(packName, packUri);
                    var packPath = GlobalSetting.GetPacketPath(packName);
                    await Task.Run(() => File.WriteAllBytes(packPath, packData));
                    if (fileNames.Contains(packName))
                    {
                        fileNames.Remove(packName);
                    }
                }

                if (fileNames.Count == 0)
                {
                    break;
                }
            }

            return fileNames.Count == 0;
        }

        private static async Task<string> LoadServerRequest(string packName, string packUri)
        {
            for (var i = 0; i < 5; i++)
            {
                using (var request = UnityWebRequest.Head(packUri))
                {
                    request.timeout = 1;
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        continue;
                    }
                }

                using (var request = UnityWebRequest.Get(packUri))
                {
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(Service.Text.Format("请求服务器下载 {0} 失败!\n", packName));
                        continue;
                    }

                    return request.downloadHandler.text;
                }
            }

            return null;
        }

        private static async Task<byte[]> LoadPacketRequest(string packName, string packUri)
        {
            using (var request = UnityWebRequest.Head(packUri))
            {
                request.timeout = 1;
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(Service.Text.Format("请求服务器校验 {0} 失败!\n", packName));
                    return null;
                }
            }

            using (var request = UnityWebRequest.Get(packUri))
            {
                var result = request.SendWebRequest();
                while (!result.isDone && GlobalSetting.Instance != null)
                {
                    Service.Event.Invoke(new PackUpdateEvent(packName, request.downloadProgress));
                    await Task.Yield();
                }

                Service.Event.Invoke(new PackUpdateEvent(packName, 1));
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(Service.Text.Format("请求服务器下载 {0} 失败!\n", packName));
                    return null;
                }

                return request.downloadHandler.data;
            }
        }

        private static async Task<string> LoadClientRequest(string persistentData, string streamingAssets)
        {
            var packData = await GlobalSetting.Instance.LoadRequest(persistentData, streamingAssets);
            string result = default;
            if (packData.Key == 1)
            {
                result = File.ReadAllText(packData.Value);
            }
            else if (packData.Key == 2)
            {
                using var request = UnityWebRequest.Get(packData.Value);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    result = request.downloadHandler.text;
                }
            }

            return result;
        }

        internal static async Task<AssetBundle> LoadAssetRequest(string persistentData, string streamingAssets)
        {
            var packData = await GlobalSetting.Instance.LoadRequest(persistentData, streamingAssets);
            byte[] result = default;
            if (packData.Key == 1)
            {
                result = await Task.Run(() => Service.Xor.Decrypt(File.ReadAllBytes(packData.Value)));
            }
            else if (packData.Key == 2)
            {
                using var request = UnityWebRequest.Get(packData.Value);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    result = await Task.Run(() => Service.Xor.Decrypt(request.downloadHandler.data));
                }
            }

            return GlobalSetting.Instance != null ? AssetBundle.LoadFromMemory(result) : null;
        }

        internal static void Dispose()
        {
            GlobalManager.clientPacks.Clear();
            GlobalManager.serverPacks.Clear();
        }
    }
}