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

namespace JFramework
{
    public abstract class StateMachine<TEntity> : Agent<TEntity> where TEntity : IEntity
    {
        private readonly Dictionary<Type, IState> states = new Dictionary<Type, IState>();
        public IState state { get; private set; }

        public override void Dispose()
        {
            state = null;
            var copies = new List<IState>(states.Values);
            foreach (var stateData in copies)
            {
                stateData.Dispose();
                Service.Heap.Enqueue(stateData, stateData.GetType());
            }

            states.Clear();
        }

        public void OnUpdate()
        {
            state.OnUpdate();
        }

        public void AddState<T>() where T : IState, new()
        {
            var stateData = Service.Heap.Dequeue<IState>(typeof(T));
            states[typeof(T)] = stateData;
            stateData.OnAwake(owner);
        }

        public void AddState<T>(Type stateType) where T : IState
        {
            var stateData = Service.Heap.Dequeue<IState>(stateType);
            states[typeof(T)] = stateData;
            stateData.OnAwake(owner);
        }

        public void ChangeState<T>() where T : IState
        {
            state.OnExit();
            state = states[typeof(T)];
            state.OnEnter();
        }
    }
}