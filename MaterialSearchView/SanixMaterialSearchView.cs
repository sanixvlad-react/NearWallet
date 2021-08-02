using System;
using System.Linq;

using Android.OS;
using Android.App;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Graphics;
using Android.Content.Res;
using Android.Graphics.Drawables;

using Java.Lang;

using MaterialSearchView.Utils;

using AndroidX.Annotations;
using AndroidX.Core.Widget;
using AndroidX.Core.Content;

using Google.Android.Material.Tabs;

namespace MaterialSearchView
{
    [Register("ru.sanix.MaterialSearchView")]
    public sealed class SanixMaterialSearchView : FrameLayout
    {
        private Color backgroundColor;
        private string _voiceSearchPrompt = "";
        public const int RequestVoiceSearch = 735;
        public const int CardCornerRadius = 4;
        public const int AnimationCenterPadding = 26;
        private const int CardPadding = 6;
        private const int CardElevation = 2;
        private const float BackIconAlphaDefault = 0.87f;
        private const float IconsAlphaDefault = 0.54f;
        public const int StyleBar = 0;
        public const int StyleCard = 1;
        private readonly Context _context;
        private Point _revealAnimationCenter;
        private ICharSequence _query;
        private bool _allowVoiceSearch;
        private bool _isClearingFocus;
        private int _style;
        private ViewGroup _searchContainer;
        private EditText _searchEditText;
        private ImageButton _backButton;
        private ImageButton _clearButton;
        private ImageButton _voiceButton;
        private View _bottomLine;
        private TabLayout _tabLayout;
        private int _tabLayoutInitialHeight;
        private IOnQueryTextListener _onQueryChangeListener;
        private ISearchViewListener _searchViewListener;

        private SanixMaterialSearchView(IntPtr javaReference, JniHandleOwnership transfer): base(javaReference, transfer){ }

        public SanixMaterialSearchView(Context context): this(context, null){}

        public SanixMaterialSearchView(Context context, IAttributeSet attrs): this(context, attrs, 0){ }

        public SanixMaterialSearchView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            _context = context;
            Inflate();
            InitStyle(attrs, defStyleAttr);
            InitSearchEditText();
            InitClickListeners();
            ShowVoice(true);
            if (IsInEditMode)
                return;
            Visibility = ViewStates.Invisible;
        }

        private void Inflate()
        {
            LayoutInflater.From(_context).Inflate(Resource.Layout.search_view, this, true);
            _searchContainer = FindViewById<ViewGroup>(Resource.Id.searchContainer);
            _searchEditText = FindViewById<EditText>(Resource.Id.searchEditText);
            _backButton = FindViewById<ImageButton>(Resource.Id.buttonBack);
            _clearButton = FindViewById<ImageButton>(Resource.Id.buttonClear);
            _voiceButton = FindViewById<ImageButton>(Resource.Id.buttonVoice);
            _bottomLine = FindViewById<View>(Resource.Id.bottomLine);
        }

        private void InitStyle(IAttributeSet attrs, int defStyleAttr)
        {
            TypedArray styledAttributes = _context.ObtainStyledAttributes(attrs, Resource.Styleable.SimpleSearchView, defStyleAttr, 0);

            if (styledAttributes == null)
            {
                SetCardStyle(_style);
            }
            else
            {
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_type))
                    SetCardStyle(styledAttributes.GetInt(Resource.Styleable.SimpleSearchView_type, _style));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_backIconAlpha))
                    SetBackIconAlpha(styledAttributes.GetFloat(Resource.Styleable.SimpleSearchView_backIconAlpha, 0.87f));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_iconsAlpha))
                    SetIconsAlpha(styledAttributes.GetFloat(Resource.Styleable.SimpleSearchView_iconsAlpha, 0.54f));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_backIconTint))
                    SetBackIconColor(styledAttributes.GetColor(Resource.Styleable.SimpleSearchView_backIconTint, ContextUtils.GetPrimaryColor(_context)));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_iconsTint))
                    SetIconsColor(styledAttributes.GetColor(Resource.Styleable.SimpleSearchView_iconsTint, Color.Black));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_cursorColor))
                    SetCursorColor(styledAttributes.GetColor(Resource.Styleable.SimpleSearchView_cursorColor, ContextUtils.GetAccentColor(_context)));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_hintColor))
                    SetHintTextColor(styledAttributes.GetColor(Resource.Styleable.SimpleSearchView_hintColor, ContextCompat.GetColor(Context, Resource.Color.default_textColorHint)));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_searchBackground))
                    SetSearchBackground(styledAttributes.GetDrawable(Resource.Styleable.SimpleSearchView_searchBackground));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_searchBackIcon))
                    SetBackIconDrawable(styledAttributes.GetDrawable(Resource.Styleable.SimpleSearchView_searchBackIcon));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_searchClearIcon))
                    SetClearIconDrawable(styledAttributes.GetDrawable(Resource.Styleable.SimpleSearchView_searchClearIcon));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_searchVoiceIcon))
                    SetVoiceIconDrawable(styledAttributes.GetDrawable(Resource.Styleable.SimpleSearchView_searchVoiceIcon));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_voiceSearch))
                    EnableVoiceSearch(styledAttributes.GetBoolean(Resource.Styleable.SimpleSearchView_voiceSearch, _allowVoiceSearch));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_voiceSearchPrompt))
                    SetVoiceSearchPrompt(styledAttributes.GetString(Resource.Styleable.SimpleSearchView_voiceSearchPrompt));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_hint))
                    SetHint(styledAttributes.GetString(Resource.Styleable.SimpleSearchView_hint));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_inputType))
                    SetInputType(styledAttributes.GetInt(Resource.Styleable.SimpleSearchView_inputType, 524288));
                if (styledAttributes.HasValue(Resource.Styleable.SimpleSearchView_textColor))
                    SetTextColor(styledAttributes.GetColor(Resource.Styleable.SimpleSearchView_textColor, ContextCompat.GetColor(Context, Resource.Color.default_textColor)));
                styledAttributes.Recycle();
            }
        }

        private void InitSearchEditText()
        {
            _searchEditText.Click += (s, e) => 
            { 
                ContextUtils.ShowKeyboard(_searchEditText); 
            };
            _searchEditText.SetOnEditorActionListener(new MyTextViewOnEditorActionListener((v, actionId, @event) =>
            {
                OnSubmitQuery();
                return true;
            }));
            _searchEditText.AddTextChangedListener(new MyTextWatcher((s, start, count, after) => 
            {

            }, (s, start, before, count) => OnTextChanged(s), editable => 
            {

            }));
            _searchEditText.OnFocusChangeListener = new MyOnFocusChangeListener((v, hasFocus) =>
            {
                if (!hasFocus)
                    return;
                ContextUtils.ShowKeyboard(_searchEditText);
            });
        }

        private void InitClickListeners()
        {
            _backButton.SetOnClickListener(new MyViewOnClickListener(v => CloseSearch()));
            _clearButton.SetOnClickListener(new MyViewOnClickListener(v => ClearSearch()));
            _voiceButton.SetOnClickListener(new MyViewOnClickListener(v => VoiceSearch()));
        }

        public override void ClearFocus()
        {
            _isClearingFocus = true;
            ContextUtils.HideKeyboard(this);
            base.ClearFocus();
            _searchEditText.ClearFocus();
            _isClearingFocus = false;
        }

        public override bool RequestFocus(FocusSearchDirection direction, Rect previouslyFocusedRect)
        {
            if (_isClearingFocus || !Focusable)
                return false;
            return _searchEditText.RequestFocus(direction, previouslyFocusedRect);
        }

        protected override IParcelable OnSaveInstanceState()
        {
            return (IParcelable)new SavedState(base.OnSaveInstanceState())
            {
                Query = _query?.ToString(),
                IsSearchOpen = IsSearchOpen,
                AnimationDuration = AnimationDuration
            };
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            if (!(state is SavedState))
            {
                base.OnRestoreInstanceState(state);
            }
            else
            {
                SavedState savedState = (SavedState)state;
                if (savedState.IsSearchOpen)
                {
                    ShowSearch(false);
                    SetQuery(savedState.Query, false);
                }
                base.OnRestoreInstanceState(savedState.SuperState);
            }
        }

        private void VoiceSearch()
        {
            Activity activity = ContextUtils.ScanForActivity(_context);
            if (activity == null)
                return;
            Intent intent = new Intent("android.speech.action.RECOGNIZE_SPEECH");
            if (_voiceSearchPrompt != null && _voiceSearchPrompt.Length == 0)
                intent.PutExtra("android.speech.extra.PROMPT", _voiceSearchPrompt);
            intent.PutExtra("android.speech.extra.LANGUAGE_MODEL", "web_search");
            intent.PutExtra("android.speech.extra.MAX_RESULTS", 1);
            activity.StartActivityForResult(intent, 735);
        }

        private void ClearSearch()
        {
            _searchEditText.TextFormatted = null;
            _onQueryChangeListener?.OnQueryTextCleared();
        }

        private void OnTextChanged(ICharSequence newText)
        {
            _query = newText;
            if (!string.IsNullOrEmpty(newText.ToString()))
            {
                _clearButton.Visibility = ViewStates.Visible;
                ShowVoice(false);
            }
            else
            {
                _clearButton.Visibility = ViewStates.Gone;
                ShowVoice(true);
            }

            if (_onQueryChangeListener != null)
                _onQueryChangeListener.OnQueryTextChange(newText.ToString());
        }

        private void OnSubmitQuery()
        {
            var textFormatted = _searchEditText.TextFormatted;
            if (textFormatted == null || TextUtils.GetTrimmedLength(textFormatted) <= 0 ||
                _onQueryChangeListener != null && _onQueryChangeListener.OnQueryTextSubmit(textFormatted.ToString()))
                return;
        }

        private bool IsVoiceAvailable
        {
            get
            {
                if (IsInEditMode)
                    return true;
                return Context.PackageManager.QueryIntentActivities(new Intent("android.speech.action.RECOGNIZE_SPEECH"), 0).Any();
            }
        }

        public void ShowSearch()
        {
            ShowSearch(true);
        }

        public void ShowSearch(bool animate)
        {
            if (IsSearchOpen)
                return;
            _searchEditText.TextFormatted = null;
            _searchEditText.RequestFocus();
            if (animate)
                SimpleAnimationUtils.RevealOrFadeIn(this, AnimationDuration, new MySimpleAnimationListener(null, v =>
                {
                    _searchViewListener?.OnSearchViewShownAnimation();
                    return false;
                }, null), GetRevealAnimationCenter()).Start();
            else
                Visibility = ViewStates.Visible;
            HideTabLayout(animate);
            IsSearchOpen = true;
            _searchViewListener?.OnSearchViewShown();
        }

        public void CloseSearch()
        {
            CloseSearch(true);
        }

        public void CloseSearch(bool animate)
        {
            if (!IsSearchOpen)
                return;
            _searchEditText.TextFormatted = null;
            ClearFocus();
            if (animate)
                SimpleAnimationUtils.HideOrFadeOut(this, AnimationDuration, new MySimpleAnimationListener(null, v =>
                {
                    _searchViewListener?.OnSearchViewClosedAnimation();
                    return false;
                }, null), GetRevealAnimationCenter()).Start();
            else
                Visibility = ViewStates.Invisible;
            ShowTabLayout(animate);
            IsSearchOpen = false;
            _searchViewListener?.OnSearchViewClosed();
        }

        public TabLayout GetTabLayout()
        {
            return _tabLayout;
        }

        public void SetTabLayout(TabLayout tabLayout)
        {
            _tabLayout = tabLayout;
            MyVtoPreDrawListener l = new MyVtoPreDrawListener();
            l.PreDraw += () =>
            {
                _tabLayoutInitialHeight = tabLayout.Height;
                tabLayout.ViewTreeObserver.RemoveOnPreDrawListener(l);
                return true;
            };
            _tabLayout.ViewTreeObserver.AddOnPreDrawListener(l);
            _tabLayout.AddOnTabSelectedListener(new MyTabSelectedListener(tab => { }, tab => CloseSearch(), tab => { }));
        }

        public void ShowTabLayout()
        {
            ShowTabLayout(true);
        }

        public void ShowTabLayout(bool animate)
        {
            if (_tabLayout == null)
                return;
            if (animate)
                SimpleAnimationUtils.VerticalSlideView(_tabLayout, 0, _tabLayoutInitialHeight, AnimationDuration).Start();
            else
                _tabLayout.Visibility = ViewStates.Visible;
        }

        public void HideTabLayout()
        {
            HideTabLayout(true);
        }

        public void HideTabLayout(bool animate)
        {
            if (_tabLayout == null)
                return;
            if (animate)
                SimpleAnimationUtils.VerticalSlideView(_tabLayout, _tabLayout.Height, 0, AnimationDuration).Start();
            else
                _tabLayout.Visibility = ViewStates.Gone;
        }

        public bool OnBackPressed()
        {
            if (!IsSearchOpen)
                return false;
            CloseSearch();
            return true;
        }

        public bool OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            return OnActivityResult(requestCode, resultCode, data, true);
        }

        public bool OnActivityResult(int requestCode, int resultCode, Intent data, bool submit)
        {
            if (requestCode != 735 || resultCode != -1)
                return false;
            var stringArrayListExtra = data.GetStringArrayListExtra("android.speech.extra.RESULTS");
            if (stringArrayListExtra != null && stringArrayListExtra.Any())
            {
                string str = stringArrayListExtra[0];
                if (!TextUtils.IsEmpty(str))
                    SetQuery(new Java.Lang.String(str), submit);
            }
            return true;
        }

        public Style GetCardStyle()
        {
            return (Style)_style;
        }

        public void SetCardStyle(int style)
        {
            SetCardStyle((Style)style);
        }

        public void SetCardStyle(Style style)
        {
            _style = (int)style;
            var layoutParams = new LayoutParams(-1, -1);
            float num = 0.0f;
            if (style != Style.Bar && style == Style.Card)
            {
                _searchContainer.Background = GetCardStyleBackground();
                _bottomLine.Visibility = ViewStates.Gone;
                int px = DimensUtils.ConvertDpToPx(6, _context);
                layoutParams.SetMargins(px, px, px, px);
                num = DimensUtils.ConvertDpToPx(2, _context);
            }
            else
            {
                _bottomLine.Visibility = ViewStates.Visible;
            }
            _searchContainer.LayoutParameters = layoutParams;
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                return;
            _searchContainer.Elevation = num;
        }

        private GradientDrawable GetCardStyleBackground()
        {
            var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetCornerRadius(DimensUtils.ConvertDpToPx(4, _context));
            return gradientDrawable;
        }

        public void SetIconsAlpha(float alpha)
        {
            _clearButton.Alpha = alpha;
            _voiceButton.Alpha = alpha;
        }

        public void SetIconsColor([ColorInt] int color)
        {
            ImageViewCompat.SetImageTintList(_clearButton, ColorStateList.ValueOf(color.ToColor()));
            ImageViewCompat.SetImageTintList(_voiceButton, ColorStateList.ValueOf(color.ToColor()));
        }

        public void SetBackIconAlpha(float alpha)
        {
            _backButton.Alpha = alpha;
        }

        public void SetBackIconColor([ColorInt] int color)
        {
            ImageViewCompat.SetImageTintList(_backButton, ColorStateList.ValueOf(color.ToColor()));
        }

        public void SetBackIconDrawable(Drawable drawable)
        {
            _backButton.SetImageDrawable(drawable);
        }

        public void SetVoiceIconDrawable(Drawable drawable)
        {
            _voiceButton.SetImageDrawable(drawable);
        }

        public void SetClearIconDrawable(Drawable drawable)
        {
            _clearButton.SetImageDrawable(drawable);
        }

        public void SetSearchBackground(Drawable background)
        {
            _searchContainer.Background = background;
        }

        public void SetTextColor([ColorInt] int color)
        {
            _searchEditText.SetTextColor(color.ToColor());
        }

        public void SetHintTextColor([ColorInt] int color)
        {
            _searchEditText.SetHintTextColor(color.ToColor());
        }

        public void SetHint(string hint)
        {
            SetHint(new Java.Lang.String(hint));
        }

        public void SetHint(ICharSequence hint)
        {
            _searchEditText.HintFormatted = hint;
        }

        public void SetInputType(int inputType)
        {
            SetInputType((InputTypes)inputType);
        }

        public void SetInputType(InputTypes inputType)
        {
            _searchEditText.InputType = inputType;
        }

        public void SetCursorDrawable([DrawableRes] int drawable)
        {
            EditTextReflectionUtils.SetCursorDrawable(_searchEditText, drawable);
        }

        public void SetCursorColor([ColorInt] int color)
        {
            EditTextReflectionUtils.SetCursorColor(_searchEditText, color);
        }

        public void EnableVoiceSearch(bool voiceSearch)
        {
            _allowVoiceSearch = voiceSearch;
        }

        public EditText GetSearchEditText()
        {
            return _searchEditText;
        }

        public void SetQuery(string query, bool submit)
        {
            SetQuery(new Java.Lang.String(query), submit);
        }

        public void SetQuery(ICharSequence query, bool submit)
        {
            _searchEditText.TextFormatted = query;
            if (query != null)
            {
                _searchEditText.SetSelection(_searchEditText.Length());
                _query = query;
            }
            if (!submit || TextUtils.IsEmpty(query))
                return;
            OnSubmitQuery();
        }

        public void ShowVoice(bool show)
        {
            _voiceButton.Visibility = !show || !IsVoiceAvailable || !_allowVoiceSearch ? ViewStates.Gone : ViewStates.Visible;
        }

        public void SetMenuItem([NonNull] IMenuItem menuItem)
        {
            menuItem.SetOnMenuItemClickListener(new MyMenuItemOnMenuItemClickListener(item =>
            {
                ShowSearch();
                return true;
            }));
        }

        public bool IsSearchOpen { get; private set; }

        public int AnimationDuration { get; set; } = 250;

        public Point GetRevealAnimationCenter()
        {
            if (_revealAnimationCenter != null)
                return _revealAnimationCenter;
            _revealAnimationCenter = new Point(Width - DimensUtils.ConvertDpToPx(26, _context), Height / 2);
            return _revealAnimationCenter;
        }

        public void SetRevealAnimationCenter(Point revealAnimationCenter)
        {
            _revealAnimationCenter = revealAnimationCenter;
        }

        public void SetOnQueryTextListener(IOnQueryTextListener listener)
        {
            _onQueryChangeListener = listener;
        }

        public void SetOnSearchViewListener(ISearchViewListener listener)
        {
            _searchViewListener = listener;
        }

        public void SetVoiceSearchPrompt(string voiceSearchPrompt)
        {
            _voiceSearchPrompt = voiceSearchPrompt;
        }

        public enum Style
        {
            Bar,
            Card,
        }

        private sealed class SavedState : BaseSavedState
        {
            public string Query;
            public bool IsSearchOpen;
            public int AnimationDuration;
            public readonly string VoiceSearchPrompt;

            public SavedState(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

            public SavedState(IParcelable superState): base(superState) { }

            public SavedState(Parcel source) : base(source)
            {
                Query = source.ReadString();
                IsSearchOpen = source.ReadInt() == 1;
                AnimationDuration = source.ReadInt();
                VoiceSearchPrompt = source.ReadString();
            }

            public SavedState(Parcel source, ClassLoader loader) : base(source, loader) { }

            public override void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
            {
                base.WriteToParcel(dest, flags);
                dest.WriteString(Query);
                dest.WriteInt(IsSearchOpen ? 1 : 0);
                dest.WriteInt(AnimationDuration);
                dest.WriteString(VoiceSearchPrompt);
            }

            [Register("CREATOR")]
            public static MySavedStateCreator InitializeCreator()
            {
                return new MySavedStateCreator();
            }

            public sealed class MySavedStateCreator : Java.Lang.Object, IParcelableCreator, IJavaObject, IDisposable
            {
                public Java.Lang.Object CreateFromParcel(Parcel source)
                {
                    return new SavedState(source);
                }

                public Java.Lang.Object[] NewArray(int size)
                {
                    return new SavedState[size];
                }
            }

        }

        public interface IOnQueryTextListener
        {
            bool OnQueryTextSubmit(string query);

            bool OnQueryTextChange(string newText);

            bool OnQueryTextCleared();
        }

        public interface ISearchViewListener
        {
            void OnSearchViewShown();

            void OnSearchViewClosed();

            void OnSearchViewShownAnimation();

            void OnSearchViewClosedAnimation();
        }

        public sealed class SearchViewListener : ISearchViewListener
        {
            private readonly Action _actionOnSearchViewShown;
            private readonly Action _actionOnSearchViewShownAnimation;
            private readonly Action _actionOnSearchViewClosed;
            private readonly Action _actionOnSearchViewClosedAnimation;

            public SearchViewListener(Action actionOnSearchViewShown, Action actionOnSearchViewShownAnimation,
              Action actionOnSearchViewClosed, Action actionOnSearchViewClosedAnimation)
            {
                _actionOnSearchViewShown = actionOnSearchViewShown;
                _actionOnSearchViewShownAnimation = actionOnSearchViewShownAnimation;
                _actionOnSearchViewClosed = actionOnSearchViewClosed;
                _actionOnSearchViewClosedAnimation = actionOnSearchViewClosedAnimation;
            }

            public void OnSearchViewShown()
            {
                Action onSearchViewShown = _actionOnSearchViewShown;
                if (onSearchViewShown == null)
                    return;
                onSearchViewShown();
            }

            public void OnSearchViewShownAnimation()
            {
                Action viewShownAnimation = _actionOnSearchViewShownAnimation;
                if (viewShownAnimation == null)
                    return;
                viewShownAnimation();
            }

            public void OnSearchViewClosed()
            {
                Action searchViewClosed = _actionOnSearchViewClosed;
                if (searchViewClosed == null)
                    return;
                searchViewClosed();
            }

            public void OnSearchViewClosedAnimation()
            {
                Action viewClosedAnimation = _actionOnSearchViewClosedAnimation;
                if (viewClosedAnimation == null)
                    return;
                viewClosedAnimation();
            }
        }

        public sealed class QueryTextListener : IOnQueryTextListener
        {
            private readonly Func<string, bool> _funcOnQueryTextChange;
            private readonly Func<string, bool> _funcOnQueryTextSubmit;
            private readonly Func<bool> _funcOnQueryTextCleared;

            public QueryTextListener(Func<string, bool> funcOnQueryTextChange,
              Func<string, bool> funcOnQueryTextSubmit, Func<bool> funcOnQueryTextCleared)
            {
                _funcOnQueryTextChange = funcOnQueryTextChange;
                _funcOnQueryTextSubmit = funcOnQueryTextSubmit;
                _funcOnQueryTextCleared = funcOnQueryTextCleared;
            }

            public bool OnQueryTextChange(string newText)
            {
                if (_funcOnQueryTextChange != null)
                    return _funcOnQueryTextChange(newText);
                return false;
            }

            public bool OnQueryTextSubmit(string query)
            {
                if (_funcOnQueryTextSubmit != null)
                    return _funcOnQueryTextSubmit(query);
                return false;
            }

            public bool OnQueryTextCleared()
            {
                if (_funcOnQueryTextCleared != null)
                    return _funcOnQueryTextCleared();
                return false;
            }
        }

    }

}
