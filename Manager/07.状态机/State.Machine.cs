// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:34
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    public abstract class StateMachine<TOwner> : Agent<TOwner> where TOwner : Component
    {
        private readonly Dictionary<Type, IState> states = new Dictionary<Type, IState>();
        private IState state;

        public override void Dispose()
        {
            state = null;
            states.Clear();
        }

        public void OnUpdate()
        {
            state.OnUpdate();
        }

        public void AddState<T>() where T : State<TOwner>
        {
            states[typeof(T)] = (State<TOwner>)AgentManager.Show(owner.gameObject, typeof(T));
        }

        public void AddState(Type stateType)
        {
            states[stateType] = (State<TOwner>)AgentManager.Show(owner.gameObject, stateType);
        }

        public void ChangeState<T>() where T : State<TOwner>
        {
            state.OnExit();
            state = states[typeof(T)];
            state.OnEnter();
        }

        public void ChangeState(Type stateType)
        {
            state.OnExit();
            state = states[stateType];
            state.OnEnter();
        }

        public void RemoveState<T>() where T : State<TOwner>
        {
            states.Remove(typeof(T));
        }

        public void RemoveState(Type stateType)
        {
            states.Remove(stateType);
        }
    }
}