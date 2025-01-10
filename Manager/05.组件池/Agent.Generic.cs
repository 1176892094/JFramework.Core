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
    public abstract class Agent<T> : ScriptableObject, IAgent where T : Component
    {
        [SerializeField] private T instance;

        public T owner => instance ??= GlobalManager.entity.GetComponent<T>();

        public virtual void OnAwake(GameObject owner) => instance ??= owner.GetComponent<T>();

        public virtual void Dispose() => instance = default;
    }
}