using System;
using System.Collections.Generic;

using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Views.Animations;

namespace SwipeListView
{
    [Register("ru.sanix.SwipeListView")]
    public class SwipeListView : ListView
    {
        public SwipeDirection SwipeDeirection { get; set; } = SwipeDirection.Left;//swipe from right to left by default
        public IOnSwipeListener SwipeListener { internal get; set; }
        public ISwipeMenuCreator MenuCreator { get; set; }
        public IOnMenuItemClickListener MenuItemClickListener { get; set; }
        public IOnMenuStateChangeListener MenuStateChangeListener { internal get; set; }
        public IInterpolator CloseInterpolator { get; set; }
        public IInterpolator OpenInterpolator { get; set; }

        private int _maxY = 5;
        private int _maxX = 3;
        private float _downX;
        private float _downY;
        private TouchState _touchState;
        private int _touchPosition;
        private SwipeMenuLayout _menuLayout;


        public SwipeListView(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

        public SwipeListView(Context context) : base(context) { Init(); }

        public SwipeListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { Init(); }

        public SwipeListView(Context context, IAttributeSet attrs) : base(context, attrs) { Init(); }

        private void Init()
        {
            _maxX = Dp2Px(_maxX);
            _maxY = Dp2Px(_maxY);
            _touchState = TouchState.None;
        }
        public void SetMenuItems(Func<SwipeMenu, List<SwipeMenuItem>> func)
        {
            MenuCreator = new MenuCreatorGenerator(func);
        }
        [Obsolete("Please use the Adapter property setter")]
        public override void SetAdapter(IListAdapter adapter)
        {
            base.SetAdapter(new MyAdapter(this, adapter));
        }
        private SwipeMenuAdapter _adapter;
        public override IListAdapter Adapter { get => _adapter?.WrappedAdapter; set { base.Adapter = new MyAdapter(this, value); _adapter = new MyAdapter(this, value); } }
        public static bool InRangeOfView(View view, MotionEvent ev)
        {
            var location = new int[2];
            view.GetLocationOnScreen(location);
            var x = location[0];
            var y = location[1];
            return !(ev.RawX < x) && !(ev.RawX > (x + view.Width)) && !(ev.RawY < y) && !(ev.RawY > (y + view.Height));
        }
        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            var action = ev.Action;
            switch (action)
            {
                case MotionEventActions.Down:
                    _downX = ev.GetX();
                    _downY = ev.GetY();
                    var handled = base.OnInterceptTouchEvent(ev);
                    _touchState = TouchState.None;
                    _touchPosition = PointToPosition((int)ev.GetX(), (int)ev.GetY());
                    var view = GetChildAt(_touchPosition - FirstVisiblePosition);

                    if (view is SwipeMenuLayout layout)
                    {
                        if (_menuLayout != null && _menuLayout.IsOpen() && !InRangeOfView(_menuLayout.MenuView, ev))
                        {
                            return true;
                        }
                        _menuLayout = layout;
                        _menuLayout.SetSwipeDirection(SwipeDeirection);
                    }
                    if (_menuLayout != null && _menuLayout.IsOpen() && view != _menuLayout)
                    {
                        handled = true;
                    }

                    _menuLayout?.OnSwipe(ev);
                    return handled;
                case MotionEventActions.Move:
                    var dy = Math.Abs(ev.GetY() - _downY);
                    var dx = Math.Abs(ev.GetX() - _downX);
                    if (Math.Abs(dy) > _maxY || Math.Abs(dx) > _maxX)
                    {
                        if (_touchState != TouchState.None) return true;

                        if (Math.Abs(dy) > _maxY)
                        {
                            _touchState = TouchState.Y;
                        }
                        else if (dx > _maxX)
                        {
                            _touchState = TouchState.X;

                            SwipeListener?.OnSwipeStart(_touchPosition);
                        }
                        return true;
                    }
                    break;
            }
            return base.OnInterceptTouchEvent(ev);
        }
        public override bool OnTouchEvent(MotionEvent ev)
        {
            if (ev.Action != MotionEventActions.Down && _menuLayout == null)
                return base.OnTouchEvent(ev);
            var action = ev.Action;
            switch (action)
            {
                case MotionEventActions.Down:
                    var oldPos = _touchPosition;
                    _downX = ev.GetX();
                    _downY = ev.GetY();
                    _touchState = TouchState.None;

                    _touchPosition = PointToPosition((int)ev.GetX(), (int)ev.GetY());

                    if (_touchPosition == oldPos && _menuLayout != null
                            && _menuLayout.IsOpen())
                    {
                        _touchState = TouchState.X;
                        _menuLayout.OnSwipe(ev);
                        return true;
                    }

                    var view = GetChildAt(_touchPosition - FirstVisiblePosition);

                    if (_menuLayout != null && _menuLayout.IsOpen())
                    {
                        _menuLayout.SmoothCloseMenu();
                        _menuLayout = null;
                        // return super.onTouchEvent(ev);
                        // try to cancel the touch event
                        var cancelEvent = MotionEvent.Obtain(ev);
                        cancelEvent.Action = MotionEventActions.Cancel;
                        OnTouchEvent(cancelEvent);
                        MenuStateChangeListener?.OnMenuClose(oldPos);
                        return true;
                    }
                    if (view is SwipeMenuLayout)
                    {
                        _menuLayout = (SwipeMenuLayout)view;
                        _menuLayout.SetSwipeDirection(SwipeDeirection);
                    }
                    _menuLayout?.OnSwipe(ev);
                    break;
                case MotionEventActions.Move:
                    _touchPosition = PointToPosition((int)ev.GetX(), (int)ev.GetY()) - HeaderViewsCount;
                    if (!_menuLayout.SwipEnable || _touchPosition != _menuLayout.GetPosition())
                    {
                        break;
                    }
                    var dy = Math.Abs((ev.GetY() - _downY));
                    var dx = Math.Abs((ev.GetX() - _downX));
                    if (_touchState == TouchState.X)
                    {
                        _menuLayout?.OnSwipe(ev);
                        Selector.SetState(new[] { 0 });
                        ev.Action = MotionEventActions.Cancel;
                        base.OnTouchEvent(ev);
                        return true;
                    }
                    else if (_touchState == TouchState.None)
                    {
                        if (Math.Abs(dy) > _maxY)
                        {
                            _touchState = TouchState.Y;
                        }
                        else if (dx > _maxX)
                        {
                            _touchState = TouchState.X;
                            SwipeListener?.OnSwipeStart(_touchPosition);
                        }
                    }
                    break;
                case MotionEventActions.Up:
                    if (_touchState == TouchState.X)
                    {
                        if (_menuLayout != null)
                        {
                            var isBeforeOpen = _menuLayout.IsOpen();
                            _menuLayout.OnSwipe(ev);
                            var isAfterOpen = _menuLayout.IsOpen();
                            if (isBeforeOpen != isAfterOpen && MenuStateChangeListener != null)
                            {
                                if (isAfterOpen)
                                {
                                    MenuStateChangeListener.OnMenuOpen(_touchPosition);
                                }
                                else
                                {
                                    MenuStateChangeListener.OnMenuClose(_touchPosition);
                                }
                            }
                            if (!isAfterOpen)
                            {
                                _touchPosition = -1;
                                _menuLayout = null;
                            }
                        }
                        SwipeListener?.OnSwipeEnd(_touchPosition);
                        ev.Action = MotionEventActions.Cancel;
                        base.OnTouchEvent(ev);
                        return true;
                    }
                    break;
            }
            try
            {
                return base.OnTouchEvent(ev);
            }
            catch (Exception) { return false; }
        }
        public void SmoothOpenMenu(int position)
        {
            if (position < FirstVisiblePosition || position > LastVisiblePosition) return;

            var view = GetChildAt(position - FirstVisiblePosition);
            if (!(view is SwipeMenuLayout layout)) return;

            _touchPosition = position;
            if (_menuLayout != null && _menuLayout.IsOpen())
            {
                _menuLayout.SmoothCloseMenu();
            }
            _menuLayout = layout;
            _menuLayout.SetSwipeDirection(SwipeDeirection);
            _menuLayout.SmoothOpenMenu();
        }
        public void SmoothCloseMenu()
        {
            if (_menuLayout != null && _menuLayout.IsOpen())
            {
                _menuLayout.SmoothCloseMenu();
            }
        }

        private int Dp2Px(int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp,
                    Context.Resources.DisplayMetrics);
        }

        private class MyAdapter : SwipeMenuAdapter
        {
            private readonly SwipeListView _listview;
            public MyAdapter(SwipeListView swipeMenuListView, IListAdapter adapter) : base(swipeMenuListView.Context, adapter)
            {
                _listview = swipeMenuListView;
            }
            public override void CreateMenu(SwipeMenu menu)
            {
                _listview.MenuCreator?.Create(menu);
            }
            public override void OnItemClick(SwipeMenuView view, SwipeMenu menu, int index)
            {
                var flag = false;

                if (_listview.MenuItemClickListener != null)
                {
                    flag = _listview.MenuItemClickListener.OnMenuItemClick(view.GetPosition(), menu, index);
                }
                if (_listview._menuLayout != null && !flag)
                {
                    _listview._menuLayout.SmoothCloseMenu();
                }
            }
        }
        private class MenuCreatorGenerator : ISwipeMenuCreator
        {
            private readonly Func<SwipeMenu, List<SwipeMenuItem>> _itemsFunc;
            public MenuCreatorGenerator(Func<SwipeMenu, List<SwipeMenuItem>> itemsFunc)
            {
                _itemsFunc = itemsFunc;
            }
            public void Create(SwipeMenu menu)
            {
                menu.AddMenuItems(_itemsFunc.Invoke(menu));
            }
        }
    }

    public interface IOnMenuItemClickListener
    {
        bool OnMenuItemClick(int position, SwipeMenu menu, int index);
    }

    public interface IOnSwipeListener
    {
        void OnSwipeStart(int position);

        void OnSwipeEnd(int position);
    }

    public interface IOnMenuStateChangeListener
    {
        void OnMenuOpen(int position);

        void OnMenuClose(int position);
    }
}

