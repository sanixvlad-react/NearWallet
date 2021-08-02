
using System.Collections.Generic;

using Android.Content;

namespace SwipeListView
{
    public class SwipeMenu
    {

        public Context Context { get; internal set; }
        private readonly List<SwipeMenuItem> _items;
        private int _viewType;

        public SwipeMenu(Context context)
        {
            Context = context;
            _items = new List<SwipeMenuItem>();
        }

        public void AddMenuItem(SwipeMenuItem item)
        {
            _items.Add(item);
        }
        public void AddMenuItems(List<SwipeMenuItem> items)
        {
            _items.AddRange(items);
        }

        public void RemoveMenuItem(SwipeMenuItem item)
        {
            _items.Remove(item);
        }

        public List<SwipeMenuItem> GetMenuItems()
        {
            return _items;
        }

        public SwipeMenuItem GetMenuItem(int index)
        {
            return _items[index];
        }

        public int GetViewType()
        {
            return _viewType;
        }

        public void SetViewType(int viewType)
        {
            _viewType = viewType;
        }

    }
}
