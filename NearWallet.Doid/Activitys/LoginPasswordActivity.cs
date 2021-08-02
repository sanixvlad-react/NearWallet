
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

using Google.Android.Material.TextView;

namespace NearWallet.Doid.Activitys
{
    [Activity(Label = "LoginPasswordActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait,
        // настройки акитивити чтоб не перезагружалась при повороте эерана
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class LoginPasswordActivity : BaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login_password);

            FindViewById<MaterialTextView>(Resource.Id.toolbarTitle).Text = Resources.GetString(Resource.String.create_wallet_title);
            FindViewById<MaterialTextView>(Resource.Id.toolbarSubTitle).Visibility = ViewStates.Gone;

            SetSupportActionBar(FindViewById<ToolbarX>(Resource.Id.toolbar));

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_left);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
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
