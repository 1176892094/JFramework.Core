// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 16:12:03
// # Recently: 2024-12-22 20:12:14
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
        private class Audio<T> : IHeap<T>
        {
            private readonly HashSet<T> cached = new HashSet<T>();
            private readonly Queue<T> unused = new Queue<T>();

            public Audio(string assetPath, Type assetType)
            {
                this.assetPath = assetPath;
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
                dequeueCount++;
                T assetData;
                if (unused.Count > 0)
                {
                    assetData = unused.Dequeue();
                }
                else
                {
                    assetData = (T)audioHelper.Instantiate(assetPath);
                }

                cached.Add(assetData);
                return assetData;
            }

            public void Enqueue(T assetData)
            {
                if (cached.Remove(assetData))
                {
                    enqueueCount++;
                    unused.Enqueue(assetData);
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