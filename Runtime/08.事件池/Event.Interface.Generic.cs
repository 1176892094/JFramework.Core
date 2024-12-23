// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-14 02:12:46
// # Recently: 2024-12-22 20:12:22
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public interface IEvent<in T> where T : struct, IEvent
    {
        void Execute(T message);
    }
}