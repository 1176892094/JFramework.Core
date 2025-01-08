// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 19:01:13
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    public sealed class Scroll<T, TItem, TGrid> : Agent<T> where T : IPanel, IScroll where TGrid : IGrid<T, TItem>
    {
        private Dictionary<int, TGrid> grids = new Dictionary<int, TGrid>();
        private List<TItem> items;
        private int oldMaxIndex = -1;
        private int oldMinIndex = -1;
     
        private int row;
        private int column;
        private float width;
        private float height;
        private string path;
        
        protected override void Dispose()
        {
            row = 0;
            width = 0;
            column = 0;
            height = 0;
            path = null;
            items = null;
            oldMinIndex = -1;
            oldMaxIndex = -1;
            grids.Clear();
        }

        protected override void Awake()
        {
            row = owner.row;
            path = owner.path;
            width = owner.width;
            column = owner.column;
            height = owner.height;
            owner.content.pivot = Vector2.up;
            owner.content.anchorMin = Vector2.up;
            owner.content.anchorMax = Vector2.one;
        }

        public void SetItem(List<TItem> items)
        {
            foreach (var i in grids.Keys)
            {
                if (grids.TryGetValue(i, out var grid))
                {
                    if (grid != null)
                    {
                        grid.Dispose();
                        Service.Pool.Hide(grid.gameObject);
                    }
                }
            }

            grids.Clear();
            this.items = items;
            owner.content.anchoredPosition = Vector2.zero;
            owner.content.sizeDelta = new Vector2(0, (float)Math.Ceiling((float)items.Count / column) * height + 1);
        }

        public void Update()
        {
            if (items == null) return;
            var minIndex = Math.Max(0, (int)(owner.position / height) * column);
            var maxIndex = Math.Min((int)((owner.position + row * height) / height) * column + column - 1, items.Count - 1);

            if (minIndex != oldMinIndex || maxIndex != oldMaxIndex)
            {
                for (var i = oldMinIndex; i < minIndex; ++i)
                {
                    if (grids.TryGetValue(i, out var grid))
                    {
                        if (grid != null)
                        {
                            grid.Dispose();
                            Service.Pool.Hide(grid.gameObject);
                        }

                        grids.Remove(i);
                    }
                }

                for (var i = maxIndex + 1; i <= oldMaxIndex; ++i)
                {
                    if (grids.TryGetValue(i, out var grid))
                    {
                        if (grid != null)
                        {
                            grid.Dispose();
                            Service.Pool.Hide(grid.gameObject);
                        }

                        grids.Remove(i);
                    }
                }
            }

            oldMinIndex = minIndex;
            oldMaxIndex = maxIndex;
            for (var i = minIndex; i <= maxIndex; ++i)
            {
                if (!grids.ContainsKey(i))
                {
                    var index = i;
                    grids[index] = default;
                    var posX = index % column * width + width / 2;
                    var posY = -(index / column) * height - height / 2;
                    Service.Pool.Show(path, obj =>
                    {
                        var transform = (RectTransform)obj.transform;
                        transform.SetParent(owner.content);
                        transform.localScale = Vector3.one;
                        transform.localPosition = new Vector3(posX, posY, 0);

                        var grid = obj.GetComponent<TGrid>();
                        grid ??= (TGrid)(object)obj.AddComponent(typeof(TGrid));
                        transform.sizeDelta = new Vector2(grid.scroll.width, grid.scroll.height);
                        if (!grids.ContainsKey(index))
                        {
                            grid.Dispose();
                            Service.Pool.Hide(grid.gameObject);
                            return;
                        }

                        grids[index] = grid;
                        grid.SetItem(items[index]);
                    });
                }
            }
        }
    }
}