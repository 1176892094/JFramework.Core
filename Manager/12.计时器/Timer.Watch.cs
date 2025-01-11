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
    public sealed class Watch : ITimer
    {
        private float duration;
        private float keepTime;
        private Action OnDispose;
        private Action OnUpdated;

        private GameObject owner;
        private int progress;
        private bool unscaled;
        private float waitTime;

        public void Dispose()
        {
            owner = null;
            progress = 0;
            duration = 0;
            waitTime = 0;
            unscaled = false;
            OnUpdated = null;
            OnDispose = null;
        }

        void ITimer.Start(GameObject owner, float duration, Action OnDispose)
        {
            progress = 1;
            waitTime = 0;
            unscaled = false;
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


                keepTime = unscaled ? elapsedTime : unscaleTime;
                if (waitTime <= 0)
                {
                    waitTime = keepTime + duration;
                }

                if (keepTime <= waitTime)
                {
                    return;
                }

                progress--;
                waitTime = keepTime + duration;
                OnUpdated?.Invoke();

                if (progress == 0)
                {
                    OnDispose.Invoke();
                }
            }
            catch (Exception e)
            {
                OnDispose.Invoke();
                Debug.Log(Service.Text.Format("计时器无法执行方法：\n{0}", e));
            }
        }

        public Watch OnUpdate(Action OnUpdated)
        {
            this.OnUpdated = OnUpdated;
            return this;
        }

        public Watch OnComplete(Action OnDispose)
        {
            this.OnDispose += OnDispose;
            return this;
        }

        public Watch Set(float duration)
        {
            this.duration = duration;
            waitTime = keepTime + duration;
            return this;
        }

        public Watch Add(float duration)
        {
            waitTime += duration;
            return this;
        }

        public Watch Loops(int progress = 0)
        {
            this.progress = progress;
            return this;
        }

        public Watch Unscale(bool unscaled = true)
        {
            this.unscaled = unscaled;
            waitTime = keepTime + duration;
            return this;
        }
    }
}