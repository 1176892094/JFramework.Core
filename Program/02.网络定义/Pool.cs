// *********************************************************************************
// # Project: JFramework.Lobby
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-08-28 20:08:49
// # Recently: 2024-12-23 00:12:04
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;

namespace JFramework.Net
{
    internal static class Pool<T> where T : new()
    {
        private static readonly Queue<T> unused = new Queue<T>();
        private static readonly HashSet<T> cached = new HashSet<T>();

        public static T Dequeue()
        {
            var assetData = unused.Count > 0 ? unused.Dequeue() : new T();
            cached.Add(assetData);
            return assetData;
        }

        public static void Enqueue(T assetData)
        {
            if (cached.Remove(assetData))
            {
                unused.Enqueue(assetData);
            }
        }
    }
}