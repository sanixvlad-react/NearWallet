using System;

using Android.Util;
using Android.Content;
using Android.Runtime;

using AndroidX.RecyclerView.Widget;

namespace NearWallet.Doid.Views
{
    public class NonScrollGridLayoutManager : GridLayoutManager
    {
        public NonScrollGridLayoutManager(Context context, int spanCount) : base(context, spanCount)
        {
        }

        public NonScrollGridLayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public NonScrollGridLayoutManager(Context context, int spanCount, int orientation, bool reverseLayout) : base(context, spanCount, orientation, reverseLayout)
        {
        }

        protected NonScrollGridLayoutManager(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool CanScrollVertically()
        {
            return false;
        }
    }
}
