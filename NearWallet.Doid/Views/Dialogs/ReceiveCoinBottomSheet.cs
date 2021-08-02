using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;

using ZXing;
using ZXing.QrCode;

using NearWallet.Doid.Activitys;

namespace NearWallet.Doid.Views.Dialogs
{
    public class ReceiveCoinBottomSheet : FullScreenBottomSheet
    {
        private TextView txtWalletPath { get => View == null? null : View.FindViewById<TextView>(Resource.Id.txt_waller_path); }
        private ImageView imgQr { get => View == null ? null : View.FindViewById<ImageView>(Resource.Id.img_qr_code); }

        private string walletPath;
        public string WalletPath
        {
            get => walletPath;
            private set
            {
                walletPath = value;

                if (txtWalletPath == null || imgQr == null) return;

                txtWalletPath.Text = value;

                SetQrCodeData(value);
            }
        }

        private void SetQrCodeData(string value)
        {
            var options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = imgQr.LayoutParameters.Width,
                Height = imgQr.LayoutParameters.Height,
            };
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options
            };

            var bitmap = writer.Write(value);
            imgQr.SetImageBitmap(bitmap);
        }

        public ReceiveCoinBottomSheet(string walletPath) { WalletPath = walletPath; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.bottom_sheet_receive, container, false); ;
        }

        public override void OnResume()
        {
            base.OnResume();

            txtWalletPath.Text = WalletPath;
            SetQrCodeData(WalletPath);

            View.FindViewById<RoundImageButton>(Resource.Id.rib_share).OnClick += Share_OnClick;
            View.FindViewById<RoundImageButton>(Resource.Id.rib_copy).OnClick += Copy_OnClick;
        }

        private void Copy_OnClick(object sender, System.EventArgs e)
        {
            SplashScreen.GetToast((Activity)Context, "Copy");
        }

        private void Share_OnClick(object sender, System.EventArgs e)
        {
            SplashScreen.GetToast((Activity)Context, "Share");
        }

    }

}