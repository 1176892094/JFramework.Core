// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:37
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public static partial class Service
    {
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