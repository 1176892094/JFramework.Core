// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 21:12:29
// # Recently: 2024-12-22 20:12:18
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework
{
    [Serializable]
    public abstract class Scroll<T, TItem, TGrid> : Agent<T> where T : IPanel, IScroll where TGrid : IGrid<T, TItem>
    {
        private int column;
        private Dictionary<int, TGrid> grids = new Dictionary<int, TGrid>();
        private float height;
        private List<TItem> items;
        private int oldMaxIndex = -1;
        private int oldMinIndex = -1;
        private string path;
        private int row;
        private float width;

        protected override void Dispose()
        {
            row = 0;
            column = 0;
            width = 0;
            height = 0;
            path = null;
            items = null;
            oldMinIndex = -1;
            oldMaxIndex = -1;
            grids.Clear();
        }

        protected override void Awake()
        {
            path = owner.path;
            row = owner.row;
            column = owner.column;
            width = owner.width;
            height = owner.height;
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
                        Service.Pool.Hide(grid);
                    }
                }
            }

            grids.Clear();
            this.items = items;
            var newHeight = (float)Math.Ceiling((float)items.Count / column) * height + 1;
            SetItem(newHeight);
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
                            Service.Pool.Hide(grid);
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
                            Service.Pool.Hide(grid);
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
                    Service.Pool.Show(path, typeof(TGrid), entity =>
                    {
                        var grid = entity.GetComponent<TGrid>();
                        var posX = index % column * width + width / 2;
                        var posY = -(index / column) * height - height / 2;
                        SetGrid(grid, posX, posY);

                        if (!grids.ContainsKey(index))
                        {
                            grid.Dispose();
                            Service.Pool.Hide(grid);
                            return;
                        }

                        grids[index] = grid;
                        grid.SetItem(items[index]);
                    });
                }
            }
        }

        protected abstract void SetItem(float height);

        protected abstract void SetGrid(TGrid grid, float posX, float posY);
    }
}