
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

using NearWallet.Doid.Views;
using NearWallet.Doid.Adapters;
using NearWallet.Doid.Activitys;
using NearWallet.Doid.Framework;

using NearWallet.Core;


namespace NearWallet.Doid.Fragments
{
    public class ValidatorsFragment : BaseFragment
    {
        public StakedFragmentType StakedFragmentType;

        public ValidatorsFragment(StakedFragmentType stakedFragmentType) {StakedFragmentType = stakedFragmentType; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.recycler_view, container, false);

            var adapter = new ValidatorRecyclerAdapter();
            adapter.AddItems(StakedFragmentType == StakedFragmentType.AllValidators? Data.AllValidators : Data.MyValidators);
            adapter.OnItemClick += Adapter_OnItemClick;

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.rc_view);
            recyclerView.SetLayoutManager(new NonScrollGridLayoutManager(Activity, 1));
            recyclerView.SetAdapter(adapter);

            return view;
        }

        private void Adapter_OnItemClick(object sender, Core.Models.ValidatorModel e)
        {
            var activity = (StakingActivity)Activity;
            activity.ValidatorModel = e;
            activity.ShowFragment(new ValidatorFragment(e), false);
        }

        public override void OnRefresh()
        {
            
        }
    }

}
