
using Android.Views;
using Android.Content;

using Google.Android.Material.Dialog;

namespace NearWallet.Doid.Framework.Helpers
{
    public class AlertHelper
    {
        public static void ShowAlertMessage(Context context, string message)
        {
            var alert = new MaterialAlertDialogBuilder(context, Resource.Style.AlertDialogStyle);
            alert.SetMessage(message);
            alert.SetNegativeButton(context.GetString(Resource.String.cancel), (senderAlert, args) => { });
            alert.Create().Show();
        }

        public static void ShowAlertMessage(Context context, View view)
        {
            var alert = new MaterialAlertDialogBuilder(context, Resource.Style.AlertDialogStyle);
            alert.SetView(view);
            alert.SetNegativeButton(context.GetString(Resource.String.cancel), (senderAlert, args) => { });
            alert.Create().Show();
        }
    }
}
