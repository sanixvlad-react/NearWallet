using AndroidX.RecyclerView.Widget;

using NearWallet.Doid.Adapters;
using static NearWallet.Core.Enums;

namespace NearWallet.Doid.Views.Recycler.SpanSizeLookup
{
    public class NftSpanSizeLookup : GridLayoutManager.SpanSizeLookup
    {
        private NftRecyclerAdapter adapter;
        public NftSpanSizeLookup(NftRecyclerAdapter adapter)
        {
            this.adapter = adapter;
        }

        public override int GetSpanSize(int position)
        {
            var viewType = adapter.GetItemViewType(position);
            return viewType == (int)NftItemType.Data ? 1 : 2;
        }
    }
}
