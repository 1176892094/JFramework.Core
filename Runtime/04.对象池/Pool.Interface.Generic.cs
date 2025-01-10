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

using System.Threading.Tasks;

namespace JFramework
{
    public static partial class Service
    {
        private interface IPool<T> : IReference
        {
            Task<T> Dequeue();

            bool Enqueue(T assetData);
        }
        
        private interface IHeap<T> : IReference
        {
            T Dequeue();

            void Enqueue(T assetData);
        }
    }
}