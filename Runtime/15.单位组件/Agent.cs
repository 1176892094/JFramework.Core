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

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace JFramework
{
    public static partial class Service
    {
        internal static class Agent
        {
            public static T Show<T>(IEntity entity) where T : class, IAgent
            {
                if (helper == null) return default;
                if (!Service.agentData.TryGetValue(entity, out var agentData))
                {
                    agentData = new Dictionary<Type, IAgent>();
                    Service.agentData.Add(entity, agentData);
                }

                if (!agentData.TryGetValue(typeof(T), out var agent))
                {
                    Service.entity = entity;
                    agent = Model.Dequeue<T>();
                    agentData.Add(typeof(T), agent);
                    agent.OnAwake(entity);
                }

                return (T)Service.agentData[entity][typeof(T)];
            }

            public static IAgent Show(IEntity entity, Type agentType)
            {
                if (helper == null) return default;
                if (!Service.agentData.TryGetValue(entity, out var agentData))
                {
                    agentData = new Dictionary<Type, IAgent>();
                    Service.agentData.Add(entity, agentData);
                }

                if (!agentData.TryGetValue(agentType, out var agent))
                {
                    Service.entity = entity;
                    agent = Model.Dequeue<IAgent>(agentType);
                    agentData.Add(agentType, agent);
                    agent.OnAwake(entity);
                }

                return Service.agentData[entity][agentType];
            }

            public static void Hide(IEntity entity)
            {
                if (helper == null) return;
                if (Service.agentData.TryGetValue(entity, out var agentData))
                {
                    foreach (var agent in agentData.Values)
                    {
                        agent.Dispose();
                        Model.Enqueue(agent, agent.GetType());
                    }

                    agentData.Clear();
                    Service.agentData.Remove(entity);
                }
            }

            private static void Destroy(IEntity entity)
            {
                if (helper == null) return;
                if (Service.agentData.TryGetValue(entity, out var agentData))
                {
                    foreach (var agent in agentData.Values)
                    {
                        Object.Destroy((ScriptableObject)agent);
                    }

                    agentData.Clear();
                    Service.agentData.Remove(entity);
                }
            }

            internal static void Dispose()
            {
                var agentCaches = new List<IEntity>(agentData.Keys);
                foreach (var cache in agentCaches)
                {
                    Destroy(cache);
                }

                agentData.Clear();
            }
        }
    }
}