// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:38
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
            public static void Register(IBaseHelper helper)
            {
                if (manager == null)
                {
                    manager = new GameObject("PoolManager");
                    Object.DontDestroyOnLoad(manager);
                }

                if (musicSource == null)
                {
                    musicSource = manager.AddComponent<AudioSource>();
                }

                Service.helper = helper;
                Json.Load(setting, nameof(AudioSetting));
            }

            public static void Update(float elapsedTime, float unscaleTime)
            {
                Timer.Update(elapsedTime, unscaleTime);
            }

            public static void UnRegister()
            {
                helper = null;
                Heap.Dispose();
                Pack.Dispose();
                Data.Dispose();
                Pool.Dispose();
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