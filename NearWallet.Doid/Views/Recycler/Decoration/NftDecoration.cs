using System;
using System.Linq;

using Android.Views;
using Android.Graphics;

using AndroidX.RecyclerView.Widget;

using NearWallet.Doid.Adapters;

using static NearWallet.Core.Enums;

namespace NearWallet.Doid.Views.Recycler.Decoration
{
    public class NftDecoration : RecyclerView.ItemDecoration
    {
        /// <summary>
        /// сколько итемов в конце 1 или 2 которым нужно установить падинг
        /// </summary>
        private int spase;

        private NftRecyclerAdapter adapter;

        public NftDecoration(int spase, NftRecyclerAdapter adapter)
        {
            this.spase = spase;
            this.adapter = adapter;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            base.GetItemOffsets(outRect, view, parent, state);

            var position = parent.GetChildLayoutPosition(view);
            var viewType = adapter.GetItemViewType(position);

            if (viewType == (int)NftItemType.Data)
            {
                var item = adapter.Items[position];

                var items = adapter.Items.Where(_ => _.MerketId == item.MerketId).ToList();
                var index = items.IndexOf(item) % 2;

                outRect.Left = index == 0 ? spase : spase / 4;
                outRect.Right = index == 0 ? spase / 4 : spase;

                if (position + 1 == adapter.ItemCount)
                {
                    outRect.Bottom = spase * 5;
                    outRect.Top = spase / 4;
                }
                else
                    outRect.Top = outRect.Bottom = spase / 4;
            }
            else
            {
                outRect.Left = spase;
                outRect.Bottom = outRect.Top = spase / 4 * 3;
            }
            
        }

    }
}
