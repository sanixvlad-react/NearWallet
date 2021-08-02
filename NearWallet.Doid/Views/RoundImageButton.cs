using System;

using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;

namespace NearWallet.Doid.Views
{
    [Register("ru.sanix.RoundImageButton")]
    public class RoundImageButton : LinearLayout
    {
        public event EventHandler OnClick = delegate { };

        private Context context;

        public RoundImageButton(Context context) : base(context) { Init(context, null); }

        public RoundImageButton(Context context, IAttributeSet attrs = null) : base(context, attrs, 0) { Init(context, attrs); }

        public RoundImageButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(context, attrs); }

        private void Init(Context context, IAttributeSet attrs)
        {
            this.context = context;
            LayoutInflater.From(context).Inflate(Resource.Layout.view_round_image_button, this, true);

            FindViewById<LinearLayout>(Resource.Id.lt_rb).Click += RoundImageButton_Click;

            if (attrs == null) return;

            var styledAttributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.RoundImageButton);

            var title = styledAttributes.GetString(Resource.Styleable.RoundImageButton_rib_title);
            FindViewById<TextView>(Resource.Id.txt_rb_title).Text = title;

            var image = styledAttributes.GetDrawable(Resource.Styleable.RoundImageButton_rib_image_src);
            FindViewById<ImageView>(Resource.Id.img_rb_title).SetImageDrawable(image);
        }

        private void RoundImageButton_Click(object sender, EventArgs e) { OnClick(sender, e); }
    }

}
