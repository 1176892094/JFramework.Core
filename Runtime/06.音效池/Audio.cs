// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:29
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework
{
    public static class AudioManager
    {
        public static float musicValue
        {
            get => GlobalManager.setting.musicValue;
            set
            {
                GlobalManager.setting.musicValue = value;
                GlobalManager.musicSource.volume = value;
                JsonManager.Save(GlobalManager.setting, nameof(AudioSetting));
            }
        }

        public static float audioValue
        {
            get => GlobalManager.setting.audioValue;
            set
            {
                GlobalManager.setting.audioValue = value;
                foreach (var audioSource in GlobalManager.audioData)
                {
                    audioSource.volume = value;
                }

                JsonManager.Save(GlobalManager.setting, nameof(AudioSetting));
            }
        }

        public static async void PlayMain(string assetPath, Action<AudioSource> assetAction = null)
        {
            if (GlobalManager.helper == null) return;
            var musicSource = GlobalManager.musicSource;
            musicSource.transform.SetParent(null);
            musicSource.gameObject.SetActive(true);
            musicSource.clip = await AssetManager.Load<AudioClip>(assetPath);
            musicSource.loop = true;
            musicSource.volume = musicValue;
            assetAction?.Invoke(musicSource);
            musicSource.Play();
        }

        public static async void PlayLoop(string assetPath, Action<AudioSource> assetAction = null)
        {
            if (GlobalManager.helper == null) return;
            var assetData = LoadPool(assetPath).Dequeue();
            GlobalManager.audioData.Add(assetData);
            assetData.transform.SetParent(null);
            assetData.gameObject.SetActive(true);
            assetData.clip = await AssetManager.Load<AudioClip>(assetPath);
            assetData.loop = true;
            assetData.volume = audioValue;
            assetAction?.Invoke(assetData);
            assetData.Play();
        }

        public static async void PlayOnce(string assetPath, Action<AudioSource> assetAction = null)
        {
            if (GlobalManager.helper == null) return;
            var assetData = LoadPool(assetPath).Dequeue();
            GlobalManager.audioData.Add(assetData);
            assetData.transform.SetParent(null);
            assetData.gameObject.SetActive(true);
            assetData.clip = await AssetManager.Load<AudioClip>(assetPath);
            assetData.loop = false;
            assetData.volume = audioValue;
            assetAction?.Invoke(assetData);
            assetData.Play();
        }

        public static void StopMain(bool pause = true)
        {
            if (GlobalManager.helper == null) return;
            if (pause)
            {
                GlobalManager.musicSource.Pause();
            }
            else
            {
                GlobalManager.musicSource.Stop();
            }

            LoadPool(GlobalManager.musicSource.name).Enqueue(GlobalManager.musicSource);
        }

        public static void StopLoop(AudioSource assetData)
        {
            if (GlobalManager.helper == null) return;
            assetData.Stop();
            GlobalManager.audioData.Remove(assetData);
            LoadPool(assetData.name).Enqueue(assetData);
        }

        private static AudioPool LoadPool(string assetPath)
        {
            if (GlobalManager.poolData.TryGetValue(assetPath, out var poolData))
            {
                return (AudioPool)poolData;
            }

            poolData = new AudioPool(assetPath, typeof(AudioSource));
            GlobalManager.poolData.Add(assetPath, poolData);
            return (AudioPool)poolData;
        }

        internal static void Dispose()
        {
            GlobalManager.audioData.Clear();
        }
    }
}