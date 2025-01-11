// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-10 20:01:59
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public enum UIState : byte
    {
        Common,
        Freeze,
        Stable,
    }

    [Flags]
    public enum UILayer : byte
    {
        Layer0 = 0,
        Layer1 = 1 << 0,
        Layer2 = 1 << 1,
        Layer3 = 1 << 2,
        Layer4 = 1 << 3,
        Layer5 = 1 << 4,
        Layer6 = 1 << 5,
        Layer7 = 1 << 6,
        Layer8 = 1 << 7,
    }
}