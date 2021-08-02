using System;

using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;

using NearWallet.Doid.Framework.Helpers;

namespace NearWallet.Doid.Views
{
    [Register("ru.sanix.StakedView")]
    public class StakedView : LinearLayout
    {

        public event EventHandler OnHeaderBtnClick = delegate { };
        public event EventHandler OnUnStakedClick = delegate { };
        public event EventHandler OnWithdrawClick = delegate { };

        private bool withdrawVisible = true;
        public bool WithdrawVisible
        {
            get => withdrawVisible;
            set
            {
                withdrawVisible = value;

                FindViewById<Button>(Resource.Id.btn_withdraw).Visibility = value ?
                                        ViewStates.Visible :
                                        ViewStates.Gone;
            }
        }

        private bool withdrawEnabled = true;
        public bool WithdrawEnabled
        {
            get => withdrawEnabled;
            set
            {
                withdrawEnabled = value;

                FindViewById<Button>(Resource.Id.btn_withdraw).Enabled = value;
            }
        }

        private bool unstakeEnabled = true;
        public bool UnstakeEnabled
        {
            get => unstakeEnabled;
            set
            {
                unstakeEnabled = value;

                FindViewById<Button>(Resource.Id.btn_unstake).Enabled = value;
            }
        }


        public StakedView(Context context) : base(context) { Init(context, null); }

        public StakedView(Context context, IAttributeSet attrs = null) : base(context, attrs, 0) { Init(context, attrs); }

        public StakedView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(context, attrs); }

        private void Init(Context context, IAttributeSet attrs)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.view_count_staked, this, true);

            FindViewById<ImageView>(Resource.Id.img_total_amount).Click += StakingFragment_Click;
            FindViewById<ImageView>(Resource.Id.img_rewards_earned).Click += StakingFragment_Click;
            FindViewById<ImageView>(Resource.Id.img_pending_release).Click += StakingFragment_Click;
            FindViewById<ImageView>(Resource.Id.img_avaible_for_withdraw).Click += StakingFragment_Click;

            FindViewById<Button>(Resource.Id.btn_stake_tokens).Click += StakingFragment_Click;
            FindViewById<Button>(Resource.Id.btn_unstake).Click += StakingFragment_Click;
            FindViewById<Button>(Resource.Id.btn_withdraw).Click += StakingFragment_Click;
        }

        public void SetBtnHeaderTitle(string title)
        {
            FindViewById<Button>(Resource.Id.btn_stake_tokens).Text = title;
        }

        private void StakingFragment_Click(object sender, EventArgs e)
        {
            switch (((View)sender).Id)
            {
                case Resource.Id.img_total_amount:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.total_amount_staked_message));
                    break;
                case Resource.Id.img_rewards_earned:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.rewards_earned_message));
                    break;
                case Resource.Id.img_pending_release:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.pending_release_message));
                    break;
                case Resource.Id.img_avaible_for_withdraw:
                    AlertHelper.ShowAlertMessage(Context, Resources.GetString(Resource.String.available_for_withdrawal_message));
                    break;
                case Resource.Id.btn_stake_tokens:
                    OnHeaderBtnClick(this, EventArgs.Empty);
                    break;
                case Resource.Id.btn_unstake:
                    OnUnStakedClick(this, EventArgs.Empty);
                    break;
                case Resource.Id.btn_withdraw:
                    OnWithdrawClick(this, EventArgs.Empty);
                    break;
            }
        }

    }

}