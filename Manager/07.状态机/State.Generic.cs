// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:32
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public abstract class State<T> : Agent<T>, IState where T : Component
    {
        void IState.OnEnter() => OnEnter();

        void IState.OnUpdate() => OnUpdate();

        void IState.OnExit() => OnExit();

        protected abstract void OnEnter();

        protected abstract void OnUpdate();

        protected abstract void OnExit();
    }
}