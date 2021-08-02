
using System.Collections.Generic;

using Android.Views;

using AndroidX.RecyclerView.Widget;

using NearWallet.Doid.Framework;

namespace NearWallet.Doid.Adapters
{
    public class BaseRecyclerAdapter<T> : RecyclerView.Adapter, IRecyclerAdapter<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public override int ItemCount => Items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) { }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return null;
        }

        public void SetItems(List<T> items)
        {
            Items = items;
            NotifyDataSetChanged();
        }

        public void AddItems(List<T> items)
        {
            Items.AddRange(items);
            NotifyDataSetChanged();
        }

        public void AddItem(T item)
        {
            Items.Add(item);
            NotifyDataSetChanged();
        }

        public void ClearItems()
        {
            Items.Clear();
            NotifyDataSetChanged();
        }

    }
}
