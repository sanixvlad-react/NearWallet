using System;
using System.Collections.Generic;

using Android.Views;
using Android.Widget;

using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;

using Google.Android.Material.SwitchMaterial;

using NearWallet.Core.Models;

namespace NearWallet.Doid.Adapters
{
    public class CoinRecyclerAdapter : BaseRecyclerAdapter<CoinModel>
    {
        public event EventHandler<CoinModel> OnItemClick = delegate { };

        private bool visibleSwitch;
        public bool VisibleSwitch
        {
            get => visibleSwitch;
            set
            {
                visibleSwitch = value;
                NotifyDataSetChanged();
            }
        }

        public CoinRecyclerAdapter() { }
        public CoinRecyclerAdapter(bool visibleSwitch) { this.visibleSwitch = visibleSwitch; }

        public override int ItemCount => Items.Count;

        //public List<CoinModel> Items { get; set; } = new List<CoinModel>();

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = Items[position];
            // проверяем на null
            if (!(holder is CoinRecyclerViewHolder viewHolder)) return;

            viewHolder.CoinName.Text = item.Name;
            viewHolder.CoinBalance.Text = item.Balance;
            viewHolder.CoinWallet.Text = item.Wallet;
            //viewHolder.ImageView item.ImageUrl;
            viewHolder.Switch.Visibility = VisibleSwitch ? ViewStates.Visible : ViewStates.Gone;
            viewHolder.Switch.Checked = item.IsAdded;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.cell_coin, parent, false);

            var viewHolder = new CoinRecyclerViewHolder(itemView);
            viewHolder.CardView.Click += (s, e) =>
            {
                var item = Items[viewHolder.AdapterPosition];
                OnItemClick(itemView, item);
            };

            return viewHolder;
        }
    }

    public class CoinRecyclerViewHolder : RecyclerView.ViewHolder
    {
        public TextView CoinName { get; private set; }
        public TextView CoinWallet { get; private set; }
        public TextView CoinBalance { get; private set; }
        public ImageView ImageView { get; private set; }
        public CardView CardView { get; private set; }
        public SwitchMaterial Switch { get; private set; }

        public CoinRecyclerViewHolder(View itemView) : base(itemView)
        {
            CardView = itemView.FindViewById<CardView>(Resource.Id.card_coin);
            ImageView = itemView.FindViewById<ImageView>(Resource.Id.img_coin);
            CoinName = itemView.FindViewById<TextView>(Resource.Id.txt_coin_name);
            CoinWallet = itemView.FindViewById<TextView>(Resource.Id.txt_coin_wallet);
            CoinBalance = itemView.FindViewById<TextView>(Resource.Id.txt_coin_balance);
            Switch = itemView.FindViewById<SwitchMaterial>(Resource.Id.switch_view);
        }
    }

}
