// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-08 17:01:50
// # Recently: 2025-01-08 17:01:25
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static class Log
    {
        public static void Info(string message)
        {
            Debug.Log(message);
        }

        public static void Warn(string message)
        {
            Debug.LogWarning(message);
        }

        public static void Warn(string message, Object context)
        {
            Debug.LogWarning(message, context);
        }

        public static void Error(string message)
        {
            Debug.LogError(message);
        }

        public static void Error(string message, Object context)
        {
            Debug.LogError(message, context);
        }
    }
}