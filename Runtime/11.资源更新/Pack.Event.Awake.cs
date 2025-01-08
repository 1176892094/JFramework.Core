// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:37
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public struct PackAwakeEvent : IEvent
    {
        public int[] sizes { get; private set; }

        public PackAwakeEvent(int[] sizes)
        {
            this.sizes = sizes;
        }
    }
}