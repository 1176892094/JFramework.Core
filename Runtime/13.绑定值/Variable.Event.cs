// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-11 03:01:58
// # Recently: 2025-01-11 03:01:59
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework.Events
{
    public struct VariableEvent : IEvent
    {
    }
    
    public struct PingUpdateEvent : IEvent
    {
        public double pingTime { get; private set; }

        public PingUpdateEvent(double pingTime)
        {
            this.pingTime = pingTime;
        }
    }
}