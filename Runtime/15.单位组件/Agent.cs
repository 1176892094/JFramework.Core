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
            public static T Show<T>(IEntity entity) where T : ScriptableObject, IAgent
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
                    agent = (IAgent)LoadPool(typeof(T)).Dequeue();
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
                    agent = (IAgent)LoadPool(agentType).Dequeue();
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
                        LoadPool(agent.GetType()).Enqueue((ScriptableObject)agent);
                    }

                    agentData.Clear();
                    Service.agentData.Remove(entity);
                }
            }

            private static IHeap<ScriptableObject> LoadPool(Type heapType)
            {
                if (Service.heapData.TryGetValue(heapType, out var heapData))
                {
                    return (IHeap<ScriptableObject>)heapData;
                }

                heapData = new AgentPool(heapType);
                Service.heapData.Add(heapType, heapData);
                return (IHeap<ScriptableObject>)heapData;
            }

            internal static void Dispose()
            {
                var agentCaches = new List<IEntity>(agentData.Keys);
                foreach (var cache in agentCaches)
                {
                    if (Service.agentData.TryGetValue(cache, out var agentData))
                    {
                        foreach (var agent in agentData.Values)
                        {
                            Object.Destroy((ScriptableObject)agent);
                        }

                        agentData.Clear();
                        Service.agentData.Remove(cache);
                    }
                }

                agentData.Clear();
            }
        }
    }
}