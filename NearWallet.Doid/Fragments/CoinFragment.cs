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
using AndroidX.RecyclerView.Widget;
using NearWallet.Doid.Views.Recycler.Decoration;

using NearWallet.Doid.Views;
using NearWallet.Doid.Adapters;
using NearWallet.Doid.Activitys;

using NearWallet.Core;


namespace NearWallet.Doid.Fragments
{
    public class CoinFragment : BaseFragment
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view  =  inflater.Inflate(Resource.Layout.recycler_view, container, false);

            ((MainActivity)Activity).IsEnabledRefresh = true;

            var adapter = new CoinRecyclerAdapter();
            adapter.SetItems(Data.AddedCoins);

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.rc_view);
            recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            recyclerView.SetAdapter(adapter);
            recyclerView.AddItemDecoration(new BaseItemDekoration(adapter));

            return view;
        }

        public override void OnRefresh()
        {
            
        }
    }

}
