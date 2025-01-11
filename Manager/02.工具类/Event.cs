// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-11 03:01:26
// # Recently: 2025-01-11 03:01:27
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework.Common
{
    public struct PackAwakeEvent : IEvent
    {
        public int[] sizes { get; private set; }

        public PackAwakeEvent(int[] sizes)
        {
            this.sizes = sizes;
        }
    }

    public struct PackUpdateEvent : IEvent
    {
        public string name { get; private set; }
        public float progress { get; private set; }

        public PackUpdateEvent(string name, float progress)
        {
            this.name = name;
            this.progress = progress;
        }
    }

    public struct PackCompleteEvent : IEvent
    {
        public int status { get; private set; }
        public string message { get; private set; }

        public PackCompleteEvent(int status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }

    public struct AssetAwakeEvent : IEvent
    {
        public string[] names { get; private set; }

        public AssetAwakeEvent(string[] names)
        {
            this.names = names;
        }
    }

    public struct AssetUpdateEvent : IEvent
    {
        public string name { get; private set; }

        public AssetUpdateEvent(string name)
        {
            this.name = name;
        }
    }

    public struct AssetCompleteEvent : IEvent
    {
    }

    public struct SceneAwakeEvent : IEvent
    {
        public string name { get; private set; }

        public SceneAwakeEvent(string name)
        {
            this.name = name;
        }
    }

    public struct SceneUpdateEvent : IEvent
    {
        public float progress { get; private set; }

        public SceneUpdateEvent(float progress)
        {
            this.progress = progress;
        }
    }

    public struct SceneCompleteEvent : IEvent
    {
    }

    public struct DataAwakeEvent : IEvent
    {
        public string[] names { get; private set; }

        public DataAwakeEvent(string[] names)
        {
            this.names = names;
        }
    }

    public struct DataUpdateEvent : IEvent
    {
        public string name { get; private set; }

        public DataUpdateEvent(string name)
        {
            this.name = name;
        }
    }

    public struct DataCompleteEvent : IEvent
    {
    }
}