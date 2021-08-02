using Android.Util;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

using Java.Lang;
using Java.Lang.Reflect;

using AndroidX.Annotations;
using AndroidX.Core.Content;

namespace MaterialSearchView.Utils
{
    public static class EditTextReflectionUtils
    {
        private const string Tag = "EditTextReflectionUtils";
        private const string EDIT_TEXT_FIELD_CURSOR_DRAWABLE_RES = "mCursorDrawableRes";
        private const string EDIT_TEXT_FIELD_EDITOR = "mEditor";
        private const string EDIT_TEXT_FIELD_CURSOR_DRAWABLE = "mCursorDrawable";

        public static void SetCursorDrawable([NonNull] EditText editText, int drawable)
        {
            try
            {
                Field declaredField = Class.FromType(typeof(TextView)).GetDeclaredField("mCursorDrawableRes");
                declaredField.Accessible = true;
                declaredField.Set(editText, drawable);
            }
            catch (Exception ex)
            {
                Log.Error(nameof(EditTextReflectionUtils), ex.Message, (object)ex);
            }
        }

        public static void SetCursorColor([NonNull] EditText editText, [ColorInt] int color)
        {
            try
            {
                Field declaredField1 = Class.FromType(typeof(EditText)).GetDeclaredField("mCursorDrawableRes");
                declaredField1.Accessible = true;
                int num = declaredField1.GetInt(editText);
                Field declaredField2 = Class.FromType(typeof(EditText)).GetDeclaredField("mEditor");
                declaredField2.Accessible = true;
                Object @object = declaredField2.Get(editText);
                Drawable drawable = ContextCompat.GetDrawable(editText.Context, num);
                drawable.SetColorFilter(color.ToColor(), PorterDuff.Mode.SrcIn);
                Drawable[] drawableArray = new Drawable[2]
                {
                    drawable,
                    drawable
                };
                Field declaredField3 = @object.Class.GetDeclaredField("mCursorDrawable");
                declaredField3.Accessible = true;
                declaredField3.Set(@object, drawableArray);
            }
            catch (Exception ex)
            {
                Log.Error(nameof(EditTextReflectionUtils), ex.Message, (object)ex);
            }
        }
    }
}