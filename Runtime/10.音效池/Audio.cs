// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public static partial class Service
    {
        public static class Audio
        {
            public static float musicValue
            {
                get => setting.musicValue;
                set
                {
                    setting.musicValue = value;
                    audioHelper.MusicVolume(value);
                    Json.Save(setting, nameof(AudioSetting));
                }
            }

            public static float audioValue
            {
                get => setting.audioValue;
                set
                {
                    setting.audioValue = value;
                    audioHelper.AudioVolume(value);
                    Json.Save(setting, nameof(AudioSetting));
                }
            }

            public static async void PlayMain<T>(string assetPath, Action<T> assetAction = null)
            {
                if (helper == null) return;
                var assetData = LoadPool<T>(assetPath, typeof(T)).Dequeue();
                await audioHelper.OnDequeue(assetPath, assetData, 0);
                assetAction?.Invoke(assetData);
            }

            public static async void PlayLoop<T>(string assetPath, Action<T> assetAction = null)
            {
                if (helper == null) return;
                var assetData = LoadPool<T>(assetPath, typeof(T)).Dequeue();
                await audioHelper.OnDequeue(assetPath, assetData, 1);
                assetAction?.Invoke(assetData);
            }

            public static async void PlayOnce<T>(string assetPath, Action<T> assetAction = null)
            {
                if (helper == null) return;
                var assetData = LoadPool<T>(assetPath, typeof(T)).Dequeue();
                await audioHelper.OnDequeue(assetPath, assetData, 2);
                assetAction?.Invoke(assetData);
            }

            public static void Stop<T>(T assetData)
            {
                if (helper == null) return;
                var assetPath = audioHelper.OnEnqueue(assetData, false);
                LoadPool<T>(assetPath, typeof(T)).Enqueue(assetData);
            }

            public static void Pause<T>(T assetData)
            {
                if (helper == null) return;
                var assetPath = audioHelper.OnEnqueue(assetData, true);
                LoadPool<T>(assetPath, typeof(T)).Enqueue(assetData);
            }

            private static IHeap<T> LoadPool<T>(string assetPath, Type assetType)
            {
                if (Service.poolData.TryGetValue(assetPath, out var poolData))
                {
                    return (IHeap<T>)poolData;
                }

                poolData = new Audio<T>(assetPath, assetType);
                Service.poolData.Add(assetPath, poolData);
                return (IHeap<T>)poolData;
            }
        }
    }
}