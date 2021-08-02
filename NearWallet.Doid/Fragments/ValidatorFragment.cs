
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

using NearWallet.Doid.Views;
using NearWallet.Doid.Framework;
using NearWallet.Doid.Views.Dialogs;

using NearWallet.Core.Models;


namespace NearWallet.Doid.Fragments
{
    public class ValidatorFragment : BaseFragment
    {
        private ValidatorModel model;

        public ValidatorFragment(ValidatorModel model) { this.model = model; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_validator, container, false);
            view.FindViewById<StakedView>(Resource.Id.staked_view)
                                .SetBtnHeaderTitle(Resources.GetString(Resource.String.stake_with_validator));

            return view;
        }

        #region override methods
        public override void OnResume()
        {
            base.OnResume();

            View.FindViewById<StakedView>(Resource.Id.staked_view).OnHeaderBtnClick += OnHeaderBtn_Click;
            View.FindViewById<StakedView>(Resource.Id.staked_view).OnUnStakedClick += OnUnStaked_Click;
            View.FindViewById<StakedView>(Resource.Id.staked_view).OnWithdrawClick += OnWithdraw_Click;
        }

        

        public override void OnPause()
        {
            base.OnPause();

            View.FindViewById<StakedView>(Resource.Id.staked_view).OnHeaderBtnClick -= OnHeaderBtn_Click;
            View.FindViewById<StakedView>(Resource.Id.staked_view).OnUnStakedClick -= OnUnStaked_Click;
            View.FindViewById<StakedView>(Resource.Id.staked_view).OnWithdrawClick -= OnWithdraw_Click;
        }

        public override void OnRefresh()
        {
            
        }
        #endregion

        #region Events
        private void OnHeaderBtn_Click(object sender, EventArgs e)
        {
            new StakeCoinBottomSheet(StakeType.Staked, model).ShowNow(Activity.SupportFragmentManager, "staked_coin_bottom_sheet");
            
        }

        private void OnUnStaked_Click(object sender, EventArgs e)
        {
            new StakeCoinBottomSheet(StakeType.UnStaked, model).ShowNow(Activity.SupportFragmentManager, "unstaked_coin_bottom_sheet");
        }

        private void OnWithdraw_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

    }

}
