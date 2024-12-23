// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:15
// # Recently: 2024-12-22 20:12:23
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Linq;

namespace JFramework
{
    public static partial class Service
    {
        public static class Heap
        {
            public static T Dequeue<T>() where T : class, IDisposable
            {
                return LoadPool<T>(typeof(T)).Dequeue();
            }

            public static T Dequeue<T>(Type heapType) where T : class, IDisposable
            {
                return LoadPool<T>(heapType).Dequeue();
            }

            public static void Enqueue<T>(T heapData) where T : class, IDisposable
            {
                LoadPool<T>(typeof(T)).Enqueue(heapData);
            }

            public static void Enqueue<T>(T heapData, Type heapType) where T : class, IDisposable
            {
                LoadPool<T>(heapType).Enqueue(heapData);
            }

            private static IHeap<T> LoadPool<T>(Type heapType) where T : class, IDisposable
            {
                if (Service.heapData.TryGetValue(heapType, out var heapData))
                {
                    return (IHeap<T>)heapData;
                }

                heapData = new Heap<T>(heapType);
                Service.heapData.Add(heapType, heapData);
                return (IHeap<T>)heapData;
            }

            public static Reference[] Reference()
            {
                var index = 0;
                var results = new Reference[heapData.Count];
                foreach (var heapData in heapData)
                {
                    var key = heapData.Key;
                    var value = heapData.Value;
                    results[index++] = new Reference(key, value.cachedCount, value.unusedCount, value.dequeueCount, value.enqueueCount);
                }

                return results;
            }

            internal static void Dispose()
            {
                var heapCaches = heapData.Keys.ToList();
                foreach (var cache in heapCaches)
                {
                    if (Service.heapData.TryGetValue(cache, out var heapData))
                    {
                        heapData.Dispose();
                        Service.heapData.Remove(cache);
                    }
                }

                heapData.Clear();
            }
        }
    }
}