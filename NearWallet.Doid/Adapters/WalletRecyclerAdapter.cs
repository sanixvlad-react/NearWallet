using System;
using System.Collections.Generic;

using Android.Views;
using Android.Widget;

using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;

using NearWallet.Core.Models;

namespace NearWallet.Doid.Adapters
{
    public class WalletRecyclerAdapter : BaseRecyclerAdapter<WalletModel>
    {
        public event EventHandler<WalletModel> OnItemClick = delegate { };

        public override int ItemCount => Items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = Items[position];
            // проверяем на null
            if (!(holder is WalletrecyclerViewHolder viewHolder)) return;

            viewHolder.Path.Text = item.Path;
            viewHolder.Indicator.Text = item.Path[0].ToString().ToUpper();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.cell_wallet, parent, false);

            var viewHolder = new WalletrecyclerViewHolder(itemView);
            viewHolder.CardView.Click += (s, e) =>
            {
                var item = Items[viewHolder.AdapterPosition];
                OnItemClick(itemView, item);
            };

            return viewHolder;
        }
    }

    public class WalletrecyclerViewHolder : RecyclerView.ViewHolder
    {
        public RelativeLayout CardView { get; private set; }
        public TextView Path { get; private set; }
        public TextView Indicator { get; private set; }

        public WalletrecyclerViewHolder(View itemView) : base(itemView)
        {
            CardView = itemView.FindViewById<RelativeLayout>(Resource.Id.rlt_data);

            Path = itemView.FindViewById<TextView>(Resource.Id.txt_waller_path);
            Indicator = itemView.FindViewById<TextView>(Resource.Id.txt_wallet_indicator);

        }
    }

}
