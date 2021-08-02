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

using NearWallet.Core;

using NearWallet.Doid.Views;
using NearWallet.Doid.Adapters;
using NearWallet.Doid.Views.Recycler.Decoration;
using NearWallet.Doid.Views.Recycler.SpanSizeLookup;

namespace NearWallet.Doid.Fragments
{
    public class NftFragment : BaseFragment
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.recycler_view, container, false);

            var adapter = new NftRecyclerAdapter();
            adapter.SetItems(Data.Nfts);


            var layoutManager = new GridLayoutManager(Activity, 2);
            layoutManager.SetSpanSizeLookup(new NftSpanSizeLookup(adapter));

            var space = (int)Resources.GetDimension(Resource.Dimension.padding_max);

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.rc_view);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetAdapter(adapter);
            recyclerView.AddItemDecoration(new NftDecoration(space, adapter));

            return view;
        }

        public override void OnRefresh()
        {
            
        }

    }

}
