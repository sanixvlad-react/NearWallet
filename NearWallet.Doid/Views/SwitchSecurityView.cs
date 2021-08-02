using System;

using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;

using Google.Android.Material.SwitchMaterial;

namespace NearWallet.Doid.Views
{
    [Register("ru.sanix.SwitchSecurityView")]
    public class SwitchSecurityView : LinearLayout
    {
        public string Title { get; private set; }

        private Context context;

        public SwitchSecutityModel Model { get; private set; }

        public SwitchSecurityView(Context context) : base(context) { Init(context, null); }

        public SwitchSecurityView(Context context, IAttributeSet attrs = null) : base(context, attrs, 0) { Init(context, attrs); }

        public SwitchSecurityView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(context, attrs); }

        private void Init(Context context, IAttributeSet attrs)
        {
            this.context = context;
            LayoutInflater.From(context).Inflate(Resource.Layout.view_switch, this, true);

            FindViewById<SwitchMaterial>(Resource.Id.view_switch).CheckedChange += SwitchSecutityView_CheckedChange;

            if (attrs == null) return;

            var typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.SwitchSecurityView);
            Title = typedArray.GetString(Resource.Styleable.SwitchSecurityView_ssv_title);

            FindViewById<TextView>(Resource.Id.txt_switch_title).Text = Title;
        }

        private void SwitchSecutityView_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var dataView = FindViewById<TextView>(Resource.Id.txt_switch_data);

            SetDataOnView(Model.Data, e.IsChecked);
        }

        public void SetData(SwitchSecutityModel dataModel)
        {
            Model = dataModel;

            FindViewById<SwitchMaterial>(Resource.Id.view_switch).Checked = dataModel.Enabled;

            SetDataOnView(Model.Data, Model.Enabled);
        }

        private void SetDataOnView(string data, bool enabled)
        {
            var dataView = FindViewById<TextView>(Resource.Id.txt_switch_data);

            dataView.Text = string.IsNullOrEmpty(data) ? string.Empty : data;
            dataView.Visibility = (enabled && !string.IsNullOrEmpty(data)) ? ViewStates.Visible : ViewStates.Gone;
        }

        public class SwitchSecutityModel
        {
            public bool Enabled { get; set; }
            public string Data { get; set; }
        }
    }

}
