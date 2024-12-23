// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 21:12:29
// # Recently: 2024-12-22 20:12:21
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

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
    }
}