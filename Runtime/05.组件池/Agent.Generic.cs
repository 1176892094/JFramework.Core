// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:24
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    public abstract class Agent<T> : ScriptableObject, IAgent where T : IEntity
    {
        [SerializeField] private T instance;

        public T owner => instance ??= (T)Service.entity;

        public virtual void OnAwake(IEntity owner) => instance ??= (T)owner;

        public virtual void Dispose() => instance = default;
    }
}