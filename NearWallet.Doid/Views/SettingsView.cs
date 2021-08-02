using System;

using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Graphics;

using AndroidX.AppCompat.App;

using NearWallet.Doid.Framework.Helpers;

namespace NearWallet.Doid.Views
{
    [Register("ru.sanix.SettingsView")]
    public class SettingsView : LinearLayout
    {
        private int animatinDuration = 500;
        private bool isOpenSettingLayout { get; set; } = false;
        private int settingLayoutHeight;

        public SettingsView(Context context) : base(context) { Init(context, null); }

        public SettingsView(Context context, IAttributeSet attrs = null) : base(context, attrs, 0) { Init(context, attrs); }

        public SettingsView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(context, attrs); }

        private void Init(Context context, IAttributeSet attrs)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.view_settings, this, true);

            FindViewById<RelativeLayout>(Resource.Id.rlt_settings_language).Click += ChangeLanguage_Click;
            FindViewById<RelativeLayout>(Resource.Id.rlt_setting_theme).Click += ChangeTheme_Click;
            FindViewById<RelativeLayout>(Resource.Id.rlt_help).Click += Help_Click;
            FindViewById<RelativeLayout>(Resource.Id.rlt_settings).Click += ExpandView_Click;

            SetLanguageData();

            SetThemeData();

            FindViewById<TextView>(Resource.Id.txt_settings_app_version).Text =
                        Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName;

            SetWillNotDraw(false);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (settingLayoutHeight != 0) return;

            var settingLayoutView = FindViewById<LinearLayout>(Resource.Id.lt_settings);
            settingLayoutHeight = settingLayoutView.Height;

            AnimateLayout(settingLayoutView, isOpenSettingLayout);
        }

        private void AnimateLayout(View view, bool isOpen, int duration = -1)
        {
            var height = isOpen ? settingLayoutHeight : 0;

            ViewColapseAnimation(view, height, duration);
        }

        private void ViewColapseAnimation(View view, int colapseHeight, int duration)
        {
            var animation = new HeightAnimation(view, colapseHeight);
            animation.Duration = duration == -1 ? 0 : duration;
            view.StartAnimation(animation);
        }

        #region Theme
        private void SetThemeData()
        {
            var theme = GetThemeByNightMode(PreferencesHelper.GetAppTheme(Context));
            FindViewById<TextView>(Resource.Id.txt_settings_theme).Text = theme;
        }

        private string GetThemeByNightMode(int defaultNightMode)
        {
            switch (defaultNightMode)
            {
                case AppCompatDelegate.ModeNightYes: return Resources.GetString(Resource.String.dark);
                case AppCompatDelegate.ModeNightNo: return Resources.GetString(Resource.String.light);
                default: case AppCompatDelegate.ModeNightFollowSystem: return Resources.GetString(Resource.String.system);
            }
        }

        private int GetThemeModeByIndex(int index)
        {
            switch (index)
            {
                case 2: return AppCompatDelegate.ModeNightYes;
                case 1: return AppCompatDelegate.ModeNightNo;
                default: return AppCompatDelegate.ModeNightFollowSystem;
            }
        }

        private int GetIndexTheme()
        {
            switch (PreferencesHelper.GetAppTheme(Context))
            {
                case AppCompatDelegate.ModeNightYes: return 2;
                case AppCompatDelegate.ModeNightNo: return 1;
                default: return 0;
            }
        }
        #endregion

        #region Language
        private void SetLanguageData()
        {
            var locale = PreferencesHelper.GetAppLocale(Context);
            var language = GetLanguageByLocale(locale);
            FindViewById<TextView>(Resource.Id.txt_settings_language).Text = language;
        }

        private string GetLanguageByLocale(string locale)
        {
            switch (locale)
            {
                case PreferencesHelper.RuLocale: return Resources.GetString(Resource.String.russian);
                case PreferencesHelper.EnLocale:
                default: return Resources.GetString(Resource.String.english);
            }
        }
        #endregion

        #region Events
        private void ExpandView_Click(object sender, EventArgs e)
        {
            isOpenSettingLayout = !isOpenSettingLayout;
            FindViewById<ImageView>(Resource.Id.img_expand).SetImageResource(isOpenSettingLayout ?
                                                            Resource.Drawable.ic_chevrone_up :
                                                            Resource.Drawable.ic_chevrone_down);

            AnimateLayout(FindViewById<LinearLayout>(Resource.Id.lt_settings), isOpenSettingLayout, animatinDuration);
        }

        private void ChangeTheme_Click(object sender, EventArgs e)
        {
            var index = GetIndexTheme();
            var builderSingle = new AndroidX.AppCompat.App.AlertDialog.Builder(Context, Resource.Style.AlertDialogStyle);
            builderSingle.SetTitle(Resources.GetString(Resource.String.choose_theme));

            var arrayAdapter = new ArrayAdapter<string>(Context, Resource.Layout.select_dialog_singlechoice_material);
            arrayAdapter.Add(Resources.GetString(Resource.String.system));
            arrayAdapter.Add(Resources.GetString(Resource.String.light));
            arrayAdapter.Add(Resources.GetString(Resource.String.dark));

            builderSingle.SetSingleChoiceItems(arrayAdapter, index, (s, e) => { index = e.Which; });

            builderSingle.SetPositiveButton(Resource.String.ok, (s, e) =>
            {
                PreferencesHelper.SetAppTheme(Context, GetThemeModeByIndex(index));

                SetThemeData();
            });
            builderSingle.SetNegativeButton(Resource.String.cancel, (s, e) => { });
            builderSingle.Show();
        }

        private void ChangeLanguage_Click(object sender, EventArgs e)
        {
            var locale = PreferencesHelper.GetAppLocale(Context);
            var index = locale == PreferencesHelper.EnLocale ? 0 : 1;
            var builderSingle = new AndroidX.AppCompat.App.AlertDialog.Builder(Context, Resource.Style.AlertDialogStyle);
            builderSingle.SetTitle(Resources.GetString(Resource.String.choose_language));

            var arrayAdapter = new ArrayAdapter<string>(Context, Resource.Layout.select_dialog_singlechoice_material);
            arrayAdapter.Add(Resources.GetString(Resource.String.english));
            arrayAdapter.Add(Resources.GetString(Resource.String.russian));

            builderSingle.SetSingleChoiceItems(arrayAdapter, index, (s, e) => { index = e.Which; });

            builderSingle.SetPositiveButton(Resource.String.ok, (s, e) =>
            {
                var locale = (index == 0) ? PreferencesHelper.EnLocale : PreferencesHelper.RuLocale;
                PreferencesHelper.SetAppLocale(Context, locale);
            });
            builderSingle.SetNegativeButton(Resource.String.cancel, (s, e) => { });
            builderSingle.Show();
        }

        private void Help_Click(object sender, EventArgs e)
        {
            var browserIntent = new Intent(Intent.ActionView,
                        Android.Net.Uri.Parse("https://nearhelp.zendesk.com/hc/en-us"));
            Context.StartActivity(browserIntent);
        }
        #endregion

    }

}
