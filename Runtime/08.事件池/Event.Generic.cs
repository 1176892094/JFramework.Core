// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:29
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework
{
    public static partial class Service
    {
        [Serializable]
        private class Event<T> : IPool where T : struct, IEvent
        {
            private readonly HashSet<IEvent<T>> cached = new HashSet<IEvent<T>>();

            public Event(Type assetType)
            {
                this.assetType = assetType;
            }

            public Type assetType { get; private set; }
            public string assetPath { get; private set; }
            public int cachedCount => cached.Count;
            public int unusedCount { get; private set; }
            public int dequeueCount { get; private set; }
            public int enqueueCount { get; private set; }

            void IDisposable.Dispose()
            {
                cached.Clear();
                OnExecute = null;
            }

            private event Action<T> OnExecute;

            public void Listen(IEvent<T> @object)
            {
                dequeueCount++;
                if (cached.Add(@object))
                {
                    OnExecute += @object.Execute;
                }
            }

            public void Remove(IEvent<T> @object)
            {
                enqueueCount++;
                if (cached.Remove(@object))
                {
                    OnExecute -= @object.Execute;
                }
            }

            public void Invoke(T message)
            {
                unusedCount++;
                OnExecute?.Invoke(message);
            }
        }
    }
}