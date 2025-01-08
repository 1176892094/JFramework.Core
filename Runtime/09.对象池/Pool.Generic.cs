// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:40
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
        [Serializable]
        private class EntityPool : IPool<GameObject>
        {
            private readonly HashSet<GameObject> cached = new HashSet<GameObject>();
            private readonly Queue<GameObject> unused = new Queue<GameObject>();

            public EntityPool(string assetPath, Type assetType)
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

            public async Task<GameObject> Dequeue()
            {
                dequeueCount++;
                GameObject assetData;
                if (unused.Count > 0)
                {
                    assetData = unused.Dequeue();
                }
                else
                {
                    assetData = await Asset.Load<GameObject>(assetPath);
                    assetData.name = Text.Format("{0}", assetPath);
                    Object.DontDestroyOnLoad(assetData);
                }
                
                cached.Add(assetData);
                return assetData;
            }

            public bool Enqueue(GameObject assetData)
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