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

namespace JFramework
{
    public static partial class Service
    {
        public static class Pool
        {
            public static async Task<T> Show<T>(string assetPath) where T : IEntity
            {
                if (helper == null) return default;
                var assetData = await LoadPool(assetPath, typeof(T)).Dequeue();
                poolHelper.OnDequeue(assetData);
                return (T)assetData;
            }

            public static async void Show<T>(string assetPath, Action<T> assetAction) where T : IEntity
            {
                if (helper == null) return;
                var assetData = await LoadPool(assetPath, typeof(T)).Dequeue();
                poolHelper.OnDequeue(assetData);
                assetAction.Invoke((T)assetData);
            }

            public static async Task<IEntity> Show(string assetPath, Type assetType)
            {
                if (helper == null) return default;
                var assetData = await LoadPool(assetPath, assetType).Dequeue();
                poolHelper.OnDequeue(assetData);
                return assetData;
            }

            public static async void Show(string assetPath, Type assetType, Action<IEntity> assetAction)
            {
                if (helper == null) return;
                var assetData = await LoadPool(assetPath, assetType).Dequeue();
                poolHelper.OnDequeue(assetData);
                assetAction.Invoke(assetData);
            }

            public static bool Hide(IEntity assetData)
            {
                if (helper == null) return false;
                var assetPath = poolHelper.OnEnqueue(assetData);
                return LoadPool(assetPath, typeof(IEntity)).Enqueue(assetData);
            }

            private static IPool<IEntity> LoadPool(string assetPath, Type assetType)
            {
                if (Service.poolData.TryGetValue(assetPath, out var poolData))
                {
                    return (IPool<IEntity>)poolData;
                }

                poolData = new Pool<IEntity>(assetPath, assetType);
                Service.poolData.Add(assetPath, poolData);
                return (IPool<IEntity>)poolData;
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