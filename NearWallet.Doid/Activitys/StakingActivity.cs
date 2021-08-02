using System;
using System.Timers;
using System.Threading.Tasks;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Content.PM;

using AndroidX.SwipeRefreshLayout.Widget;

using ToolbarX = AndroidX.AppCompat.Widget.Toolbar;

using Google.Android.Material.TextView;

using NearWallet.Doid.Fragments;
using NearWallet.Doid.Framework;
using NearWallet.Doid.Framework.Helpers;

using NearWallet.Core.Models;

using MaterialSearchView;
using MaterialSearchView.Utils;
using static MaterialSearchView.SanixMaterialSearchView;

using Newtonsoft.Json;


namespace NearWallet.Doid.Activitys
{
    [Activity(Label = "StakingActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait,
        // настройки акитивити чтоб не перезагружалась при повороте эерана
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class StakingActivity : BaseActivity, SwipeRefreshLayout.IOnRefreshListener
    {
        public ValidatorModel ValidatorModel;

        private Timer timer = new Timer();

        private BaseFragment currentFragment;
        private BaseFragment CurrentFragment
        {
            get => currentFragment;
            set
            {
                currentFragment = value;

                SetTitleFragment(value);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_staking);

            SetSupportActionBar(FindViewById<ToolbarX>(Resource.Id.toolbar));

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_left);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var validatorJson = Intent.GetStringExtra(DroidConstString.IntentStakedValidatorModel);
            if (!string.IsNullOrEmpty(validatorJson))
                ValidatorModel = JsonConvert.DeserializeObject<ValidatorModel>(validatorJson);

            var fragmentType = (StakedFragmentType)Intent.GetIntExtra(DroidConstString.IntentStakedType, 0);
            ShowFragmentByType(fragmentType);
        }

        private void ShowFragmentByType(StakedFragmentType fragmentType)
        {
            switch(fragmentType)
            {
                case StakedFragmentType.AllValidators:
                    ShowFragment(new ValidatorsFragment(fragmentType), true);
                    break;
                case StakedFragmentType.Validator:
                    ShowFragment(new ValidatorFragment(ValidatorModel), true);
                    break;
                case StakedFragmentType.YourValidators:
                    ShowFragment(new ValidatorsFragment(fragmentType), true);
                    break;
            }
        }

        public void ShowFragment(BaseFragment fragment, bool isFirst)
        {
            var transaction = SupportFragmentManager.BeginTransaction();

            if (isFirst)
                transaction.SetCustomAnimations(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            else
                transaction.SetCustomAnimations(Resource.Animation.side_in_right, Resource.Animation.side_out_left,
                    Resource.Animation.side_in_left, Resource.Animation.side_out_right);

            transaction.Replace(Resource.Id.fragment_container, fragment, fragment.ToString());
            transaction.AddToBackStack(fragment.ToString());
            transaction.CommitAllowingStateLoss();

            CurrentFragment = fragment;

            InvalidateOptionsMenu();
        }

        #region title activity

        private void SetTitleFragment(BaseFragment fragment)
        {
            var title = GetTitleByFragment(fragment);
            var subtitle = GetSuntitleByFragment(fragment);

            FindViewById<MaterialTextView>(Resource.Id.toolbarTitle).Text = title;

            var subTitleTxt = FindViewById<MaterialTextView>(Resource.Id.toolbarSubTitle);
            subTitleTxt.Visibility = string.IsNullOrEmpty(subtitle) ? ViewStates.Gone : ViewStates.Visible;
            subTitleTxt.Text = subtitle;
        }

        private string GetSuntitleByFragment(BaseFragment fragment)
        {
            if (fragment is ValidatorFragment)
                return ValidatorModel?.Name;

            return string.Empty;
        }

        private string GetTitleByFragment(BaseFragment fragment)
        {
            if (fragment is ValidatorFragment)
                return Resources.GetString(Resource.String.validator);

            if (fragment is ValidatorsFragment)
                if((fragment as ValidatorsFragment).StakedFragmentType == StakedFragmentType.AllValidators)
                    return Resources.GetString(Resource.String.select_validator);
                else
                    return Resources.GetString(Resource.String.your_current_validators);

            return string.Empty;
        }

        #endregion

        #region Search
        private void SetupSearchView(IMenu menu)
        {
            try
            {
                var searchView = FindViewById<SanixMaterialSearchView>(Resource.Id.searchView);
                var item = menu.FindItem(Resource.Id.menu_search);
                searchView.SetMenuItem(item);

                Func<string, bool> onQueryTextChange = OnQueryTextChange;
                Func<string, bool> onQueryTextSubmit = OnQueryTextSubmit;
                Func<bool> onQueryTextCleared = OnQueryTextCleared;
                var ty = new QueryTextListener(onQueryTextChange, onQueryTextSubmit, onQueryTextCleared);
                searchView.SetOnQueryTextListener(ty);

                var revealCenter = searchView.GetRevealAnimationCenter();
                revealCenter.X -= DimensUtils.ConvertDpToPx(20, this);
            }
            catch
            {

            }
        }

        private bool OnQueryTextChange(string newText)
        {
            timer.Stop();

            if (!string.IsNullOrEmpty(newText))
            {
                RunOnUiThread(() =>
                {
                    //((BusFragment)CurrentFragment).RefreshLayout.Refreshing = true;
                });
            }

            timer = new Timer(1500);
            timer.Elapsed += delegate
            {
                RunOnUiThread(() =>
                {
                    //((BusFragment)CurrentFragment).SetSearchFilter(newText);
                });
                timer.Stop();
            };
            timer.Start();

            return false;
        }

        private bool OnQueryTextSubmit(string query) => false;

        public bool OnQueryTextCleared() => false;
        #endregion

        public async void OnRefresh()
        {
            await Task.Delay(800);

            FindViewById<SwipeRefreshLayout>(Resource.Id.refresh_layout).Refreshing = false;
        }

        #region Ovveride

        public override void OnBackPressed()
        {
            if (SupportFragmentManager.BackStackEntryCount == 1)
            {
                Finish();
            }
            else
            {
                base.OnBackPressed();

                InvalidateOptionsMenu();

                CurrentFragment = (BaseFragment)SupportFragmentManager.Fragments[SupportFragmentManager.BackStackEntryCount - 1];
            }
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            menu.Clear();

            if (CurrentFragment is ValidatorsFragment)
            {
                MenuInflater.Inflate(Resource.Menu.search_menu, menu);
                SetupSearchView(menu);
            }
            else
                MenuInflater.Inflate(Resource.Menu.empty_menu, menu);

            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion

    }

}
