using System;

using Android.OS;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.Graphics;

namespace SwitchTab
{
    [Register("ru.sanix.SwitchTabView")]
    public class SwitchTabView : View
    {
        public event EventHandler<SwitchTabViewEventArgs> SwitchChanged = delegate { };

        private const String TAG = "SwitchMultiButton";
        /*default value*/
        private string[] mTabTexts = { "L", "R" };
        private int mTabNum = 2;
        private const float STROKE_RADIUS = 0;
        private const float STROKE_WIDTH = 2;
        private const float TEXT_SIZE = 14;
        private int SELECTED_COLOR = Color.ParseColor("#ffeb7b00");// 0xffeb7b00;
        private int DISABLE_COLOR = Color.ParseColor("#ffcccccc");//0xffcccccc;
        private int ENABLE_COLOR = Color.ParseColor("#ffffffff");//0xffffffff;
        private const int SELECTED_TAB = 0;
        private const String FONTS_DIR = "fonts/";
        /*other*/
        private Paint mStrokePaint;
        private Paint mFillPaint;
        private int mWidth;
        private int mHeight;
        private TextPaint mSelectedTextPaint;
        private TextPaint mUnselectedTextPaint;
        private OnSwitchListener onSwitchListener;
        private float mStrokeRadius;
        private float mStrokeWidth;
        private int mSelectedColor;
        private int mDisableColor;
        private float mTextSize;
        private int mSelectedTab;
        private float perWidth;
        private float mTextHeightOffset;
        private Paint.FontMetrics mFontMetrics;
        private Typeface typeface;
        private bool mEnable = true;

        public SwitchTabView(Context context) : base(context) { }

        public SwitchTabView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context, attrs);
            InitPaint();
        }

        public SwitchTabView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize(context, attrs);
            InitPaint();
        }

        private void Initialize(Context context, IAttributeSet attrs)
        {
            var typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.SwitchMultiButton);
            mStrokeRadius = typedArray.GetDimension(Resource.Styleable.SwitchMultiButton_strokeRadius, STROKE_RADIUS);
            mStrokeWidth = typedArray.GetDimension(Resource.Styleable.SwitchMultiButton_strokeWidth, STROKE_WIDTH);
            mTextSize = typedArray.GetDimension(Resource.Styleable.SwitchMultiButton_textSize, TEXT_SIZE);
            mSelectedColor = typedArray.GetColor(Resource.Styleable.SwitchMultiButton_selectedColor, SELECTED_COLOR);
            mDisableColor = typedArray.GetColor(Resource.Styleable.SwitchMultiButton_disableColor, DISABLE_COLOR);
            mSelectedTab = typedArray.GetInteger(Resource.Styleable.SwitchMultiButton_selectedTab, SELECTED_TAB);
            String mTypeface = typedArray.GetString(Resource.Styleable.SwitchMultiButton_typeface);
            int mSwitchTabsResId = typedArray.GetResourceId(Resource.Styleable.SwitchMultiButton_switchTabs, 0);

            if (mSwitchTabsResId != 0)
            {
                mTabTexts = Resources.GetStringArray(mSwitchTabsResId);
                mTabNum = mTabTexts.Length;
            }

            SwitchChanged(this, new SwitchTabViewEventArgs { Index = mSelectedTab, TabName = mTabTexts[mSelectedTab] });

            if (!TextUtils.IsEmpty(mTypeface))
            {
                typeface = Typeface.CreateFromAsset(context.Assets, FONTS_DIR + mTypeface);
            }
            typedArray.Recycle();
        }

        private void InitPaint()
        {
            // round rectangle paint
            mStrokePaint = new Paint();
            mStrokePaint.Color = new Color(mSelectedColor);
            mStrokePaint.SetStyle(Paint.Style.Stroke);
            mStrokePaint.AntiAlias = true;
            mStrokePaint.StrokeWidth = mStrokeWidth;
            // selected paint
            mFillPaint = new Paint();
            mFillPaint.Color = new Color(mSelectedColor);
            mFillPaint.SetStyle(Paint.Style.FillAndStroke);
            mStrokePaint.AntiAlias = true;
            // selected text paint
            mSelectedTextPaint = new TextPaint(PaintFlags.AntiAlias);
            mSelectedTextPaint.TextSize = mTextSize;
            mSelectedTextPaint.Color = new Color(mDisableColor);
            mStrokePaint.AntiAlias = true;
            // unselected text paint
            mUnselectedTextPaint = new TextPaint(PaintFlags.AntiAlias);
            mUnselectedTextPaint.TextSize = mTextSize;
            mUnselectedTextPaint.Color = new Color(mSelectedColor);
            mStrokePaint.AntiAlias = true;
            mTextHeightOffset = -(mSelectedTextPaint.Ascent() + mSelectedTextPaint.Descent()) * 0.5f;
            mFontMetrics = mSelectedTextPaint.GetFontMetrics();
            if (typeface != null)
            {
                mSelectedTextPaint.SetTypeface(typeface);
                mUnselectedTextPaint.SetTypeface(typeface);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            //base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            int defaultWidth = GetDefaultWidth();
            int defaultHeight = GetDefaultHeight();
            SetMeasuredDimension(GetExpectSize(defaultWidth, widthMeasureSpec), GetExpectSize(defaultHeight,
                    heightMeasureSpec));
        }

        /**
        * get default height when android:layout_height="wrap_content"
        */
        private int GetDefaultHeight()
        {
            return (int)(mFontMetrics.Bottom - mFontMetrics.Top) + PaddingTop + PaddingBottom;
        }

        /**
         * get default width when android:layout_width="wrap_content"
         */
        private int GetDefaultWidth()
        {
            float tabTextWidth = 0f;
            int tabs = mTabTexts.Length;
            for (int i = 0; i < tabs; i++)
            {
                tabTextWidth = Math.Max(tabTextWidth, mSelectedTextPaint.MeasureText(mTabTexts[i]));
            }
            float totalTextWidth = tabTextWidth * tabs;
            float totalStrokeWidth = (mStrokeWidth * tabs);
            int totalPadding = (PaddingRight + PaddingLeft) * tabs;
            return (int)(totalTextWidth + totalStrokeWidth + totalPadding);
        }

        /**
        * get expect size
        *
        * @param size
        * @param measureSpec
        * @return
        */
        private int GetExpectSize(int size, int measureSpec)
        {
            int result = size;
            var specMode = MeasureSpec.GetMode(measureSpec);
            int specSize = MeasureSpec.GetSize(measureSpec);
            switch (specMode)
            {
                case MeasureSpecMode.Exactly:
                    result = specSize;
                    break;
                case MeasureSpecMode.Unspecified:
                    result = size;
                    break;
                case MeasureSpecMode.AtMost:
                    result = Math.Min(size, specSize);
                    break;
                default:
                    break;
            }
            return result;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (!mEnable)
            {
                mStrokePaint.Color = new Color(mDisableColor);
                mFillPaint.Color = new Color(mDisableColor);
                mSelectedTextPaint.Color = new Color(ENABLE_COLOR);
                mUnselectedTextPaint.Color = new Color(mDisableColor);
            }
            float left = mStrokeWidth * 0.5f;
            float top = mStrokeWidth * 0.5f;
            float right = mWidth - mStrokeWidth * 0.5f;
            float bottom = mHeight - mStrokeWidth * 0.5f;

            //draw rounded rectangle
            canvas.DrawRoundRect(new RectF(left, top, right, bottom), mStrokeRadius, mStrokeRadius, mStrokePaint);

            //draw line
            for (int i = 0; i < mTabNum - 1; i++)
            {
                canvas.DrawLine(perWidth * (i + 1), top, perWidth * (i + 1), bottom, mStrokePaint);
            }
            //draw tab and line
            for (int i = 0; i < mTabNum; i++)
            {
                String tabText = mTabTexts[i];
                float tabTextWidth = mSelectedTextPaint.MeasureText(tabText);
                if (i == mSelectedTab)
                {
                    //draw selected tab
                    if (i == 0)
                    {
                        DrawLeftPath(canvas, left, top, bottom);
                    }
                    else if (i == mTabNum - 1)
                    {
                        DrawRightPath(canvas, top, right, bottom);
                    }
                    else
                    {
                        canvas.DrawRect(new RectF(perWidth * i, top, perWidth * (i + 1), bottom), mFillPaint);
                    }
                    // draw selected text
                    canvas.DrawText(tabText, 0.5f * perWidth * (2 * i + 1) - 0.5f * tabTextWidth, mHeight * 0.5f +
                            mTextHeightOffset, mSelectedTextPaint);
                }
                else
                {
                    //draw unselected text
                    canvas.DrawText(tabText, 0.5f * perWidth * (2 * i + 1) - 0.5f * tabTextWidth, mHeight * 0.5f +
                            mTextHeightOffset, mUnselectedTextPaint);
                }
            }
        }

        /**
        * draw right path
        *
        * @param canvas
        * @param left
        * @param top
        * @param bottom
        */
        private void DrawLeftPath(Canvas canvas, float left, float top, float bottom)
        {
            Path leftPath = new Path();
            leftPath.MoveTo(left + mStrokeRadius, top);
            leftPath.LineTo(perWidth, top);
            leftPath.LineTo(perWidth, bottom);
            leftPath.LineTo(left + mStrokeRadius, bottom);
            leftPath.ArcTo(new RectF(left, bottom - 2 * mStrokeRadius, left + 2 * mStrokeRadius, bottom), 90, 90);
            leftPath.LineTo(left, top + mStrokeRadius);
            leftPath.ArcTo(new RectF(left, top, left + 2 * mStrokeRadius, top + 2 * mStrokeRadius), 180, 90);
            canvas.DrawPath(leftPath, mFillPaint);
        }

        /**
        * draw left path
        *
        * @param canvas
        * @param top
        * @param right
        * @param bottom
        */
        private void DrawRightPath(Canvas canvas, float top, float right, float bottom)
        {
            Path rightPath = new Path();
            rightPath.MoveTo(right - mStrokeRadius, top);
            rightPath.LineTo(right - perWidth, top);
            rightPath.LineTo(right - perWidth, bottom);
            rightPath.LineTo(right - mStrokeRadius, bottom);
            rightPath.ArcTo(new RectF(right - 2 * mStrokeRadius, bottom - 2 * mStrokeRadius, right, bottom), 90, -90);
            rightPath.LineTo(right, top + mStrokeRadius);
            rightPath.ArcTo(new RectF(right - 2 * mStrokeRadius, top, right, top + 2 * mStrokeRadius), 0, -90);
            canvas.DrawPath(rightPath, mFillPaint);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            mWidth = MeasuredWidth;
            mHeight = MeasuredHeight;
            perWidth = mWidth / mTabNum;
            CheckAttrs();
        }

        private void CheckAttrs()
        {
            if (mStrokeRadius > 0.5f * mHeight)
            {
                mStrokeRadius = 0.5f * mHeight;
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (!mEnable)
                return true;

            if (e.Action == MotionEventActions.Up)
            {
                float x = e.GetX();
                for (int i = 0; i < mTabNum; i++)
                {
                    if (x > perWidth * i && x < perWidth * (i + 1))
                    {
                        if (mSelectedTab == i)
                            return true;
                        mSelectedTab = i;
                        if (onSwitchListener != null)
                        {
                            onSwitchListener.onSwitch(i, mTabTexts[i]);
                        }

                        SwitchChanged(this, new SwitchTabViewEventArgs { Index = mSelectedTab, TabName = mTabTexts[mSelectedTab] });
                    }
                }
                Invalidate();
            }
            return true;
        }

        public override bool Enabled { get => mEnable; set => mEnable = value; }

        /*=========================================Interface=========================================*/

        /**
         * called when swtiched
         */
        public interface OnSwitchListener
        {
            void onSwitch(int position, String tabText);
        }

        public SwitchTabView SetOnSwitchListener(OnSwitchListener onSwitchListener)
        {
            this.onSwitchListener = onSwitchListener;
            return this;
        }

        /*=========================================Set and Get=========================================*/

        /**
         * get position of selected tab
         */
        public int GetSelectedTab()
        {
            return mSelectedTab;
        }

        /**
         * set selected tab
         *
         * @param mSelectedTab
         * @return
         */
        public SwitchTabView SetSelectedTab(int mSelectedTab)
        {
            this.mSelectedTab = mSelectedTab;
            Invalidate();
            if (onSwitchListener != null)
            {
                onSwitchListener.onSwitch(mSelectedTab, mTabTexts[mSelectedTab]);
                
            }
            if (this.mSelectedTab >= 0 && this.mSelectedTab <= mTabTexts.Length)
                SwitchChanged(this, new SwitchTabViewEventArgs { Index = mSelectedTab, TabName = mTabTexts[mSelectedTab] });

            return this;
        }

        public void ClearSelection()
        {
            this.mSelectedTab = -1;
            Invalidate();
        }

        /**
         * set data for the switchbutton
         *
         * @param tagTexts
         * @return
         */
        public SwitchTabView SetText(String[] tagTexts)
        {
            if (tagTexts.Length > 1)
            {
                this.mTabTexts = tagTexts;
                mTabNum = tagTexts.Length;
                RequestLayout();
                return this;
            }
            else
            {
                throw new Java.Lang.IllegalArgumentException("the size of tagTexts should greater then 1");
            }
        }

        /*======================================save and restore======================================*/

        protected override IParcelable OnSaveInstanceState()
        {
            Bundle bundle = new Bundle();
            bundle.PutParcelable("View", base.OnSaveInstanceState());
            bundle.PutFloat("StrokeRadius", mStrokeRadius);
            bundle.PutFloat("StrokeWidth", mStrokeWidth);
            bundle.PutFloat("TextSize", mTextSize);
            bundle.PutInt("SelectedColor", mSelectedColor);
            bundle.PutInt("DisableColor", mDisableColor);
            bundle.PutInt("SelectedTab", mSelectedTab);
            bundle.PutBoolean("Enable", mEnable);
            return bundle;
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            if (state is Bundle) {
                Bundle bundle = (Bundle)state;
                mStrokeRadius = bundle.GetFloat("StrokeRadius");
                mStrokeWidth = bundle.GetFloat("StrokeWidth");
                mTextSize = bundle.GetFloat("TextSize");
                mSelectedColor = bundle.GetInt("SelectedColor");
                mDisableColor = bundle.GetInt("DisableColor");
                mSelectedTab = bundle.GetInt("SelectedTab");
                mEnable = bundle.GetBoolean("Enable");
                base.OnRestoreInstanceState((IParcelable)bundle.GetParcelable("View"));
            }
            else
            {
                base.OnRestoreInstanceState(state);
            }

        }
    }

    public class SwitchTabViewEventArgs
    {
        public int Index { get; set; }
        public string TabName { get; set; }
    }

}
