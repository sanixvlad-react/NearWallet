
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Runtime;
using Android.Content;
using Android.Graphics;
using Android.Content.PM;
using Android.Graphics.Drawables;

using AndroidX.SwipeRefreshLayout.Widget;

using SwipeMenuListView = SwipeListView.SwipeListView;
using ToolbarX = AndroidX.AppCompat.Widget.Toolbar;

using NearWallet.Doid.Adapters;

using NearWallet.Core;

using SwipeListView;
using System.Threading.Tasks;
using Google.Android.Material.TextView;

namespace NearWallet.Doid.Activitys
{
    [Activity(Label = "AuthorizationAppsActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait,
        // настройки акитивити чтоб не перезагружалась при повороте эерана
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class AuthorizationAppsActivity : BaseActivity, AbsListView.IOnScrollListener,
                 ISwipeMenuCreator, IOnMenuItemClickListener, IOnSwipeListener, SwipeRefreshLayout.IOnRefreshListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_authorization_apps);

            FindViewById<MaterialTextView>(Resource.Id.toolbarTitle).Text = Resources.GetString(Resource.String.authorized_apps);
            FindViewById<MaterialTextView>(Resource.Id.toolbarSubTitle).Visibility = ViewStates.Gone;

            SetSupportActionBar(FindViewById<ToolbarX>(Resource.Id.toolbar));

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_left);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var listView = FindViewById<SwipeMenuListView>(Resource.Id.swipe_list_view);
            listView.SetOnScrollListener(this);
            listView.MenuCreator = this;
            listView.MenuItemClickListener = this;
            listView.SwipeListener = this;
            listView.Adapter = new AuthorizationAppAdapter(this, Data.AuthorizationApps);
            listView.SetSelector(Android.Resource.Color.Transparent);

            FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh).SetOnRefreshListener(this);
        }

        /// <summary>
        /// SwipeRefreshLayout.IOnRefreshListener
        /// </summary>
        public async void OnRefresh()
        {
            await Task.Delay(800);

            FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh).Refreshing = false;
        }

        #region IOnSwipeListener
        public void OnSwipeStart(int position)
        {
            FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh).Enabled = false;
        }

        public void OnSwipeEnd(int position)
        {
            FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh).Enabled = true;
        }
        #endregion

        /// <summary>
        /// IOnMenuItemClickListener
        /// </summary>
        /// <param name="position"></param>
        /// <param name="menu"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool OnMenuItemClick(int position, SwipeMenu menu, int index)
        {
            ((AuthorizationAppAdapter)FindViewById<SwipeMenuListView>(Resource.Id.swipe_list_view).Adapter).RemoveItem(position);
            return true;
        }

        /// <summary>
        /// ISwipeMenuCreator
        /// </summary>
        /// <param name="menu"></param>
        public void Create(SwipeMenu menu)
        {
            var callItem = new SwipeMenuItem(this)
            {
                Width = 230,
                Background = new ColorDrawable(Color.Red),
                IconRes = Resource.Drawable.ic_delete
            };
            menu.AddMenuItem(callItem);
        }

        #region list scroll
        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
            
        }
        #endregion

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

        
    }

}
