// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:31
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public struct AssetAwakeEvent : IEvent
    {
        public string[] names { get; private set; }

        public AssetAwakeEvent(string[] names)
        {
            this.names = names;
        }
    }
}