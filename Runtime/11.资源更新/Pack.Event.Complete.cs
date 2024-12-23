// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:15
// # Recently: 2024-12-22 20:12:21
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public struct PackCompleteEvent : IEvent
    {
        public bool status { get; private set; }
        public string message { get; private set; }

        public PackCompleteEvent(bool status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }
}