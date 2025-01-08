// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:28
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static partial class Service
    {
        [Serializable]
        private class AudioPool : IHeap<AudioSource>
        {
            private readonly HashSet<AudioSource> cached = new HashSet<AudioSource>();
            private readonly Queue<AudioSource> unused = new Queue<AudioSource>();

            public AudioPool(string assetPath, Type assetType)
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

            public AudioSource Dequeue()
            {
                dequeueCount++;
                AudioSource assetData;
                if (unused.Count > 0)
                {
                    assetData = unused.Dequeue();
                    if (assetData != null)
                    {
                        cached.Add(assetData);
                        return assetData;
                    }

                    enqueueCount++;
                    cached.Remove(assetData);
                }

                assetData = new GameObject(assetPath).AddComponent<AudioSource>();
                Object.DontDestroyOnLoad(assetData.gameObject);
                cached.Add(assetData);
                return assetData;
            }

            public void Enqueue(AudioSource assetData)
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