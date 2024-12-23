// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public struct SceneAwakeEvent : IEvent
    {
        public SceneAwakeEvent(string name)
        {
            this.name = name;
        }

        public string name { get; private set; }
    }
}