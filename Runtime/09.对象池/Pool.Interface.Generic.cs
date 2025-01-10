// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;

namespace JFramework
{
    public static partial class Service
    {
        private interface IPool<T> : IPool
        {
            Task<T> Dequeue();

            bool Enqueue(T assetData);
        }
        
        private interface IHeap<T> : IPool
        {
            T Dequeue();

            void Enqueue(T assetData);
        }
        
        private interface IPool : IDisposable
        {
            public Type assetType { get; }
            public string assetPath { get; }
            public int cachedCount { get; }
            public int unusedCount { get; }
            public int dequeueCount { get; }
            public int enqueueCount { get; }
        }
    }
}