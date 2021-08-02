
using Android.OS;
using Android.Content;
using Android.Annotation;
using Android.Content.Res;

using Java.Util;

namespace NearWallet.Doid.Framework.Helpers
{
    public class AppContextWrapper //: ContextWrapper
    {
        //public AppContextWrapper(Context context) : base(context) { }

        public static ContextWrapper Wrap(Context context, string locale)
        {
            Resources res = context.Resources;
            Configuration configuration = res.Configuration;
            var newLocale = new Locale(locale);

            if (Build.VERSION.SdkInt == BuildVersionCodes.N)
            {
                
                configuration.SetLocale(newLocale);

                LocaleList localeList = new LocaleList(newLocale);
                LocaleList.Default = localeList;
                configuration.Locales = localeList;

                context = context.CreateConfigurationContext(configuration);

            }
            else if (Build.VERSION.SdkInt == BuildVersionCodes.JellyBeanMr1)
            {
                configuration.SetLocale(newLocale);
                context = context.CreateConfigurationContext(configuration);

            }
            else
            {
                configuration.Locale = newLocale;
                res.UpdateConfiguration(configuration, res.DisplayMetrics);
            }

            return new ContextWrapper(context);
        }

    }

}
