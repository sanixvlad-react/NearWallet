
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
using Android.Content.PM;

using ToolbarX = AndroidX.AppCompat.Widget.Toolbar;

using AndroidX.RecyclerView.Widget;

using NearWallet.Doid.Adapters;

using NearWallet.Core;
using AndroidX.SwipeRefreshLayout.Widget;
using Google.Android.Material.TextView;

namespace NearWallet.Doid.Activitys
{
    [Activity(Label = "EditTokensActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait,
        // настройки акитивити чтоб не перезагружалась при повороте эерана
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class EditTokensActivity : BaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_recycler_view);

            FindViewById<MaterialTextView>(Resource.Id.toolbarTitle).Text = Resources.GetString(Resource.String.edit_token_list);
            FindViewById<MaterialTextView>(Resource.Id.toolbarSubTitle).Visibility = ViewStates.Gone;

            SetSupportActionBar(FindViewById<ToolbarX>(Resource.Id.toolbar));

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_left);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var adapter = new CoinRecyclerAdapter(true);
            adapter.SetItems(Data.AllCoins);

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.rc_view);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            recyclerView.SetAdapter(adapter);

            FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh).Enabled = false;
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

    }

}
