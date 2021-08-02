
using System;
using System.Collections.Generic;

using Android;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Runtime;
using Android.Content;
using Android.Content.PM;

using AndroidX.RecyclerView.Widget;

using Google.Android.Material.Button;
using Google.Android.Material.TextView;

using ToolbarX = AndroidX.AppCompat.Widget.Toolbar;

using NearWallet.Doid.Adapters;
using NearWallet.Doid.Views.Recycler.Decoration;

using NearWallet.Core;

using Com.Karumi.Dexter;
using Com.Karumi.Dexter.Listener;
using Com.Karumi.Dexter.Listener.Multi;

using Plugin.Permissions;

namespace NearWallet.Doid.Activitys
{
    [Activity(Label = "ListwalletsActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait,
        // настройки акитивити чтоб не перезагружалась при повороте эерана
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class ListWalletsActivity : BaseActivity, IMultiplePermissionsListener
    {
        private bool isHomeBack;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_list_wallets);

            SetSupportActionBar(FindViewById<ToolbarX>(Resource.Id.toolbar));

            FindViewById<MaterialTextView>(Resource.Id.toolbarTitle).Text = Resources.GetString(Resource.String.my_wallets);
            FindViewById<MaterialTextView>(Resource.Id.toolbarSubTitle).Visibility = ViewStates.Gone;
 
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            var adapter = new WalletRecyclerAdapter();
            adapter.SetItems(Data.Wallets);
            adapter.OnItemClick += Adapter_OnItemClick;

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.rc_view);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            recyclerView.SetAdapter(adapter);
            recyclerView.AddItemDecoration(new BaseItemDekoration(adapter));

            FindViewById<MaterialButton>(Resource.Id.btn_add_wallet).Click += AddWallet_Click;

            CheckPermissionLocation();
        }

        private void Adapter_OnItemClick(object sender, Core.Models.WalletModel e)
        {
            StartActivity(new Intent(this, typeof(MainActivity)));
        }

        private void AddWallet_Click(object sender, EventArgs e)
        {
            var index = 0;
            var alert = new AndroidX.AppCompat.App.AlertDialog.Builder(this, Resource.Style.AlertDialogStyle);
            alert.SetCancelable(false);
            alert.SetTitle(Resources.GetString(Resource.String.add_wallet_message));

            var arrayAdapter = new ArrayAdapter<string>(this, Resource.Layout.select_dialog_singlechoice_material);
            arrayAdapter.Add(Resources.GetString(Resource.String.create_wallet));
            arrayAdapter.Add(Resources.GetString(Resource.String.import_wallet));

            alert.SetSingleChoiceItems(arrayAdapter, index, (s, e) => { index = e.Which; });

            alert.SetPositiveButton(Resource.String.ok, (s, e) =>
            {
                StartActivity(new Intent(this, index == 0 ? typeof(LoginPasswordActivity) : typeof(MainActivity)));
            });
            alert.SetNegativeButton(Resource.String.cancel, (s, e) => { });
            alert.Show();
        }

        private void CheckPermissionLocation()
        {
            Dexter.WithActivity(this).WithPermissions(
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation,
                Manifest.Permission.Camera,
                Manifest.Permission.WriteExternalStorage).
                WithListener(this).
                WithErrorListener(new ErrorPermissionListener()).
                Check();
        }

        public override void OnBackPressed()
        {
            if (isHomeBack)
            {
                var startMain = new Intent(Intent.ActionMain);
                startMain.AddCategory(Intent.CategoryHome);
                StartActivity(startMain);
                return;
            }

            isHomeBack = true;
            SplashScreen.GetToast(this, Resources.GetString(Resource.String.exit_message));

            new Handler().PostDelayed(() => { isHomeBack = false; }, 1000);
        }

        public void OnPermissionRationaleShouldBeShown(IList<PermissionRequest> p0, IPermissionToken token)
        {
            token.ContinuePermissionRequest();
        }

        public void OnPermissionsChecked(MultiplePermissionsReport p0) { }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class ErrorPermissionListener : Java.Lang.Object, IPermissionRequestErrorListener
    {
        public void OnError(DexterError p0)
        {

        }
    }

}
