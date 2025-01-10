// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 17:01:16
// # Recently: 2025-01-10 17:01:17
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public static partial class Utility
    {
        public static class Event
        {
            public static event Action OnVariableEvent;

            internal static void Variable()
            {
                OnVariableEvent?.Invoke();
            }
        }
    }
}