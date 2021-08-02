using System;

using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;

using Pl.Tajchert.WaitingDots;

namespace NearWallet.Doid.Views
{
    [Register("ru.sanix.WalletDataHeaderView")]
    public class WalletDataHeaderView : LinearLayout
    {
        public event EventHandler OnSendClick = delegate { };
        public event EventHandler OnReceiveClick = delegate { };
        public event EventHandler OnBuyClick = delegate { };

        public bool IsLoaded { get; private set; }

        private Context context;

        public WalletDataHeaderView(Context context) : base(context) { Init(context, null); }

        public WalletDataHeaderView(Context context, IAttributeSet attrs = null) : base(context, attrs, 0) { Init(context, attrs); }

        public WalletDataHeaderView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(context, attrs); }

        private void Init(Context context, IAttributeSet attrs)
        {
            this.context = context;
            LayoutInflater.From(context).Inflate(Resource.Layout.view_wallet_data_header, this, true);

            FindViewById<RoundImageButton>(Resource.Id.rib_buy).OnClick += Buy_OnClick;
            FindViewById<RoundImageButton>(Resource.Id.rib_receive).OnClick += Receive_OnClick;
            FindViewById<RoundImageButton>(Resource.Id.rib_send).OnClick += Send_OnClick;

        }

        public void SetLoaded(bool isLoaded)
        {
            IsLoaded = isLoaded;

            FindViewById<LinearLayout>(Resource.Id.lt_data_balance).Visibility = IsLoaded ? ViewStates.Invisible : ViewStates.Visible;
            FindViewById<LinearLayout>(Resource.Id.lt_load_balance).Visibility = IsLoaded ? ViewStates.Visible : ViewStates.Invisible;

            if (IsLoaded)
                FindViewById<DotsTextView>(Resource.Id.dotsTextView).ShowAndPlay();
            else
                FindViewById<DotsTextView>(Resource.Id.dotsTextView).HideAndStop();
            
        }

        public void SetTokenCount(string tokenCount)
        {

        }

        private void Send_OnClick(object sender, EventArgs e)
        {
            OnSendClick(this, EventArgs.Empty);
        }

        private void Receive_OnClick(object sender, EventArgs e)
        {
            OnReceiveClick(this, EventArgs.Empty);
        }

        private void Buy_OnClick(object sender, EventArgs e)
        {
            OnBuyClick(this, EventArgs.Empty);
        }
    }

}
