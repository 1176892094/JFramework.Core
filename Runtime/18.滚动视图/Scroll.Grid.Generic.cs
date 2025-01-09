// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    public interface IGrid<out TPanel, TItem> : IGrid where TPanel : IPanel
    {
        TItem item { get; }

        TPanel panel { get; }

        RectTransform content { get; }

        void SetItem(TItem item);
    }
}