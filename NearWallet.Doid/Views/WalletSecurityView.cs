using System;

using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;

using static NearWallet.Doid.Views.SwitchSecurityView;

namespace NearWallet.Doid.Views
{
    [Register("ru.sanix.WalletSecurityView")]
    public class WalletSecurityView : LinearLayout
    {
        private Context context;

        private SecurityModel model = new SecurityModel
        {
            Ledger = new SwitchSecutityModel
            {
                Data = string.Empty,
                Enabled = false
            },
            Passphrase = new SwitchSecutityModel
            {
                Data = string.Empty,
                Enabled = true
            },
            Mail = new SwitchSecutityModel
            {
                Data = "sanixvlad@gmail.com",
                Enabled = true
            },
            Phone = new SwitchSecutityModel
            {
                Data = "+(373)77940598",
                Enabled = true
            },
            TFA = new SwitchSecutityModel
            {
                Data = string.Empty,
                Enabled = false
            },
        };

        public WalletSecurityView(Context context) : base(context) { Init(context, null); }

        public WalletSecurityView(Context context, IAttributeSet attrs = null) : base(context, attrs, 0) { Init(context, attrs); }

        public WalletSecurityView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(context, attrs); }

        private void Init(Context context, IAttributeSet attrs)
        {
            this.context = context;
            LayoutInflater.From(context).Inflate(Resource.Layout.view_security_and_recovery, this, true);

            SetData(model);
        }

        public void SetData(SecurityModel model)
        {
            this.model = model;

            FindViewById<SwitchSecurityView>(Resource.Id.view_switch_ledger).SetData(model.Ledger);
            FindViewById<SwitchSecurityView>(Resource.Id.view_switch_passphrase).SetData(model.Passphrase);
            FindViewById<SwitchSecurityView>(Resource.Id.view_switch_email).SetData(model.Mail);
            FindViewById<SwitchSecurityView>(Resource.Id.view_switch_phone).SetData(model.Phone);
            FindViewById<SwitchSecurityView>(Resource.Id.view_switch_two_factor_authentication).SetData(model.TFA);
        }
    }

    public class SecurityModel
    {
        public SwitchSecutityModel Ledger { get; set; }

        public SwitchSecutityModel Passphrase { get; set; }

        public SwitchSecutityModel Mail { get; set; }

        public SwitchSecutityModel Phone { get; set; }

        /// <summary>
        /// 2FA Not Enabled
        /// </summary>
        public SwitchSecutityModel TFA { get; set; }
    }


}
