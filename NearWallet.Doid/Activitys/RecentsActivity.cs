
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Content.PM;

using SwipeMenuListView = SwipeListView.SwipeListView;
using ToolbarX = AndroidX.AppCompat.Widget.Toolbar;

using AndroidX.Core.Widget;
using AndroidX.SwipeRefreshLayout.Widget;

using NearWallet.Doid.Adapters;

using NearWallet.Core;
using NearWallet.Doid.Framework.Helpers;
using static NearWallet.Core.Enums;
using Google.Android.Material.TextView;

namespace NearWallet.Doid.Activitys
{
    [Activity(Label = "RecentsActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait,
        // настройки акитивити чтоб не перезагружалась при повороте эерана
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class RecentsActivity : BaseActivity, SwipeRefreshLayout.IOnRefreshListener, AbsListView.IOnScrollListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_authorization_apps);

            FindViewById<MaterialTextView>(Resource.Id.toolbarTitle).Text = Resources.GetString(Resource.String.recent_activity);
            FindViewById<MaterialTextView>(Resource.Id.toolbarSubTitle).Visibility = ViewStates.Gone;

            SetSupportActionBar(FindViewById<ToolbarX>(Resource.Id.toolbar));

            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_left);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var listView = FindViewById<SwipeMenuListView>(Resource.Id.swipe_list_view);
            listView.SetOnScrollListener(this);
            listView.Adapter = new TransactionAdapter(this, Data.Tranzactions);
            listView.ItemClick += ListView_ItemClick;

            FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh).SetOnRefreshListener(this);
        }

        #region Alert transaction
        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = ((TransactionAdapter)FindViewById<SwipeMenuListView>(Resource.Id.swipe_list_view).Adapter).Items[e.Position];

            var view = LayoutInflater.Inflate(Resource.Layout.view_aler_transaction, null);

            view.FindViewById<TextView>(Resource.Id.txt_title).Text = GetTitleByType(this, item);

            view.FindViewById<LinearLayout>(Resource.Id.lt_alert_amount).Visibility =
                                                        item.Type == TranzactionType.AccessKeyAdded ?
                                                                        ViewStates.Gone :
                                                                        ViewStates.Visible;
            view.FindViewById<TextView>(Resource.Id.txt_title_amount).Text = GetAmoutTitleByType(item.Type);
            var txtAmount = view.FindViewById<TextView>(Resource.Id.txt_amount);
            txtAmount.Text = GetStringAmountByType(item);
            TextViewCompat.SetTextAppearance(txtAmount, GetAmmountTxtxStyleByType(item.Type));

            view.FindViewById<TextView>(Resource.Id.txt_wallet_title).Text = GetWalletTitle(this, item.Type);
            view.FindViewById<TextView>(Resource.Id.txt_wallet_path).Text = item.WalletPath;

            view.FindViewById<TextView>(Resource.Id.txt_date_time).Text = item.DateCreated;

            view.FindViewById<TextView>(Resource.Id.txt_status).Text =
                                        item.Status == StatusType.Succeeded?
                                                Resources.GetString(Resource.String.succeeded):
                                                Resources.GetString(Resource.String.status_not_available);

            view.FindViewById<View>(Resource.Id.view_status)
                .SetBackgroundResource(item.Status == StatusType.Succeeded ?
                                                   Resource.Drawable.view_positive_background :
                                                   Resource.Drawable.view_negative_background);

            AlertHelper.ShowAlertMessage(this, view);
        }

        public static string GetDescription(Context context, Core.Models.TranzactionModel model)
        {
            switch (model.Type)
            {
                case TranzactionType.AccessKeyAdded: return context.Resources.GetString(Resource.String.for_for) + " " + model.WalletPath;
                case TranzactionType.Receive: return context.Resources.GetString(Resource.String.from) + " " + model.WalletPath;
                case TranzactionType.Send: return context.Resources.GetString(Resource.String.to) + " " + model.WalletPath; ;
                case TranzactionType.MethodCalled:
                default: return model.Description + " " + context.Resources.GetString(Resource.String.in_contract);
            }
        }

        public static string GetWalletTitle(Context context, TranzactionType type)
        {
            switch (type)
            {
                case TranzactionType.AccessKeyAdded: return context.Resources.GetString(Resource.String.for_for);
                case TranzactionType.Receive: return context.Resources.GetString(Resource.String.from);
                case TranzactionType.Send: return context.Resources.GetString(Resource.String.to);
                case TranzactionType.MethodCalled:
                default: return context.Resources.GetString(Resource.String.in_contract);
            }
        }

        private string GetStringAmountByType(Core.Models.TranzactionModel item)
        {
            switch (item.Type)
            {
                case TranzactionType.AccessKeyAdded: return string.Empty;
                case TranzactionType.Receive:
                case TranzactionType.Send: return item.Amount.ToString();
                case TranzactionType.MethodCalled:
                default: return item.Description;
            }
        }

        private int GetAmmountTxtxStyleByType(TranzactionType type)
        {
            switch (type)
            {
                
                case TranzactionType.Receive: return Resource.Style.TextViewDefaultPositive;
                case TranzactionType.Send: return Resource.Style.TextViewDefaultNegative;
                case TranzactionType.MethodCalled:
                case TranzactionType.AccessKeyAdded: 
                default: return Resource.Style.TextViewDefault;
            }
        }

        public static string GetTitleByType(Context context, Core.Models.TranzactionModel item)
        {
            switch (item.Type)
            {
                case TranzactionType.AccessKeyAdded: return context.Resources.GetString(Resource.String.access_key_added);
                case TranzactionType.Receive: return context.Resources.GetString(Resource.String.receive) + " " + item.TokenName;
                case TranzactionType.Send: return context.Resources.GetString(Resource.String.send) + " " + item.TokenName;
                case TranzactionType.MethodCalled:
                default: return context.Resources.GetString(Resource.String.method);
            }
        }

        private string GetAmoutTitleByType(TranzactionType type)
        {
            switch (type)
            {
                case TranzactionType.AccessKeyAdded: return string.Empty;
                case TranzactionType.Receive:
                case TranzactionType.Send: return Resources.GetString(Resource.String.amount);
                case TranzactionType.MethodCalled:
                default: return Resources.GetString(Resource.String.method);
            }
        }

        #endregion

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        /// <summary>
        /// SwipeRefreshLayout.IOnRefreshListener
        /// </summary>
        public async void OnRefresh()
        {
            await Task.Delay(800);

            FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh).Refreshing = false;
        }

        #region list scroll
        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {

        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {

        }
        #endregion
    }

}
