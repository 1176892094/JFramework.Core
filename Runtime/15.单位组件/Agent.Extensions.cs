// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-08 18:01:44
// # Recently: 2025-01-08 18:01:44
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        public static IAgent Agent(this IEntity current, Type agentType)
        {
            return Service.Agent.Show(current, agentType);
        }

        public static T Agent<T>(this IEntity current) where T : ScriptableObject, IAgent
        {
            return Service.Agent.Show<T>(current);
        }

        public static void Destroy(this IEntity current)
        {
            Service.Agent.Hide(current);
        }
    }
}