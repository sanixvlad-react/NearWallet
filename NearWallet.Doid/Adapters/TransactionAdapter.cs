
using System;
using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Widget;
using NearWallet.Core;
using NearWallet.Core.Models;
using NearWallet.Doid.Activitys;
using static NearWallet.Core.Enums;

namespace NearWallet.Doid.Adapters
{
    public class TransactionAdapter : BaseAdapter<TranzactionModel>
    {
        private Activity activity;
        public List<TranzactionModel> Items = new List<TranzactionModel>();

        public override TranzactionModel this[int position] => Items[position];

        public TransactionAdapter(Activity activity) { this.activity = activity; }

        public TransactionAdapter(Activity activity, List<TranzactionModel> items) { this.activity = activity; Items = items; }

        public override int Count => Items.Count;

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = Items[position];

            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.cell_transaction, null);

            view.FindViewById<ImageView>(Resource.Id.img_transaction).SetImageResource(GetImageResource(item.Type));

            view.FindViewById<TextView>(Resource.Id.txt_transaction_name).Text = RecentsActivity.GetTitleByType(activity, item);
            view.FindViewById<TextView>(Resource.Id.txt_descriprion).Text = RecentsActivity.GetDescription(activity, item);// item.Description;

            view.FindViewById<TextView>(Resource.Id.txt_time).Text = item.DateTimePassed;

            var txtAmount = view.FindViewById<TextView>(Resource.Id.txt_amount);
            txtAmount.Text = (item.Type == TranzactionType.Receive || item.Type == TranzactionType.Send) ?
                                item.Amount + " " + item.TokenName : string.Empty;
            txtAmount.Visibility = (item.Type == TranzactionType.Receive || item.Type == TranzactionType.Send) ?
                                    ViewStates.Visible : ViewStates.Gone;

            TextViewCompat.SetTextAppearance(txtAmount, item.Type == TranzactionType.Send ?
                                                                Resource.Style.TextViewDefaultNegative :
                                                                Resource.Style.TextViewDefaultPositive);

            return view;
        }

        private int GetImageResource(Enums.TranzactionType type)
        {
            switch (type)
            {
                case TranzactionType.AccessKeyAdded: return Resource.Drawable.ic_access_key;
                case TranzactionType.Receive: return Resource.Drawable.ic_receive_accent;
                case TranzactionType.Send: return Resource.Drawable.ic_send_accent;
                case TranzactionType.MethodCalled:
                default: return Resource.Drawable.ic_chevrone_right_left;
            }
        }

        public void SetItems(List<TranzactionModel> items)
        {
            Items = items;
            NotifyDataSetChanged();
        }

        public void AddItems(List<TranzactionModel> items)
        {
            Items.AddRange(items);
            NotifyDataSetChanged();
        }

        public void RemoveItem(TranzactionModel item)
        {
            Items.Remove(item);
            NotifyDataSetChanged();
        }

    }
}
