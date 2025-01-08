// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-08 19:01:53
// # Recently: 2025-01-08 19:01:53
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public abstract class UIPanel<TItem, TGrid> : UIPanel, IScroll where TGrid : IGrid<UIPanel<TItem, TGrid>, TItem>
    {
        [SerializeField, Inject] protected Scroll<UIPanel<TItem, TGrid>, TItem, TGrid> scroll;
        [SerializeField, Inject] protected RectTransform content;
        [SerializeField] protected Rect assetRect;
        [SerializeField] protected string assetPath;

        private void Update()
        {
            scroll.Update();
        }

        string IScroll.path => assetPath;
        int IScroll.row => (int)assetRect.x;
        int IScroll.column => (int)assetRect.y;
        float IScroll.width => assetRect.width;
        float IScroll.height => assetRect.height;
        float IScroll.position => content.anchoredPosition.y;
        RectTransform IScroll.content => content;

        public void Spawn(List<TItem> items)
        {
            scroll.SetItem(items);
        }
    }
}