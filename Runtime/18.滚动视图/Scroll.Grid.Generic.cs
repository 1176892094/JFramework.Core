// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 20:12:46
// # Recently: 2024-12-22 20:12:29
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public interface IGrid<out TPanel, TItem> : IGrid where TPanel : IPanel
    {
        TItem item { get; }

        object content { get; }

        TPanel panel { get; }

        IScroll scroll { get; }

        void SetItem(TItem item);
    }
}