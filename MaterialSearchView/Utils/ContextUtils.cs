using Android.App;
using Android.Util;
using Android.Views;
using Android.Content;
using Android.Views.InputMethods;

using AndroidX.Annotations;

namespace MaterialSearchView.Utils
{
    public static class ContextUtils
    {
        [Nullable]
        public static Activity ScanForActivity([NonNull] Context context)
        {
            if (context is Activity activity)
                return activity;
            if (context is ContextWrapper contextWrapper)
                return ContextUtils.ScanForActivity(contextWrapper.BaseContext);
            return (Activity)null;
        }

        [ColorInt]
        public static int GetPrimaryColor([NonNull] Context context)
        {
            TypedValue outValue = new TypedValue();
            context.Theme.ResolveAttribute(Resource.Attribute.colorPrimary, outValue, true);
            return outValue.Data;
        }

        [ColorInt]
        public static int GetAccentColor([NonNull] Context context)
        {
            TypedValue outValue = new TypedValue();
            context.Theme.ResolveAttribute(Resource.Attribute.colorAccent, outValue, true);
            return outValue.Data;
        }

        public static void ShowKeyboard([NonNull] View view)
        {
            view.RequestFocus();
            ((InputMethodManager)view.Context.GetSystemService("input_method"))?.ShowSoftInput(view, ShowFlags.Forced);
        }

        public static void HideKeyboard([NonNull] View view)
        {
            ((InputMethodManager)view.Context.GetSystemService("input_method"))?.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
        }
    }
}