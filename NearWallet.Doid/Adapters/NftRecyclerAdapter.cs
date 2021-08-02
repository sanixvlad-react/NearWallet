using System;
using System.Collections.Generic;

using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;

using NearWallet.Core.Models;
using static NearWallet.Core.Enums;

namespace NearWallet.Doid.Adapters
{
    public class NftRecyclerAdapter : BaseRecyclerAdapter<NftModel>
    {
        public NftRecyclerAdapter() { }

        public override int ItemCount => Items.Count;

        public override int GetItemViewType(int position) =>
            Items[position].MerketId == 0 ? (int)NftItemType.Header : (int)NftItemType.Data;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = Items[position];

            if (holder is NftHeaderRecyclerView titleHolder)
            {
                titleHolder.Name.Text = item.Title;
            }

            if (holder is NftDataRecyclerView nftHolder)
            {
                nftHolder.Name.Text = item.Title;
                //nftHolder.ImageView.;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == (int)NftItemType.Header)
            {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.cell_nft_header, parent, false);

                return new NftHeaderRecyclerView(view);
            }
            else
            {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.cell_nft_data, parent, false);

                var viewHolder = new NftDataRecyclerView(view);
                viewHolder.CardView.Click += (s, e) => { };

                return viewHolder;
            }
        }
    }

    public class NftHeaderRecyclerView : RecyclerView.ViewHolder
    {
        public TextView Name { get; private set; }
        public ImageView ImageView { get; private set; }


        public NftHeaderRecyclerView(View itemView) : base(itemView)
        {
            ImageView = itemView.FindViewById<ImageView>(Resource.Id.img_nft_market);
            Name = itemView.FindViewById<TextView>(Resource.Id.txt_nft_market_name);
        }
    }

    public class NftDataRecyclerView : RecyclerView.ViewHolder
    {
        public TextView Name { get; private set; }
        public ImageView ImageView { get; private set; }
        public CardView CardView { get; private set; }

        public NftDataRecyclerView(View itemView) : base(itemView)
        {
            CardView = itemView.FindViewById<CardView>(Resource.Id.card_nft);
            ImageView = itemView.FindViewById<ImageView>(Resource.Id.img_nft);
            Name = itemView.FindViewById<TextView>(Resource.Id.txt_nft_name);
        }
    }

}
