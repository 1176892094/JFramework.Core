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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework
{
    public static class PoolManager
    {
        public static async Task<GameObject> Show(string assetPath)
        {
            if (!GlobalSetting.Runtime) return default;
            var assetData = await LoadPool(assetPath).Dequeue();
            assetData.transform.SetParent(null);
            assetData.SetActive(true);
            return assetData;
        }

        public static async void Show(string assetPath, Action<GameObject> assetAction)
        {
            if (!GlobalSetting.Runtime) return;
            var assetData = await LoadPool(assetPath).Dequeue();
            assetData.transform.SetParent(null);
            assetData.SetActive(true);
            assetAction.Invoke(assetData);
        }

        public static bool Hide(GameObject assetData)
        {
            if (!GlobalSetting.Runtime) return false;
            var assetPath = assetData.name;
            if (!GlobalManager.poolGroup.TryGetValue(assetPath, out var parent))
            {
                parent = new GameObject(Service.Text.Format("Pool - {0}", assetPath));
                parent.transform.SetParent(GlobalManager.manager.transform);
                GlobalManager.poolGroup.Add(assetPath, parent);
            }

            assetData.SetActive(false);
            assetData.transform.SetParent(parent.transform);
            return LoadPool(assetData.name).Enqueue(assetData);
        }

        private static EntityPool LoadPool(string assetPath)
        {
            if (GlobalManager.poolData.TryGetValue(assetPath, out var poolData))
            {
                return (EntityPool)poolData;
            }

            poolData = new EntityPool(assetPath, typeof(GameObject));
            GlobalManager.poolData.Add(assetPath, poolData);
            return (EntityPool)poolData;
        }

        public static Reference[] Reference()
        {
            var index = 0;
            var results = new Reference[GlobalManager.poolData.Count];
            foreach (var value in GlobalManager.poolData.Values)
            {
                var type = value.assetType;
                var path = value.assetPath;
                results[index++] = new Reference(type, path, value.caches, value.unuseds, value.dequeue, value.enqueue);
            }

            return results;
        }

        internal static void Dispose()
        {
            var poolCaches = new List<string>(GlobalManager.poolData.Keys);
            foreach (var cache in poolCaches)
            {
                if (GlobalManager.poolData.TryGetValue(cache, out var poolData))
                {
                    poolData.Dispose();
                    GlobalManager.poolData.Remove(cache);
                }
            }

            GlobalManager.poolData.Clear();
            GlobalManager.poolGroup.Clear();
        }
    }
}