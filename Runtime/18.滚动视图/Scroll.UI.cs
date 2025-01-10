// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-09 20:01:17
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public abstract class UIScroll<TItem, TGrid> : UIPanel, IScroll where TGrid : Component, IGrid<UIScroll<TItem, TGrid>, TItem>
    {
        [SerializeField, Inject] protected Scroll<UIScroll<TItem, TGrid>, TItem, TGrid> scroll;
        [SerializeField, Inject] protected RectTransform content;
        [SerializeField] protected string assetPath;
        [SerializeField] protected Rect assetRect;

        private void Update()
        {
            scroll.Update();
        }

        Rect IScroll.rect => assetRect;
        string IScroll.prefab => assetPath;
        RectTransform IScroll.content => content;

        public void Spawn(List<TItem> items)
        {
            scroll.SetItem(items);
        }
    }
}