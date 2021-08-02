using System;
using System.Threading.Tasks;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Content.PM;

using AndroidX.CoordinatorLayout.Widget;
using AndroidX.SwipeRefreshLayout.Widget;

using Google.Android.Material.AppBar;
using Google.Android.Material.BottomNavigation;

using ToolbarX = AndroidX.AppCompat.Widget.Toolbar;
using FragmentX = AndroidX.Fragment.App.Fragment;

using NearWallet.Doid.Views;
using NearWallet.Doid.Activitys;
using NearWallet.Doid.Fragments;
using NearWallet.Doid.Views.Dialogs;
using NearWallet.Doid.Views.Behavior;
using NearWallet.Doid.Framework.Helpers;

using SwitchTab;

namespace NearWallet.Doid
{
    [Activity(Label = "MainActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait,
        // настройки акитивити чтоб не перезагружалась при повороте эерана
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : BaseActivity, SwipeRefreshLayout.IOnRefreshListener
    {
        private bool isAnimatedHeader = true;

        public bool IsEnabledRefresh
        {
            get
            {
                var refreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.refresh_layout);
                return refreshLayout == null? false: refreshLayout.Enabled;
            }
            set
            {
                var refreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.refresh_layout);
                if (refreshLayout == null) return;
                refreshLayout.Enabled = value;
            }
        }

        #region CurrentFragment
        private static FragmentX currentFragment;
        public FragmentX CurrentFragment
        {
            get { return currentFragment; }
            set
            {
                currentFragment = value;

                var appBar = FindViewById<AppBarLayout>(Resource.Id.appBarLayout);
                appBar.SetExpanded(currentFragment is CoinFragment, isAnimatedHeader);
                isAnimatedHeader = true;

                var layoutParams = (CoordinatorLayout.LayoutParams)appBar.LayoutParameters;
                ((DisableableAppBarLayoutBehavior)layoutParams.Behavior).IsEnabled = currentFragment is CoinFragment;

                FindViewById<SwitchTabView>(Resource.Id.sw_tab_tokens).Visibility =
                            (value is CoinFragment || value is NftFragment) ?ViewStates.Visible : ViewStates.Gone;
            }
        }
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);

            SetSupportActionBar(FindViewById<ToolbarX>(Resource.Id.toolbar));

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_left);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            FindViewById<SwitchTabView>(Resource.Id.sw_tab_tokens).SwitchChanged += MainActivity_SwitchChanged;

            FindViewById<SwipeRefreshLayout>(Resource.Id.refresh_layout).SetOnRefreshListener(this);

            var headerData = FindViewById<WalletDataHeaderView>(Resource.Id.lt_header_operations);
            headerData.OnBuyClick += HeaderData_OnBuyClick;
            headerData.OnSendClick += HeaderData_OnSendClick;
            headerData.OnReceiveClick += HeaderData_OnReceiveClick;

            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            if (CurrentFragment == null)
                bottomNavigation.SelectedItemId = Resource.Id.action_token;
            else
            {
                isAnimatedHeader = false;
                ShowFragment(new ProfilFragment(), Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
                bottomNavigation.SelectedItemId = Resource.Id.action_profil;
            }

            OnRefresh();
        }

        private void ShowFragment(FragmentX fragment, int enterAnim = -1, int exitAnim = -1)
        {
            var transaction = SupportFragmentManager.BeginTransaction();

            SupportFragmentManager.Fragments.Clear();

            if (enterAnim != -1 && exitAnim != -1)
                transaction.SetCustomAnimations(enterAnim, exitAnim);

            transaction.Replace(Resource.Id.fragment_container, fragment, fragment.ToString() + DateTime.Now.Ticks);
            transaction.DisallowAddToBackStack();
            transaction.CommitAllowingStateLoss();

            CurrentFragment = fragment;

            InvalidateOptionsMenu();
        }

        #region Events

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.action_token:
                    if (CurrentFragment is CoinFragment || CurrentFragment is NftFragment && CurrentFragment != null) return;

                    FindViewById<SwitchTabView>(Resource.Id.sw_tab_tokens).SetSelectedTab(0);
                    ShowFragment(new CoinFragment(), Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
                    break;
                case Resource.Id.action_staking:
                    if (CurrentFragment is StakingFragment && CurrentFragment != null) return;

                    ShowFragment(new StakingFragment(), Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
                    break;
                //case Resource.Id.action_dashboard:
                //if (CurrentFragment is DashboardFragment && CurrentFragment != null) return;

                //ShowFragment(new DashboardFragment(), Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
                //break;
                case Resource.Id.action_map:
                    if (CurrentFragment is MapFragment && CurrentFragment != null) return;

                    ShowFragment(new MapFragment(), Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
                    break;
                case Resource.Id.action_profil:
                    if (CurrentFragment is ProfilFragment && CurrentFragment != null) return;

                    ShowFragment(new ProfilFragment(), Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
                    break;
            }
        }

        private void MainActivity_SwitchChanged(object sender, SwitchTabViewEventArgs e)
        {
            var enterAnim = AnimationHelper.GetEnterAnimation(CurrentFragment, e.TabName);
            var exitAnim = AnimationHelper.GetExitAnimation(CurrentFragment, e.TabName);

            BaseFragment fragment = null;
            if (e.TabName == Resources.GetString(Resource.String.balances))
                fragment = new CoinFragment();

            if (e.TabName == Resources.GetString(Resource.String.collectibles))
                fragment = new NftFragment();

            if (fragment == null) return;

            ShowFragment(fragment, enterAnim, exitAnim);
        }

        private void HeaderData_OnSendClick(object sender, EventArgs e)
        {
            new SendCoinBottomSheet().ShowNow(SupportFragmentManager, "receive_coin_bottom_sheet");
        }

        private void HeaderData_OnBuyClick(object sender, EventArgs e)
        {
            new BuyCoinBottomSheet().ShowNow(SupportFragmentManager, "buy_coin_bottom_sheet");
        }

        private void HeaderData_OnReceiveClick(object sender, EventArgs e)
        {
            new ReceiveCoinBottomSheet("sanix.near").ShowNow(SupportFragmentManager, "receive_coin_bottom_sheet");
        }

        #endregion

        #region override medthods

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            menu.Clear();

            if(CurrentFragment is CoinFragment)
                MenuInflater.Inflate(Resource.Menu.add_menu, menu);

            if (CurrentFragment is NftFragment)
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

                case Resource.Id.menu_add_token:
                    StartActivity(new Intent(this, typeof(EditTokensActivity)));
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public async void OnRefresh()
        {
            FindViewById<SwipeRefreshLayout>(Resource.Id.refresh_layout).Refreshing = true;

            var header = FindViewById<WalletDataHeaderView>(Resource.Id.lt_header_operations);
            header.SetLoaded(true);

            await Task.Delay(800);

            header.SetLoaded(false);
            FindViewById<SwipeRefreshLayout>(Resource.Id.refresh_layout).Refreshing = false;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion


    }

}
