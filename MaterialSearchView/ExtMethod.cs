
using Java.Lang;
using Android.Graphics;

using AndroidX.Annotations;

namespace MaterialSearchView
{
    internal static class ExtMethod
    {
        public static Color ToColor([ColorInt] this int colorArgb)
        {
            return Color.ParseColor(String.Format("#%06X", (Object)(16777215 & colorArgb)));
        }
    }
}