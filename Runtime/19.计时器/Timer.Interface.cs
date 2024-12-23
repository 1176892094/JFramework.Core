// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-17 15:12:38
// # Recently: 2024-12-22 20:12:29
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    internal interface ITimer : IUpdate
    {
        void Start(IEntity owner, float duration, Action OnFinish);
    }
}