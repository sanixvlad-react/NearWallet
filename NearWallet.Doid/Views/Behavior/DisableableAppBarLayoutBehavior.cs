using System;

using Android.Util;
using Android.Views;
using Android.Content;

using AndroidX.CoordinatorLayout.Widget;

using Google.Android.Material.AppBar;

namespace NearWallet.Doid.Views.Behavior
{
    public class DisableableAppBarLayoutBehavior : AppBarLayout.Behavior
    {
        public bool IsEnabled { get; set; } = true;

        public DisableableAppBarLayoutBehavior(Context context, IAttributeSet attrs): base(context, attrs) { }

        public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View directTargetChild, View target, int axes, int type)
        {
            return IsEnabled && base.OnStartNestedScroll(coordinatorLayout, child, directTargetChild, target, axes, type);
        }

    }

}
