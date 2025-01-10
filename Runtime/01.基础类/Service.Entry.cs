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
    public partial class GlobalManager
    {
        public static class Entry
        {
            public static void Register(Helper helper)
            {
                GlobalManager.helper = helper;
                JsonManager.Load(setting, nameof(AudioSetting));
                manager = new GameObject("PoolManager");
                musicSource = manager.AddComponent<AudioSource>();
                DontDestroyOnLoad(manager);
            }

            public static void Update()
            {
                TimerManager.Update(Time.time, Time.unscaledTime);
            }

            public static void UnRegister()
            {
                helper = null;
                PackManager.Dispose();
                Data.Dispose();
                AudioManager.Dispose();
                AssetManager.Dispose();
                AgentManager.Dispose();
                UIManager.Dispose();
                TimerManager.Dispose();
                PoolManager.Dispose();
                Utility.Pool.Dispose();
                Utility.Event.Dispose();
            }
        }
    }
}