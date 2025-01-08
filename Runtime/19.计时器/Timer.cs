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

using UnityEngine;

namespace JFramework
{
    public static partial class Service
    {
        internal static class Timer
        {
            public static void Update(float elapsedTime, float unscaleTime)
            {
                for (var i = timerData.Count - 1; i >= 0; i--)
                {
                    timerData[i].Update(elapsedTime, unscaleTime);
                }
            }

            public static T Load<T>(GameObject entity, float duration) where T : class, ITimer
            {
                if (helper == null) return default;
                var timerData = Heap.Dequeue<T>();
                timerData.Start(entity, duration, OnComplete);
                Service.timerData.Add(timerData);
                return timerData;

                void OnComplete()
                {
                    Service.timerData.Remove(timerData);
                    timerData.Dispose();
                    Heap.Enqueue(timerData, typeof(T));
                }
            }

            internal static void Dispose()
            {
                timerData.Clear();
            }
        }
    }
}