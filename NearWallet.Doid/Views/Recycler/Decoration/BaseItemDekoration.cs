using System;

using Android.Views;
using Android.Graphics;

using AndroidX.RecyclerView.Widget;

namespace NearWallet.Doid.Views.Recycler.Decoration
{
    public class BaseItemDekoration : RecyclerView.ItemDecoration
    {
        private RecyclerView.Adapter adapter;

        public BaseItemDekoration(RecyclerView.Adapter adapter) { this.adapter = adapter; }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            base.GetItemOffsets(outRect, view, parent, state);

            var position = parent.GetChildLayoutPosition(view);

            var paddingMax = view.Resources.GetDimension(Resource.Dimension.padding_max);
            var paddingMin = view.Resources.GetDimension(Resource.Dimension.padding_litle);

            if (position == 0)
            {
                outRect.Right = outRect.Left = outRect.Top = (int)paddingMax;
                outRect.Bottom = (int)paddingMin;
                return;
            }

            if(position == adapter.ItemCount - 1)
            {
                outRect.Right = outRect.Left = outRect.Bottom = (int)paddingMax;
                outRect.Top = (int)paddingMin;
                return;
            }

            outRect.Right = outRect.Left =  (int)paddingMax;
            outRect.Top = outRect.Bottom = (int)paddingMin;
        }
    }

}
