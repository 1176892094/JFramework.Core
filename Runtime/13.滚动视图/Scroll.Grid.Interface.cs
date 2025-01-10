// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 13:01:49
// # Recently: 2025-01-09 13:01:50
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    public interface IGrid<out TPanel, TItem> : IGrid<TItem> where TPanel : UIPanel
    {
        TPanel panel { get; }
        RectTransform content { get; }
    }
}