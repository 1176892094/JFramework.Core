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
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    public static partial class Service
    {
        public static class Audio
        {
            private static readonly HashSet<AudioSource> audioSources = new HashSet<AudioSource>();
            private static AudioSource musicSource;

            public static float musicValue
            {
                get => setting.musicValue;
                set
                {
                    setting.musicValue = value;
                    musicSource.volume = value;
                    Json.Save(setting, nameof(AudioSetting));
                }
            }

            public static float audioValue
            {
                get => setting.audioValue;
                set
                {
                    setting.audioValue = value;
                    foreach (var audioSource in audioSources)
                    {
                        audioSource.volume = value;
                    }

                    Json.Save(setting, nameof(AudioSetting));
                }
            }

            public static async void PlayMain(string assetPath, Action<AudioSource> assetAction = null)
            {
                if (helper == null) return;
                var assetData = LoadPool(assetPath).Dequeue();
                assetData.transform.SetParent(null);
                assetData.gameObject.SetActive(true);
                assetData.clip = await Asset.Load<AudioClip>(assetPath);
                assetData.loop = true;
                assetData.volume = musicValue;
                assetAction?.Invoke(assetData);
                assetData.Play();
            }

            public static async void PlayLoop(string assetPath, Action<AudioSource> assetAction = null)
            {
                if (helper == null) return;
                var assetData = LoadPool(assetPath).Dequeue();
                audioSources.Add(assetData);
                assetData.transform.SetParent(null);
                assetData.gameObject.SetActive(true);
                assetData.clip = await Asset.Load<AudioClip>(assetPath);
                assetData.loop = true;
                assetData.volume = audioValue;
                assetAction?.Invoke(assetData);
                assetData.Play();
            }

            public static async void PlayOnce(string assetPath, Action<AudioSource> assetAction = null)
            {
                if (helper == null) return;
                var assetData = LoadPool(assetPath).Dequeue();
                audioSources.Add(assetData);
                assetData.transform.SetParent(null);
                assetData.gameObject.SetActive(true);
                assetData.clip = await Asset.Load<AudioClip>(assetPath);
                assetData.loop = false;
                assetData.volume = audioValue;
                assetAction?.Invoke(assetData);
                assetData.Play();
            }

            public static void StopMain(bool pause = true)
            {
                if (helper == null) return;
                if (pause)
                {
                    musicSource.Pause();
                }
                else
                {
                    musicSource.Stop();
                }

                LoadPool(musicSource.name).Enqueue(musicSource);
            }

            public static void StopLoop(AudioSource assetData)
            {
                if (helper == null) return;
                assetData.Stop();
                audioSources.Remove(assetData);
                LoadPool(assetData.name).Enqueue(assetData);
            }

            private static IHeap<AudioSource> LoadPool(string assetPath)
            {
                if (Service.poolData.TryGetValue(assetPath, out var poolData))
                {
                    return (IHeap<AudioSource>)poolData;
                }

                poolData = new AudioPool(assetPath, typeof(AudioSource));
                Service.poolData.Add(assetPath, poolData);
                return (IHeap<AudioSource>)poolData;
            }
        }
    }
}