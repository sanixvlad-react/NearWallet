using System;

using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

using AndroidX.CardView.Widget;

namespace NearWallet.Doid.Views.Dialogs
{
    public class BuyCoinBottomSheet : FullScreenBottomSheet
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.bottom_sheet_buy, container, false); ;
        }

        public override void OnResume()
        {
            base.OnResume();

            View.FindViewById<TextView>(Resource.Id.txt_coin_market_cap).Click += BuyCoinBottomSheet_Click;
            View.FindViewById<TextView>(Resource.Id.txt_moon_pay_more).Click += BuyCoinBottomSheet_Click;

            View.FindViewById<CardView>(Resource.Id.card_binance).Click += BuyCoinBottomSheet_Click;
            View.FindViewById<CardView>(Resource.Id.card_huobi).Click += BuyCoinBottomSheet_Click;
            View.FindViewById<CardView>(Resource.Id.card_okex).Click += BuyCoinBottomSheet_Click;
            View.FindViewById<CardView>(Resource.Id.card_gateio).Click += BuyCoinBottomSheet_Click;
        }

        public override void OnPause()
        {
            base.OnPause();

            View.FindViewById<TextView>(Resource.Id.txt_coin_market_cap).Click -= BuyCoinBottomSheet_Click;
            View.FindViewById<TextView>(Resource.Id.txt_moon_pay_more).Click -= BuyCoinBottomSheet_Click;

            View.FindViewById<CardView>(Resource.Id.card_binance).Click -= BuyCoinBottomSheet_Click;
            View.FindViewById<CardView>(Resource.Id.card_huobi).Click -= BuyCoinBottomSheet_Click;
            View.FindViewById<CardView>(Resource.Id.card_okex).Click -= BuyCoinBottomSheet_Click;
            View.FindViewById<CardView>(Resource.Id.card_gateio).Click -= BuyCoinBottomSheet_Click;
        }

        private void BuyCoinBottomSheet_Click(object sender, EventArgs e)
        {
            var view = (View)sender;
            switch (view.Id)
            {
                case Resource.Id.txt_moon_pay_more:
                    GoToUrl("https://support.moonpay.com");
                    break;
                case Resource.Id.txt_coin_market_cap:
                    GoToUrl("https://coinmarketcap.com/currencies/near-protocol/markets/");
                    break;

                case Resource.Id.card_binance:
                    GoToUrl("https://www.binance.com/");
                    break;
                case Resource.Id.card_huobi:
                    GoToUrl("https://www.huobi.com/");
                    break;
                case Resource.Id.card_okex:
                    GoToUrl("https://www.okex.com/");
                    break;
                case Resource.Id.card_gateio:
                    GoToUrl("https://www.gate.io/");
                    break;
            }
        }

        private void GoToUrl(string url)
        {
            var browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
            StartActivity(browserIntent);
        }
    }

}
