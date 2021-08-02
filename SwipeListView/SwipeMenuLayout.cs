using System;

using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Views.Animations;

using AndroidX.Core.View;

namespace SwipeListView
{
    public class SwipeMenuLayout : FrameLayout
    {
        private static readonly int ContentViewId = 1;
        private static readonly int MenuViewId = 2;

        private SwipeDirection _swipeDirection;

        public View ContentView { get; internal set; }
        public SwipeMenuView MenuView { get; internal set; }
        private int _downX;
        private SwipeMenuLayoutState _state = SwipeMenuLayoutState.Close;
        private GestureDetectorCompat _gestureDetector;
        private GestureDetector.IOnGestureListener _gestureListener;
        public bool IsFling;
        public int MinFling;
        public int MaxVelocityx;
        private OverScroller _openScroller;
        private OverScroller _closeScroller;
        private int _baseX;
        private int _position;
        private readonly IInterpolator _closeInterpolator;
        private readonly IInterpolator _openInterpolator;

        public bool SwipEnable { get; set; } = true;

        public SwipeMenuLayout(View contentView, SwipeMenuView menuView) : this(contentView, menuView, null, null)
        {

        }

        public SwipeMenuLayout(View contentView, SwipeMenuView menuView, IInterpolator closeInterpolator, IInterpolator openInterpolator) : base(contentView.Context)
        {
            _closeInterpolator = closeInterpolator;
            _openInterpolator = openInterpolator;
            ContentView = contentView;
            MenuView = menuView;
            MenuView.SwipeMenuLayout = this;
            Init();
        }

        private SwipeMenuLayout(Context context, IAttributeSet attrs) : base(context, attrs) { }

        private SwipeMenuLayout(Context context) : base(context) { }

        public int GetPosition()
        {
            return _position;
        }

        public void SetPosition(int position)
        {
            _position = position;
            MenuView.SetPosition(position);
        }

        public void SetSwipeDirection(SwipeDirection swipeDirection)
        {
            _swipeDirection = swipeDirection;
        }
        private void Init()
        {
            MinFling = Dp2Px(15);
            MaxVelocityx = -Dp2Px(500);

            LayoutParameters = new AbsListView.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            _gestureListener = new GestureListerner(this);
            _gestureDetector = new GestureDetectorCompat(Context, _gestureListener);


            _closeScroller = _closeInterpolator != null ? new OverScroller(Context, _closeInterpolator) : new OverScroller(Context);

            _openScroller = _openInterpolator != null ? new OverScroller(Context, _openInterpolator) : new OverScroller(Context);

            var contentParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            ContentView.LayoutParameters = (contentParams);
            if (ContentView.Id < 1)
            {
                ContentView.Id = ContentViewId;
            }

            MenuView.Id = MenuViewId;
            MenuView.LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            AddView(ContentView);
            AddView(MenuView);
        }
        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
        }
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
        }

        public bool OnSwipe(MotionEvent e)
        {
            _gestureDetector.OnTouchEvent(e);
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _downX = (int)e.GetX();
                    IsFling = false;
                    break;
                case MotionEventActions.Move:
                    // Log.i("byz", "downX = " + _downX + ", moveX = " + event.getX());
                    var dis = (int)(_downX - e.GetX());
                    if (_state == SwipeMenuLayoutState.Open)
                    {
                        dis += MenuView.Width * (int)_swipeDirection;
                    }

                    Swipe(dis);
                    break;
                case MotionEventActions.Up:
                    if ((IsFling || Math.Abs(_downX - e.GetX()) > (MenuView.Width / 2)) && Math.Sign(_downX - e.GetX()) == (int)_swipeDirection)
                    {
                        // open
                        SmoothOpenMenu();
                    }
                    else
                    {
                        // close
                        SmoothCloseMenu();
                        return false;
                    }
                    break;
            }
            return true;
        }

        public bool IsOpen()
        {
            return _state == SwipeMenuLayoutState.Open;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return base.OnTouchEvent(e);
        }

        private void Swipe(int dis)
        {
            if (!SwipEnable)
            {
                return;
            }
            if (Math.Sign(dis) != (int)_swipeDirection)
            {
                dis = 0;
            }
            else if (Math.Abs(dis) > MenuView.Width)
            {
                dis = MenuView.Width * (int)_swipeDirection;
            }

            ContentView.Layout(-dis, ContentView.Top,
                    ContentView.Width - dis, MeasuredHeight);

            if (_swipeDirection == SwipeDirection.Left)
            {

                MenuView.Layout(ContentView.Width - dis, MenuView.Top,
                        ContentView.Width + MenuView.Width - dis,
                        MenuView.Bottom);
            }
            else
            {
                MenuView.Layout(-MenuView.Width - dis, MenuView.Top,
                        -dis, MenuView.Bottom);
            }
        }
        public override void ComputeScroll()
        {
            if (_state == SwipeMenuLayoutState.Open)
            {
                if (_openScroller.ComputeScrollOffset())
                {
                    Swipe(_openScroller.CurrX * (int)_swipeDirection);
                    PostInvalidate();
                }
            }
            else
            {
                if (_closeScroller.ComputeScrollOffset())
                {
                    Swipe((_baseX - _closeScroller.CurrX) * (int)_swipeDirection);
                    PostInvalidate();
                }
            }
        }
        public void SmoothCloseMenu()
        {
            _state = SwipeMenuLayoutState.Close;
            if (_swipeDirection == SwipeDirection.Left)
            {
                _baseX = -ContentView.Left;
                _closeScroller.StartScroll(0, 0, MenuView.Width, 0, 350);
            }
            else
            {
                _baseX = MenuView.Right;
                _closeScroller.StartScroll(0, 0, MenuView.Width, 0, 350);
            }
            PostInvalidate();
        }

        public void SmoothOpenMenu()
        {
            if (!SwipEnable)
            {
                return;
            }
            _state = SwipeMenuLayoutState.Open;
            if (_swipeDirection == SwipeDirection.Left)
            {
                _openScroller.StartScroll(-ContentView.Left, 0, MenuView.Width, 0, 350);
            }
            else
            {
                _openScroller.StartScroll(ContentView.Left, 0, MenuView.Width, 0, 350);
            }
            PostInvalidate();
        }

        public void CloseMenu()
        {
            if (_closeScroller.ComputeScrollOffset())
            {
                _closeScroller.AbortAnimation();
            }
            if (_state == SwipeMenuLayoutState.Open)
            {
                _state = SwipeMenuLayoutState.Close;
                Swipe(0);
            }
        }

        public void OpenMenu()
        {
            if (!SwipEnable)
            {
                return;
            }
            if (_state == SwipeMenuLayoutState.Close)
            {
                _state = SwipeMenuLayoutState.Open;
                Swipe(MenuView.Width * (int)_swipeDirection);
            }
        }

        public int Dp2Px(int dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp,
                    Context.Resources.DisplayMetrics);
        }
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            MenuView.Measure(MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(MeasuredHeight, MeasureSpecMode.Exactly));
        }
        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            ContentView.Layout(0, 0, MeasuredWidth, ContentView.MeasuredHeight);
            if (_swipeDirection == SwipeDirection.Left)
            {
                MenuView.Layout(MeasuredWidth, 0,
                        MeasuredWidth + MenuView.MeasuredWidth,
                        ContentView.MeasuredHeight);
            }
            else
            {
                MenuView.Layout(-MenuView.MeasuredWidth, 0,
                        0, ContentView.MeasuredHeight);
            }
        }
        public void SetMenuHeight(int measuredHeight)
        {
            LayoutParams param = (LayoutParams)MenuView.LayoutParameters;
            if (param.Height != measuredHeight)
            {
                param.Height = measuredHeight;
                MenuView.LayoutParameters = (MenuView.LayoutParameters);
            }
        }
    }

    class GestureListerner : GestureDetector.SimpleOnGestureListener
    {
        private readonly SwipeMenuLayout _swipeMenuLayout;
        public GestureListerner(SwipeMenuLayout swipeMenuLayout)
        {
            _swipeMenuLayout = swipeMenuLayout;
        }
        public override bool OnDown(MotionEvent e)
        {
            _swipeMenuLayout.IsFling = false;
            return false;
        }
        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            if (Math.Abs(e1.GetX() - e2.GetX()) > _swipeMenuLayout.MinFling && velocityX < _swipeMenuLayout.MaxVelocityx)
            {
                _swipeMenuLayout.IsFling = true;
            }
            return base.OnFling(e1, e2, velocityX, velocityY);
        }
    }
}
