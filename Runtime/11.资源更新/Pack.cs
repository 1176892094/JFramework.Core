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
using UnityEngine;
using UnityEngine.Networking;

namespace JFramework
{
    public static partial class Service
    {
        public static class Pack
        {
            public static async void LoadAssetData()
            {
                if (helper == null) return;
                if (!helper.assetPackMode)
                {
                    Event.Invoke(new PackCompleteEvent(false, "启动本地资源加载。"));
                    return;
                }

                if (helper.assetPackMode && !Directory.Exists(assetPackPath))
                {
                    Directory.CreateDirectory(assetPackPath);
                }

                var fileUri = GetServerPath(assetPackData);
                var serverRequest = await LoadServerRequest(assetPackData, fileUri);
                if (!string.IsNullOrEmpty(serverRequest))
                {
                    var assetPacks = Json.FromJson<List<PackData>>(serverRequest);
                    foreach (var assetPack in assetPacks)
                    {
                        serverPacks.Add(assetPack.name, assetPack);
                    }

                    var sizes = new int[assetPacks.Count];
                    for (var i = 0; i < sizes.Length; i++)
                    {
                        sizes[i] = assetPacks[i].size;
                    }

                    Event.Invoke(new PackAwakeEvent(sizes));
                }
                else
                {
                    Event.Invoke(new PackCompleteEvent(false, "没有连接到服务器!"));
                    return;
                }

                var persistentData = GetPacketPath(assetPackData);
                var streamingAssets = GetClientPath(assetPackData);
                var clientRequest = await LoadClientRequest(persistentData, streamingAssets);
                if (!string.IsNullOrEmpty(clientRequest))
                {
                    var assetPacks = Json.FromJson<List<PackData>>(clientRequest);
                    foreach (var assetPack in assetPacks)
                    {
                        clientPacks.Add(assetPack.name, assetPack);
                    }
                }

                var fileNames = new HashSet<string>();
                foreach (var fileName in serverPacks.Keys)
                {
                    if (clientPacks.TryGetValue(fileName, out var assetPack))
                    {
                        if (assetPack != serverPacks[fileName])
                        {
                            fileNames.Add(fileName);
                        }

                        clientPacks.Remove(fileName);
                    }
                    else
                    {
                        fileNames.Add(fileName);
                    }
                }

                foreach (var clientPack in clientPacks.Keys)
                {
                    var filePath = GetPacketPath(clientPack);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                var status = await LoadPacketRequest(fileNames);
                if (status)
                {
                    var filePath = GetPacketPath(assetPackData);
                    File.WriteAllText(filePath, serverRequest);
                }

                Event.Invoke(new PackCompleteEvent(status, status ? "更新完成!" : "更新失败!"));
            }

            private static async Task<bool> LoadPacketRequest(HashSet<string> fileNames)
            {
                var packNames = new HashSet<string>(fileNames);
                for (var i = 0; i < 5; i++)
                {
                    foreach (var packName in packNames)
                    {
                        var packUri = GetServerPath(packName);
                        var packData = await LoadPacketRequest(packName, packUri);
                        var packPath = GetPacketPath(packName);
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
                            Log.Info(Utility.Text.Format("请求服务器下载 {0} 失败!\n", packName));
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
                        Log.Info(Utility.Text.Format("请求服务器校验 {0} 失败!\n", packName));
                        return null;
                    }
                }

                using (var request = UnityWebRequest.Get(packUri))
                {
                    var result = request.SendWebRequest();
                    while (!result.isDone && helper != null)
                    {
                        Event.Invoke(new PackUpdateEvent(packName, request.downloadProgress));
                        await Task.Yield();
                    }

                    Event.Invoke(new PackUpdateEvent(packName, 1));
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Log.Info(Utility.Text.Format("请求服务器下载 {0} 失败!\n", packName));
                        return null;
                    }

                    return request.downloadHandler.data;
                }
            }

            private static async Task<string> LoadClientRequest(string persistentData, string streamingAssets)
            {
                var packData = await helper.LoadRequest(persistentData, streamingAssets);
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
                var packData = await helper.LoadRequest(persistentData, streamingAssets);
                byte[] result = default;
                if (packData.Key == 1)
                {
                    result = await Task.Run(() => Utility.Xor.Decrypt(File.ReadAllBytes(packData.Value)));
                }
                else if (packData.Key == 2)
                {
                    using var request = UnityWebRequest.Get(packData.Value);
                    await request.SendWebRequest();
                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        result = await Task.Run(() => Utility.Xor.Decrypt(request.downloadHandler.data));
                    }
                }

                return helper != null ? AssetBundle.LoadFromMemory(result) : null;
            }

            internal static void Dispose()
            {
                clientPacks.Clear();
                serverPacks.Clear();
            }
        }
    }
}