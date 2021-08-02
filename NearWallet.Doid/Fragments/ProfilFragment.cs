
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;


using Android.OS;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;

using AndroidX.Preference;

using NearWallet.Doid.Views;
using NearWallet.Doid.Activitys;


namespace NearWallet.Doid.Fragments
{
    public class ProfilFragment : BaseFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_profil, container, false);

            ((MainActivity)Activity).IsEnabledRefresh = false;

            return view;
        }

        #region Events
        private void ProfilFragment_Click(object sender, EventArgs e)
        {
            switch (((View)sender).Id)
            {
                case Resource.Id.rlt_delete_wallet:
                    break;
            }
        }

        private void OnRecentActivity_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(Activity, typeof(RecentsActivity)));
        }

        private void OnAuthorizationApps_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(Activity, typeof(AuthorizationAppsActivity)));
        }
        #endregion

        public override void OnResume()
        {
            base.OnResume();

            View.FindViewById<RelativeLayout>(Resource.Id.rlt_delete_wallet).Click += ProfilFragment_Click;

            View.FindViewById<WalletDataView>(Resource.Id.wallet_data_view).OnAuthorizationAppsClick += OnAuthorizationApps_Click;
            View.FindViewById<WalletDataView>(Resource.Id.wallet_data_view).OnRecentActivityClick += OnRecentActivity_Click;
        }

        public override void OnPause()
        {
            base.OnPause();

            View.FindViewById<RelativeLayout>(Resource.Id.rlt_delete_wallet).Click -= ProfilFragment_Click;

            View.FindViewById<WalletDataView>(Resource.Id.wallet_data_view).OnAuthorizationAppsClick -= OnAuthorizationApps_Click;
            View.FindViewById<WalletDataView>(Resource.Id.wallet_data_view).OnRecentActivityClick -= OnRecentActivity_Click;
        }

        public override void OnRefresh()
        {
            
        }
    }

}
