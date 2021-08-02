
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using NearWallet.Doid.Views;

namespace NearWallet.Doid.Fragments
{
    public class DashboardFragment : BaseFragment
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_nft, container, false);

            view.FindViewById<TextView>(Resource.Id.txt_title).Text = "Dashboard";

            return view;
        }

        public override void OnRefresh()
        {
            
        }

    }

}
