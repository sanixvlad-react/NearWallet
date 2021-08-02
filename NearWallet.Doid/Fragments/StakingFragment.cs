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
using NearWallet.Doid.Framework.Helpers;

using NearWallet.Core;

using Newtonsoft.Json;

namespace NearWallet.Doid.Fragments
{
    public class StakingFragment : BaseFragment
    {
        #region override methods

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_staking, container, false);

            ((MainActivity)Activity).IsEnabledRefresh = true;

            var adapter = new ValidatorRecyclerAdapter();
            adapter.AddItems(Data.MyValidators);
            adapter.OnItemClick += Adapter_OnItemClick;

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.rc_view);
            recyclerView.SetLayoutManager(new NonScrollGridLayoutManager(Activity, 1));
            recyclerView.SetAdapter(adapter);

            return view;
        }
        
        public override void OnRefresh()
        {

        }

        public override void OnResume()
        {
            base.OnResume();

            View.FindViewById<StakedView>(Resource.Id.staked_view).OnHeaderBtnClick += StakedView_OnStakedClick;
            View.FindViewById<StakedView>(Resource.Id.staked_view).OnUnStakedClick += OnUnStaked_Click;
            View.FindViewById<ImageView>(Resource.Id.img_staked_from).Click += StakingFragment_Click;
        }

        public override void OnPause()
        {
            base.OnPause();

            View.FindViewById<StakedView>(Resource.Id.staked_view).OnHeaderBtnClick -= StakedView_OnStakedClick;
            View.FindViewById<StakedView>(Resource.Id.staked_view).OnUnStakedClick -= OnUnStaked_Click;
            View.FindViewById<ImageView>(Resource.Id.img_staked_from).Click -= StakingFragment_Click;
        }
        #endregion

        #region Events
        private void StakedView_OnStakedClick(object sender, EventArgs e)
        {
            GoStakingActivity(StakedFragmentType.AllValidators);
        }

        private void OnUnStaked_Click(object sender, EventArgs e)
        {
            GoStakingActivity(StakedFragmentType.YourValidators);
        }

        private void GoStakingActivity(StakedFragmentType type, string jsonValidatorModel = null)
        {
            var intent = new Intent(Activity, typeof(StakingActivity));
            intent.PutExtra(DroidConstString.IntentStakedValidatorModel, jsonValidatorModel);
            intent.PutExtra(DroidConstString.IntentStakedType, (int)type);
            StartActivity(intent);
        }

        private void StakingFragment_Click(object sender, EventArgs e)
        {
            switch (((View)sender).Id)
            {
                case Resource.Id.img_staked_from:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.staking_from_message));
                    break;
            }
        }

        private void Adapter_OnItemClick(object sender, Core.Models.ValidatorModel e)
        {
            GoStakingActivity(StakedFragmentType.Validator, JsonConvert.SerializeObject(e));
        }
        #endregion

    }

}
