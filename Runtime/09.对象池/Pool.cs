// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:36
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class Service
    {
        public static class Pool
        {
            public static async Task<GameObject> Show(string assetPath)
            {
                if (helper == null) return default;
                var assetData = await LoadPool(assetPath).Dequeue();
                assetData.transform.SetParent(null);
                assetData.SetActive(true);
                return assetData;
            }

            public static async void Show(string assetPath, Action<GameObject> assetAction)
            {
                if (helper == null) return;
                var assetData = await LoadPool(assetPath).Dequeue();
                assetData.transform.SetParent(null);
                assetData.SetActive(true);
                assetAction.Invoke(assetData);
            }

            public static bool Hide(GameObject assetData)
            {
                if (helper == null) return false;
                var assetPath = assetData.name;
                if (!assetPools.TryGetValue(assetPath, out var parent))
                {
                    parent = new GameObject(Text.Format("Pool - {0}", assetPath));
                    parent.transform.SetParent(manager.transform);
                    assetPools.Add(assetPath, parent);
                }

                assetData.SetActive(false);
                assetData.transform.SetParent(parent.transform);
                return LoadPool(assetData.name).Enqueue(assetData);
            }

            private static IPool<GameObject> LoadPool(string assetPath)
            {
                if (Service.poolData.TryGetValue(assetPath, out var poolData))
                {
                    return (IPool<GameObject>)poolData;
                }

                poolData = new EntityPool(assetPath, typeof(GameObject));
                Service.poolData.Add(assetPath, poolData);
                return (IPool<GameObject>)poolData;
            }

            public static Reference[] Reference()
            {
                var index = 0;
                var results = new Reference[poolData.Count];
                foreach (var value in poolData.Values)
                {
                    var type = value.assetType;
                    var path = value.assetPath;
                    results[index++] = new Reference(type, path, value.cachedCount, value.unusedCount, value.dequeueCount,
                        value.enqueueCount);
                }

                return results;
            }

            internal static void Dispose()
            {
                var poolCaches = new List<string>(poolData.Keys);
                foreach (var cache in poolCaches)
                {
                    if (Service.poolData.TryGetValue(cache, out var poolData))
                    {
                        poolData.Dispose();
                        Service.poolData.Remove(cache);
                    }
                }

                poolData.Clear();
            }
        }
    }
}