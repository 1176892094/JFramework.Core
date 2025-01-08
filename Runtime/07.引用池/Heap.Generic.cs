// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:36
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework
{
    public static partial class Service
    {
        [Serializable]
        private class Heap<T> : IHeap<T>
        {
            private readonly HashSet<T> cached = new HashSet<T>();
            private readonly Queue<T> unused = new Queue<T>();

            public Heap(Type assetType)
            {
                this.assetType = assetType;
            }

            public Type assetType { get; private set; }
            public string assetPath { get; private set; }
            public int cachedCount => cached.Count;
            public int unusedCount => unused.Count;
            public int dequeueCount { get; private set; }
            public int enqueueCount { get; private set; }

            public T Dequeue()
            {
                T assetData;
                lock (unused)
                {
                    dequeueCount++;
                    if (unused.Count > 0)
                    {
                        assetData = unused.Dequeue();
                    }
                    else
                    {
                        assetData = (T)Activator.CreateInstance(assetType);
                    }

                    cached.Add(assetData);
                }

                return assetData;
            }

            public void Enqueue(T assetData)
            {
                lock (unused)
                {
                    if (cached.Remove(assetData))
                    {
                        enqueueCount++;
                        unused.Enqueue(assetData);
                    }
                }
            }

            void IDisposable.Dispose()
            {
                cached.Clear();
                unused.Clear();
            }
        }
    }
}