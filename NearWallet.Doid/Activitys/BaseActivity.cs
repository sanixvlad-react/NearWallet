
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using NearWallet.Doid.Framework;
using NearWallet.Doid.Framework.Helpers;
using Xamarin.Essentials;

namespace NearWallet.Doid.Activitys
{
    public class BaseActivity : AppCompatActivity
    {
        public string Locale;
        public int AppTheme => PreferencesHelper.GetAppTheme(this);

        protected override void AttachBaseContext(Context context)
        {
            Locale = PreferencesHelper.GetAppLocale(context);
            PreferencesHelper.ChangeLocale += PreferencesHelper_ChangeLocale;

            base.AttachBaseContext(AppContextWrapper.Wrap(context, Locale));
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                Window.DecorView.SystemUiVisibility = AppTheme == (int)ThemeType.Night ?
                                                    0 :
                                                    (StatusBarVisibility)SystemUiFlags.LightStatusBar;
            }
            catch { }
        }

        public void setDarkStatusIcon(bool bDark)
        {
            
        }


        public override void Recreate()
        {
            PreferencesHelper.ChangeLocale -= PreferencesHelper_ChangeLocale;

            base.Recreate();
        }

        private void PreferencesHelper_ChangeLocale(object sender, string e)
        {
            Recreate();
        }
    }
}
