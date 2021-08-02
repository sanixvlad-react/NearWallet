
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace SwipeListView
{
    public class SwipeMenuView : LinearLayout, View.IOnClickListener
    {

        private SwipeListView _listView;
        public SwipeMenuLayout SwipeMenuLayout { internal get; set; }
        private readonly SwipeMenu _menu;
        public IOnSwipeItemClickListener SwipeItemClickListener { get; set; }
        private int _position;

        public int GetPosition()
        {
            return _position;
        }

        public void SetPosition(int position)
        {
            _position = position;
        }

        public SwipeMenuView(SwipeMenu menu, SwipeListView listView) : base(menu.Context)
        {
            _listView = listView;
            _menu = menu;
            var items = menu.GetMenuItems();
            var id = 0;
            foreach (var item in items)
            {
                AddItem(item, id++);
            }
        }

        private void AddItem(SwipeMenuItem item, int id)
        {
            var paramsS = new LayoutParams(item.Width, ViewGroup.LayoutParams.MatchParent);
            var paramsP = new LayoutParams(item.Width - 2, ViewGroup.LayoutParams.MatchParent);
            var boss = new LinearLayout(Context)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = paramsS,
                Background = item.Background
            };
            var parent = new LinearLayout(Context)
            {
                Id = id,
                Orientation = Orientation.Vertical,
                LayoutParameters = paramsP
            };
            parent.SetGravity(GravityFlags.Center);
            parent.SetOnClickListener(this);
            AddView(boss);

            if (item.Icon != null)
            {
                parent.AddView(CreateIcon(item));
            }

            if (!string.IsNullOrEmpty(item.Title))
            {
                parent.AddView(CreateTitle(item));
            }

            var view = new View(Context)
            {
                LayoutParameters = new LayoutParams(2, ViewGroup.LayoutParams.MatchParent),
                Background = new ColorDrawable(Color.Red)
            };
            boss.AddView(view);
            boss.AddView(parent);

        }

        private ImageView CreateIcon(SwipeMenuItem item)
        {
            var iv = new ImageView(Context);
            iv.SetImageDrawable(item.Icon);
            return iv;
        }

        private TextView CreateTitle(SwipeMenuItem item)
        {
            var lp = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            var itemTitle = new TextView(Context)
            {
                Text = item.Title,
                Gravity = GravityFlags.Center,
                TextSize = item.TitleTextSize
            };
            itemTitle.SetTextColor(item.TitleTextColor);
            itemTitle.LayoutParameters = lp;
            itemTitle.SetBackgroundColor(Color.Transparent);

            return itemTitle;
        }
        public void OnClick(View v)
        {
            if (SwipeItemClickListener != null && SwipeMenuLayout.IsOpen())
            {
                SwipeItemClickListener.OnItemClick(this, _menu, v.Id);
            }
        }

        public interface IOnSwipeItemClickListener
        {
            void OnItemClick(SwipeMenuView view, SwipeMenu menu, int index);
        }
    }
}
