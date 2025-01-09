// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 02:01:00
// # Recently: 2025-01-10 02:01:00
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public static partial class Service
    {
        private static class Model
        {
            public static T Dequeue<T>()
            {
                return LoadPool<T>(typeof(T)).Dequeue();
            }

            public static T Dequeue<T>(Type heapType)
            {
                return LoadPool<T>(heapType).Dequeue();
            }

            public static void Enqueue<T>(T heapData)
            {
                LoadPool<T>(typeof(T)).Enqueue(heapData);
            }

            public static void Enqueue<T>(T heapData, Type heapType)
            {
                LoadPool<T>(heapType).Enqueue(heapData);
            }

            private static IHeap<T> LoadPool<T>(Type heapType)
            {
                if (Service.heapData.TryGetValue(heapType, out var heapData))
                {
                    return (IHeap<T>)heapData;
                }

                heapData = new AgentPool(heapType);
                Service.heapData.Add(heapType, heapData);
                return (IHeap<T>)heapData;
            }
        }
    }
}