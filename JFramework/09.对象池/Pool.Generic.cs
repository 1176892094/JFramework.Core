// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 16:12:39
// # Recently: 2024-12-22 20:12:28
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
        [Serializable]
        private class Pool<T> : IPool<T>
        {
            private readonly HashSet<T> cached = new HashSet<T>();
            private readonly Queue<T> unused = new Queue<T>();

            public Pool(string assetPath, Type assetType)
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

            public async Task<T> Dequeue()
            {
                dequeueCount++;
                T assetData;
                if (unused.Count > 0)
                {
                    assetData = unused.Dequeue();
                }
                else
                {
                    assetData = (T)await poolHelper.Instantiate(assetPath, assetType);
                }

                cached.Add(assetData);
                return assetData;
            }

            public bool Enqueue(T assetData)
            {
                if (cached.Remove(assetData))
                {
                    enqueueCount++;
                    unused.Enqueue(assetData);
                    return true;
                }

                return false;
            }

            void IDisposable.Dispose()
            {
                cached.Clear();
                unused.Clear();
            }
        }
    }
}