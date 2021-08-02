using System;

using Android.Content;
using AndroidX.AppCompat.App;
using AndroidX.Preference;

using Java.Util;

namespace NearWallet.Doid.Framework.Helpers
{
    public class PreferencesHelper
    {
        public const string LocaleKey = "LocaleKey";
        public const string ThemeKey = "ThemeKey";

        public const string RuLocale = "ru";
        public const string EnLocale = "en";
        public const string Default = EnLocale;

        public static event EventHandler<string> ChangeLocale = delegate { };

        public static void SetPreference(Context context, string key, string value)
        {
            var ensharedPreferences = context.GetSharedPreferences(key, FileCreationMode.Private);
            var eneditor = ensharedPreferences.Edit();
            eneditor.PutString(key, value);
            eneditor.Commit();
        }

        public static string GetPreference(Context context, string key)
        {
            var sharedPreferences = context.GetSharedPreferences(key, FileCreationMode.Private);
            var value = sharedPreferences.GetString(key, string.Empty);

            return value;
        }

        #region Theme
        public static void SetAppTheme(Context context, int theme)
        {
            SetPreference(context, ThemeKey, theme.ToString());

            //((AppCompatActivity)context).Delegate.SetLocalNightMode(theme);
           AppCompatDelegate.DefaultNightMode = theme;
        }

        public static int GetAppTheme(Context context)
        {
            var theme = GetPreference(context, ThemeKey);

            if (string.IsNullOrEmpty(theme)) return (int)ThemeType.System;

            return Convert.ToInt32(theme);
        }
        #endregion

        #region Locale
        public static void SetAppLocale(Context context, string locale)
        {
            SetPreference(context, LocaleKey, locale);

            ChangeLocale(context, locale);
        }

        public static string GetAppLocale(Context context)
        {
            var locale = GetPreference(context, LocaleKey);
            if (string.IsNullOrEmpty(locale))
            {
                var localeDef = Locale.Default.Language;
                return localeDef == RuLocale ? RuLocale : EnLocale;
            }

            return locale;
        }
        #endregion
    }



}
