using System;
using System.Collections.Generic;

using Android.Views;
using Android.Widget;
using Android.Graphics;

using AndroidX.RecyclerView.Widget;

using NearWallet.Core.Models;

using Refractored.Controls;

namespace NearWallet.Doid.Adapters
{
    public class ValidatorRecyclerAdapter : BaseRecyclerAdapter<ValidatorModel>
    {
        public event EventHandler<ValidatorModel> OnItemClick = delegate { };

        public override int ItemCount => Items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = Items[position];
            // проверяем на null
            if (!(holder is ValidatorRecyclerViewHolder viewHolder)) return;

            viewHolder.WalletPath.Text = item.Name;
            viewHolder.FreeProcent.Text = item.Procent + "% Fee - ";
            viewHolder.StakedBalance.Text = item.StakedNear.ToString() + " NEAR";
            viewHolder.StatusValidator.Text = item.Status;

            var color = new Color(viewHolder.StatusValidator.Context.GetColor(item.Status == "active"
                                                                                ? Resource.Color.color_green :
                                                                                Resource.Color.color_red));
            viewHolder.StatusValidator.SetTextColor(color);
            //viewHolder.ImageView item.PhotoUrl;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.cell_validator, parent, false);

            var viewHolder = new ValidatorRecyclerViewHolder(itemView);
            viewHolder.TouchLayout.Click += (s, e) =>
            {
                var item = Items[viewHolder.AdapterPosition];
                OnItemClick(itemView, item);
            };

            return viewHolder;
        }
    }

    public class ValidatorRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public RelativeLayout TouchLayout { get; private set; }

        public TextView WalletPath { get; private set; }
        public TextView FreeProcent { get; private set; }
        public TextView StakedBalance { get; private set; }
        public TextView StatusValidator { get; private set; }

        public CircleImageView ImageView { get; private set; }

        public ValidatorRecyclerViewHolder(View itemView) : base(itemView)
        {
            TouchLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.rlt_validator);

            ImageView = itemView.FindViewById<CircleImageView>(Resource.Id.img_validator);

            WalletPath = itemView.FindViewById<TextView>(Resource.Id.txt_validaror_name);
            FreeProcent = itemView.FindViewById<TextView>(Resource.Id.txt_free);
            StakedBalance = itemView.FindViewById<TextView>(Resource.Id.txt_count_staking);
            StatusValidator = itemView.FindViewById<TextView>(Resource.Id.txt_status_validator);
        }
    }

}
