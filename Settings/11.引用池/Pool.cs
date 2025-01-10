// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:35
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework
{
    public static partial class Utility
    {
        public static class Pool
        {
            private static readonly Dictionary<Type, IReference> poolData = new Dictionary<Type, IReference>();

            public static T Dequeue<T>()
            {
                return LoadPool<T>(typeof(T)).Dequeue();
            }

            public static T Dequeue<T>(Type heapType)
            {
                return LoadPool<T>(heapType).Dequeue();
            }

            public static void Enqueue<T>(T heapData)
            {
                LoadPool<T>(typeof(T)).Enqueue(heapData);
            }

            public static void Enqueue<T>(T heapData, Type heapType)
            {
                LoadPool<T>(heapType).Enqueue(heapData);
            }

            private static Pool<T> LoadPool<T>(Type heapType)
            {
                if (poolData.TryGetValue(heapType, out var pool))
                {
                    return (Pool<T>)pool;
                }

                pool = new Pool<T>(heapType);
                poolData.Add(heapType, pool);
                return (Pool<T>)pool;
            }

            public static Reference[] Reference()
            {
                var index = 0;
                var results = new Reference[poolData.Count];
                foreach (var pair in poolData)
                {
                    var key = pair.Key;
                    var value = pair.Value;
                    results[index++] = new Reference(key, value.caches, value.unuseds, value.dequeue, value.enqueue);
                }

                return results;
            }
        }
    }
}