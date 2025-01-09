// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:37
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    public static partial class Service
    {
        public static class Entry
        {
            public static void Register(Helper helper)
            {
                Service.helper = helper;
                Json.Load(setting, nameof(AudioSetting));
                manager = new GameObject("PoolManager");
                musicSource = manager.AddComponent<AudioSource>();
                Object.DontDestroyOnLoad(manager);
            }

            public static void Update()
            {
                Timer.Update(Time.time, Time.unscaledTime);
            }

            public static void UnRegister()
            {
                helper = null;
                Heap.Dispose();
                Pack.Dispose();
                Data.Dispose();
                Pool.Dispose();
                Audio.Dispose();
                Group.Dispose();
                Event.Dispose();
                Asset.Dispose();
                Agent.Dispose();
                Panel.Dispose();
                Timer.Dispose();
            }
        }
    }
}