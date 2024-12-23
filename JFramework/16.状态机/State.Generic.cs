// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 20:12:46
// # Recently: 2024-12-22 20:12:13
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    [Serializable]
    public abstract class State<T> : Agent<T>, IState
    {
        void IState.OnEnter() => OnEnter();

        void IState.OnUpdate() => OnUpdate();

        void IState.OnExit() => OnExit();

        protected abstract void OnEnter();

        protected abstract void OnUpdate();

        protected abstract void OnExit();
    }
}