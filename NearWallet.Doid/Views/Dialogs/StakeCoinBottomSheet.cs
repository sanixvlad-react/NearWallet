using System;

using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using NearWallet.Core.Models;

using NearWallet.Doid.Framework;

namespace NearWallet.Doid.Views.Dialogs
{
    public class StakeCoinBottomSheet : FullScreenBottomSheet
    {
        private StakeType type;
        private ValidatorModel model;

        public StakeCoinBottomSheet(StakeType type, ValidatorModel mode) { this.type = type; this.model = mode; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.bottom_sheet_staked, container, false);

            view.FindViewById<ImageView>(Resource.Id.img_send_to)
                .SetImageResource(type == StakeType.Staked ? Resource.Drawable.ic_arrow_bottom_send : Resource.Drawable.ic_arrow_top_send);
            view.FindViewById<TextView>(Resource.Id.txt_title).Text =
                                        Resources.GetString(type == StakeType.Staked ?
                                                            Resource.String.stake_amount : Resource.String.unstake_tokens);
            view.FindViewById<TextView>(Resource.Id.txt_send_to).Text = 
                                        Resources.GetString(type == StakeType.Staked ?
                                                            Resource.String.stake_with : Resource.String.unstake_from);

            view.FindViewById<TextView>(Resource.Id.txt_validaror_name).Text = model.Name;
            view.FindViewById<TextView>(Resource.Id.txt_free).Text = model.Procent.ToString() + "% Fee - ";
            view.FindViewById<TextView>(Resource.Id.txt_status_validator).Text = model.Status;


            var color = new Color(Context.GetColor(model.Status == "active"
                                          ? Resource.Color.color_green :
                                          Resource.Color.color_red));
            view.FindViewById<TextView>(Resource.Id.txt_status_validator).SetTextColor(color);

            return view;
        }

    }

}
