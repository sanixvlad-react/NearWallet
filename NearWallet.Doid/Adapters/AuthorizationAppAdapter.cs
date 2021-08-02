
using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;

using NearWallet.Core.Models;

namespace NearWallet.Doid.Adapters
{
    public class AuthorizationAppAdapter : BaseAdapter<AuthorizationAppModel>
    {
        private Activity activity;
        public List<AuthorizationAppModel> Items = new List<AuthorizationAppModel>();

        public override AuthorizationAppModel this[int position] => Items[position];

        public AuthorizationAppAdapter(Activity activity) { this.activity = activity; }

        public AuthorizationAppAdapter(Activity activity, List<AuthorizationAppModel> items) { this.activity = activity; Items = items; }

        public override int Count => Items.Count;

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = Items[position];

            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.cell_auth_app, null);

            view.FindViewById<TextView>(Resource.Id.txt_waller_path).Text = item.WalletPath;

            view.FindViewById<TextView>(Resource.Id.txt_payment_storage).Text =
                activity.Resources.GetString(Resource.String.fee_allowance) + " " + item.PaymentForStorage + " NEAR";
            view.FindViewById<TextView>(Resource.Id.txt_pablic_key).Text = activity.Resources.GetString(Resource.String.public_key) + " " + item.PublicKey;

            return view;
        }

        public void SetItems(List<AuthorizationAppModel> items)
        {
            Items = items;
            NotifyDataSetChanged();
        }

        public void AddItems(List<AuthorizationAppModel> items)
        {
            Items.AddRange(items);
            NotifyDataSetChanged();
        }

        public void RemoveItem(AuthorizationAppModel item)
        {
            Items.Remove(item);
            NotifyDataSetChanged();
        }

        public void RemoveItem(int index)
        {
            Items.Remove(Items[index]);
            NotifyDataSetChanged();
        }
    }

}
