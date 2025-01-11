// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 18:01:53
// # Recently: 2025-01-11 18:01:31
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Common
{
    public interface IPool : IDisposable
    {
        public Type assetType { get; }
        public string assetPath { get; }
        public int caches { get; }
        public int unuseds { get; }
        public int dequeue { get; }
        public int enqueue { get; }
    }
}