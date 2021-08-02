using System.Collections.Generic;

using Android.Views;

namespace NearWallet.Doid.Framework
{
    public interface IRecyclerClickListener
    {
        void OnClick(View view, int position);
    }

    public interface IRecyclerAdapter<T>
    {
        public List<T> Items { get; set; }
    }

}
