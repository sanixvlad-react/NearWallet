using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Google.Android.Material.BottomSheet;

namespace NearWallet.Doid.Views.Dialogs
{
    public class FullScreenBottomSheet : BottomSheetDialogFragment, IDialogInterfaceOnShowListener
    {
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = base.OnCreateDialog(savedInstanceState);
            dialog.SetOnShowListener(this);
            return dialog;
        }

        public void OnShow(IDialogInterface dialog)
        {
            var bottomSheetDialog = (BottomSheetDialog)dialog;

            var view = bottomSheetDialog.FindViewById(Resource.Id.design_bottom_sheet);
            var behavior = BottomSheetBehavior.From(view);

            var layoutParams = view.LayoutParameters;

            int windowHeight = GetWindowHeight();
            if (layoutParams != null)
                layoutParams.Height = windowHeight;

            view.LayoutParameters = layoutParams;
            view.SetBackgroundResource(Resource.Drawable.bottom_sheet);

            behavior.State = BottomSheetBehavior.StateExpanded;
        }

        #region Height
        private int GetWindowHeight()
        {
            var displayMetrics = new DisplayMetrics();
            ((Activity)Context).WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

            int actionBarHeight = GetActionbarHeight();

            var statusBarHeight = GetStatusBarHeight();

            return displayMetrics.HeightPixels - actionBarHeight - statusBarHeight - 40;
        }

        private int GetActionbarHeight()
        {
            TypedValue tv = new TypedValue();

            int actionBarHeight = 0;
            if (Context.Theme.ResolveAttribute(Resource.Attribute.actionBarSize, tv, true))
                actionBarHeight = TypedValue.ComplexToDimensionPixelSize(tv.Data, Resources.DisplayMetrics);

            return actionBarHeight;
        }

        /// <summary>
        /// вернуть высоту status bar
        /// </summary>
        /// <returns></returns>
        public int GetStatusBarHeight()
        {
            var result = 0;
            var resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
                result = Resources.GetDimensionPixelSize(resourceId);

            return result / 2;
        }
        #endregion
    }

}
