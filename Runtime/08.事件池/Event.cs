// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 21:12:35
// # Recently: 2024-12-22 20:12:26
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Linq;

namespace JFramework
{
    public static partial class Service
    {
        public static class Event
        {
            public static void Listen<T>(IEvent<T> objectData) where T : struct, IEvent
            {
                if (helper == null) return;
                LoadPool<T>().Listen(objectData);
            }

            public static void Remove<T>(IEvent<T> objectData) where T : struct, IEvent
            {
                if (helper == null) return;
                LoadPool<T>().Remove(objectData);
            }

            public static void Invoke<T>(T objectData) where T : struct, IEvent
            {
                if (helper == null) return;
                LoadPool<T>().Invoke(objectData);
            }

            private static Event<T> LoadPool<T>() where T : struct, IEvent
            {
                if (Service.eventData.TryGetValue(typeof(T), out var eventData))
                {
                    return (Event<T>)eventData;
                }

                eventData = new Event<T>(typeof(T));
                Service.eventData.Add(typeof(T), eventData);
                return (Event<T>)eventData;
            }

            public static Reference[] Reference()
            {
                var index = 0;
                var results = new Reference[eventData.Count];
                foreach (var eventData in eventData)
                {
                    var key = eventData.Key;
                    var value = eventData.Value;
                    results[index++] = new Reference(key, value.cachedCount, value.unusedCount, value.dequeueCount, value.enqueueCount);
                }

                return results;
            }

            internal static void Dispose()
            {
                var eventCaches = eventData.Keys.ToList();
                foreach (var cache in eventCaches)
                {
                    if (Service.eventData.TryGetValue(cache, out var eventData))
                    {
                        eventData.Dispose();
                        Service.eventData.Remove(cache);
                    }
                }

                eventData.Clear();
            }
        }
    }
}