using System;
using Android.OS;
using Android.Views;

namespace NearWallet.Doid.Views.Dialogs
{
    public class SendCoinBottomSheet : FullScreenBottomSheet
    {

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.bottom_sheet_send, container, false); ;
        }

        public override void OnResume()
        {
            base.OnResume();

            
        }

    }

}
