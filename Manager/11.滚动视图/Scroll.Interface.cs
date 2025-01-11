// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-10 20:01:57
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework.Common
{
    public interface IContent
    {
        RectTransform content { get; }
    }

    public interface IScroll : IContent
    {
        Rect rect { get; }
        string prefab { get; }
    }
}