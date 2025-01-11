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

using System;
using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public sealed class Tween : ITimer
    {
        private float duration;
        private Action OnDispose;
        private Action<float> OnUpdated;

        private GameObject owner;
        private float progress;
        private float waitTime;

        public void Dispose()
        {
            owner = null;
            progress = 0;
            duration = 0;
            waitTime = 0;
            OnUpdated = null;
            OnDispose = null;
        }

        void ITimer.Start(GameObject owner, float duration, Action OnDispose)
        {
            progress = 0;
            waitTime = 0;
            this.owner = owner;
            this.duration = duration;
            this.OnDispose = OnDispose;
        }

        void ITimer.Update(float elapsedTime, float unscaleTime)
        {
            try
            {
                if (owner == null)
                {
                    OnDispose.Invoke();
                    return;
                }

                if (!owner.activeInHierarchy)
                {
                    OnDispose.Invoke();
                    return;
                }

                if (waitTime <= 0)
                {
                    waitTime = elapsedTime;
                }

                progress = (elapsedTime - waitTime) / duration;
                if (progress > 1)
                {
                    progress = 1;
                }

                OnUpdated.Invoke(progress);

                if (progress >= 1)
                {
                    OnDispose.Invoke();
                }
            }
            catch (Exception e)
            {
                OnDispose.Invoke();
                Debug.Log(Service.Text.Format("缓动差值无法执行方法：\n{0}", e));
            }
        }

        public Tween OnUpdate(Action<float> OnUpdated)
        {
            this.OnUpdated = OnUpdated;
            return this;
        }

        public void OnComplete(Action OnDispose)
        {
            this.OnDispose += OnDispose;
        }
    }
}