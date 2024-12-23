// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:32
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    [Serializable]
    public sealed class Watch : ITimer
    {
        private float duration;
        private float keepTime;
        private Action OnFinish;
        private Action OnUpdate;

        private IEntity owner;
        private int progress;
        private bool unscaled;
        private float waitTime;

        public void Dispose()
        {
            owner = null;
            progress = 0;
            duration = 0;
            waitTime = 0;
            OnUpdate = null;
            OnFinish = null;
            unscaled = false;
        }

        void ITimer.Start(IEntity owner, float duration, Action OnFinish)
        {
            progress = 1;
            waitTime = 0;
            unscaled = false;
            this.owner = owner;
            this.duration = duration;
            this.OnFinish = OnFinish;
        }

        void IUpdate.Update(float elapsedTime, float unscaleTime)
        {
            try
            {
                if (!Service.IsEntity(owner))
                {
                    OnFinish.Invoke();
                    return;
                }

                if (!Service.IsActive(owner))
                {
                    OnFinish.Invoke();
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
                OnUpdate.Invoke();

                if (progress == 0)
                {
                    OnFinish.Invoke();
                }
            }
            catch (Exception e)
            {
                OnFinish.Invoke();
                Service.Log(Service.Text.Format("计时器无法执行方法：\n{0}", e));
            }
        }

        public Watch Invoke(Action OnUpdate)
        {
            this.OnUpdate = OnUpdate;
            return this;
        }

        public Watch OnComplete(Action OnFinish)
        {
            this.OnFinish += OnFinish;
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