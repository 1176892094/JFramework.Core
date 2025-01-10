// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:25
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Common;

namespace JFramework
{
    public static partial class Service
    {
        public static class Event
        {
            private static readonly Dictionary<Type, IPool> poolData = new Dictionary<Type, IPool>();

            public static void Listen<T>(IEvent<T> objectData) where T : struct, IEvent
            {
                LoadPool<T>().Listen(objectData);
            }

            public static void Remove<T>(IEvent<T> objectData) where T : struct, IEvent
            {
                LoadPool<T>().Remove(objectData);
            }

            public static void Invoke<T>(T objectData) where T : struct, IEvent
            {
                LoadPool<T>().Invoke(objectData);
            }

            private static Event<T> LoadPool<T>() where T : struct, IEvent
            {
                if (poolData.TryGetValue(typeof(T), out var pool))
                {
                    return (Event<T>)pool;
                }

                pool = new Event<T>(typeof(T));
                poolData.Add(typeof(T), pool);
                return (Event<T>)pool;
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

            public static void Dispose()
            {
                var poolCaches = new List<Type>(poolData.Keys);
                foreach (var cache in poolCaches)
                {
                    if (poolData.TryGetValue(cache, out var pool))
                    {
                        pool.Dispose();
                        poolData.Remove(cache);
                    }
                }

                poolData.Clear();
            }
        }
    }
}