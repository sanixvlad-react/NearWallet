using System;

using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Graphics;
using Android.Views.Animations;

using NearWallet.Doid.Framework.Helpers;

namespace NearWallet.Doid.Views
{
    [Register("ru.sanix.WalletDataView")]
    public class WalletDataView : LinearLayout
    {
        private int animatinDuration = 500;

        private bool isOpenWalletBalance { get; set; } = false;
        private bool isOpenStakingPools { get; set; } = false;

        private int stakedPoolsViewHeight;
        private int walletBalanceViewHeight;

        public bool IsLoaded { get; private set; }

        public event EventHandler OnRecentActivityClick = delegate { };
        public event EventHandler OnAuthorizationAppsClick = delegate { };
        public event EventHandler OnChengeWalletClick = delegate { };
        public event EventHandler OnExitClick = delegate { };

        private Context context;

        public WalletDataView(Context context) : base(context) { Init(context, null); }

        public WalletDataView(Context context, IAttributeSet attrs = null) : base(context, attrs, 0) { Init(context, attrs); }

        public WalletDataView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(context, attrs); }

        private void Init(Context context, IAttributeSet attrs)
        {
            this.context = context;
            LayoutInflater.From(context).Inflate(Resource.Layout.view_wallet_data, this, true);

            FindViewById<RelativeLayout>(Resource.Id.rlt_wallet_balance).Click += ExpandLayout_Click;
            FindViewById<RelativeLayout>(Resource.Id.rlt_in_staking_pools).Click += ExpandLayout_Click;

            FindViewById<TextView>(Resource.Id.txt_wallet_balance_reserved).Click += TextView_Click;
            FindViewById<TextView>(Resource.Id.txt_wallet_staked).Click += TextView_Click;
            FindViewById<TextView>(Resource.Id.txt_wallet_pending_release).Click += TextView_Click;
            FindViewById<TextView>(Resource.Id.txt_wallet_balance_available_withdraw).Click += TextView_Click;
            FindViewById<TextView>(Resource.Id.txt_wallet_balance_avaible).Click += TextView_Click;
            FindViewById<TextView>(Resource.Id.txt_wallet_id_name).Click += TextView_Click;

            FindViewById<RelativeLayout>(Resource.Id.rlt_authorization_apps).Click += TextView_Click;
            FindViewById<RelativeLayout>(Resource.Id.rlt_recent_activity).Click += TextView_Click;
            FindViewById<RelativeLayout>(Resource.Id.rlt_go_to_another_wallet).Click += TextView_Click;
            FindViewById<RelativeLayout>(Resource.Id.rlt_exit).Click += TextView_Click;

            SetWillNotDraw(false);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (walletBalanceViewHeight != 0 || stakedPoolsViewHeight != 0) return;

            var viewWalletBalance = FindViewById<LinearLayout>(Resource.Id.lt_wallet_balance);
            walletBalanceViewHeight = viewWalletBalance.Height;

            var viewStakedPools = FindViewById<LinearLayout>(Resource.Id.lt_wallet_staking_pools);
            stakedPoolsViewHeight = viewStakedPools.Height;

            AnimateLayout(viewStakedPools, isOpenStakingPools);
            AnimateLayout(viewWalletBalance, isOpenWalletBalance);
        }

        private void AnimateLayout(View view, bool isOpen, int duration = -1)
        {
           var height = isOpen ?
                        GetExpandedViewHeight(view.Id, duration):
                        GetCollapsedViewHeight(view.Id, duration);

            ViewColapseAnimation(view, height, duration);
        }

        private void ViewColapseAnimation(View view, int colapseHeight, int duration)
        {
            var animation = new HeightAnimation(view, colapseHeight);
            animation.Duration = duration == -1 ? 0 : duration;
            view.StartAnimation(animation);
        }

        private int GetExpandedViewHeight(int viewId, int duration)
        {
            if (duration == -1) return 0;

            switch (viewId)
            {
                case Resource.Id.lt_wallet_staking_pools: return stakedPoolsViewHeight;
                case Resource.Id.lt_wallet_balance:
                    return isOpenStakingPools ?
                        walletBalanceViewHeight :
                        walletBalanceViewHeight - stakedPoolsViewHeight;
            }

            return 0;
        }

        private int GetCollapsedViewHeight(int viewId, int duration)
        {
            if (duration == -1) return 0;

            switch (viewId)
            {
                case Resource.Id.lt_wallet_staking_pools: return 0;
                case Resource.Id.lt_wallet_balance:
                    return isOpenStakingPools ?
                        0 :
                        isOpenWalletBalance?
                            walletBalanceViewHeight - stakedPoolsViewHeight :
                            0;
            }
 
            return 0;
        }

        #region Events

        private void ExpandLayout_Click(object sender, EventArgs e)
        {
            var view = (View)sender;

            switch (view.Id)
            {
                case Resource.Id.rlt_wallet_balance:
                    isOpenWalletBalance = !isOpenWalletBalance;
                    AnimateLayout(FindViewById<LinearLayout>(Resource.Id.lt_wallet_balance), isOpenWalletBalance, animatinDuration);

                    FindViewById<TextView>(Resource.Id.txt_wallet_balance)
                        .SetCompoundDrawablesWithIntrinsicBounds(0, 0, isOpenWalletBalance ?
                                    Resource.Drawable.ic_chevrone_up :
                                    Resource.Drawable.ic_chevrone_down, 0);
                    break;
                case Resource.Id.rlt_in_staking_pools:
                    isOpenStakingPools = !isOpenStakingPools;

                    AnimateLayout(FindViewById<LinearLayout>(Resource.Id.lt_wallet_staking_pools), isOpenStakingPools, animatinDuration);
                    AnimateLayout(FindViewById<LinearLayout>(Resource.Id.lt_wallet_balance), isOpenStakingPools, animatinDuration);

                    FindViewById<TextView>(Resource.Id.txt_wallet_reserve_storage)
                        .SetCompoundDrawablesWithIntrinsicBounds(0, 0, isOpenStakingPools ?
                                    Resource.Drawable.ic_chevrone_up :
                                    Resource.Drawable.ic_chevrone_down, 0);
                    break;
            }
        }

        private void TextView_Click(object sender, EventArgs e)
        {
            var view = (View)sender;

            switch (view.Id)
            {
                case Resource.Id.txt_wallet_balance_reserved:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.reserved_for_store_message));
                    break;
                case Resource.Id.txt_wallet_staked:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.total_amount_staked_message));
                    break;
                case Resource.Id.txt_wallet_pending_release:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.pending_release_message));
                    break;
                case Resource.Id.txt_wallet_balance_available_withdraw:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.available_for_withdrawal_message));
                    break;
                case Resource.Id.txt_wallet_balance_avaible:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.avaible_balance_prodil_message));
                    break;
                case Resource.Id.txt_wallet_id_name:
                    // copy on storage wallet name (sanix.near)
                    break;
                case Resource.Id.rlt_authorization_apps:
                    OnAuthorizationAppsClick(this, EventArgs.Empty);
                    break;
                case Resource.Id.rlt_recent_activity:
                    OnRecentActivityClick(this, EventArgs.Empty);
                    break;
                case Resource.Id.rlt_go_to_another_wallet:
                    OnChengeWalletClick(this, EventArgs.Empty);
                    break;
                case Resource.Id.rlt_exit:
                    OnExitClick(this, EventArgs.Empty);
                    break;
            }
        }

        #endregion

    }

    public class HeightAnimation : Animation
    {
        private View view;
        private int collapsedHeight;

        public HeightAnimation(View view, int collapsedHeight) { this.view = view; this.collapsedHeight = collapsedHeight; }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            int initialHeight = view.MeasuredHeight;

            int distanceToCollapse = (int)(initialHeight - collapsedHeight);

            view.LayoutParameters.Height = (int)(initialHeight - (distanceToCollapse * interpolatedTime));
            view.RequestLayout();
        }

        public override bool WillChangeBounds()
        {
            return true;
        }
    }

}
