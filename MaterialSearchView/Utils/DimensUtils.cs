using System;

using Android.Util;
using Android.Content;

using AndroidX.Annotations;

namespace MaterialSearchView.Utils
{
    public static class DimensUtils
    {
        public static int ConvertDpToPx(int dp, [NonNull] Context context)
        {
            return (int)Math.Round((double)TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)dp, context.Resources.DisplayMetrics));
        }

        public static float ConvertDpToPx(float dp, [NonNull] Context context)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }
    }
}