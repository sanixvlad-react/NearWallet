
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Database;

using Java.Lang;

namespace SwipeListView
{
    public class SwipeMenuAdapter : Object, IWrapperListAdapter, SwipeMenuView.IOnSwipeItemClickListener
    {

        private readonly IListAdapter _adapter;
        private readonly Context _context;
        public IOnMenuItemClickListener MenuItemClickListener { get; set; }

        public IListAdapter WrappedAdapter => _adapter;

        public int Count => _adapter.Count;

        public bool HasStableIds => _adapter.HasStableIds;

        public bool IsEmpty => _adapter.IsEmpty;

        public int ViewTypeCount => _adapter.ViewTypeCount;

        public SwipeMenuAdapter(Context context, IListAdapter adapter)
        {
            _adapter = adapter;
            _context = context;
        }


        public virtual void CreateMenu(SwipeMenu menu)
        {
        }

        public bool AreAllItemsEnabled()
        {
            return _adapter.AreAllItemsEnabled();
        }

        public bool IsEnabled(int position)
        {
            return _adapter.IsEnabled(position);
        }

        public Object GetItem(int position)
        {
            return _adapter.GetItem(position);

        }

        public long GetItemId(int position)
        {
            return _adapter.GetItemId(position);
        }

        public int GetItemViewType(int position)
        {
            return _adapter.GetItemViewType(position);
        }

        public View GetView(int position, View convertView, ViewGroup parent)
        {
            SwipeMenuLayout layout;
            if (convertView == null)
            {
                var contentView = _adapter.GetView(position, convertView, parent);
                var menu = new SwipeMenu(_context);
                menu.SetViewType(GetItemViewType(position));
                CreateMenu(menu);
                var menuView = new SwipeMenuView(menu, (SwipeListView)parent)
                {
                    SwipeItemClickListener = this
                };
                var listView = (SwipeListView)parent;
                layout = new SwipeMenuLayout(contentView, menuView, listView.CloseInterpolator, listView.OpenInterpolator);
                layout.SetPosition(position);
            }
            else
            {
                layout = (SwipeMenuLayout)convertView;
                layout.CloseMenu();
                layout.SetPosition(position);
                var view = _adapter.GetView(position, layout.ContentView, parent);
            }

            if (!(_adapter is BaseSwipListAdapter adapter)) return layout;

            var swipEnable = adapter.GetSwipEnableByPosition(position);
            layout.SwipEnable = swipEnable;

            return layout;
        }

        public void RegisterDataSetObserver(DataSetObserver observer)
        {
            _adapter.RegisterDataSetObserver(observer);
        }

        public void UnregisterDataSetObserver(DataSetObserver observer)
        {
            _adapter.UnregisterDataSetObserver(observer);
        }

        public virtual void OnItemClick(SwipeMenuView view, SwipeMenu menu, int index)
        {
            if (MenuItemClickListener != null)
            {
                MenuItemClickListener.OnMenuItemClick(view.GetPosition(), menu,
                        index);
            }
        }
    }
}
