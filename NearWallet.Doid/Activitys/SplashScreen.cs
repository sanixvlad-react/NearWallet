
using System.Threading.Tasks;

using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;
using Android.Content.PM;

using AndroidX.AppCompat.App;

using NearWallet.Doid.Framework.Helpers;

using Plugin.CurrentActivity;

namespace NearWallet.Doid.Activitys
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Splash", ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize,
        MainLauncher = true)]
    public class SplashScreen : AppCompatActivity
    {
        public static ContextWrapper wraper { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            OverridePendingTransition(Resource.Animation.fade_in, Resource.Animation.fade_out);

            ChooseActivity();
        }

        protected override void AttachBaseContext(Context context)
        {
            var theme = PreferencesHelper.GetAppTheme(context);
            AppCompatDelegate.DefaultNightMode = theme;

            base.AttachBaseContext(context);
        }

        private void ChooseActivity()
        {
            Task.Delay(1500);

            //StartActivity(new Intent(this, typeof(MainActivity)));
            StartActivity(new Intent(this, typeof(ListWalletsActivity)));
        }

        /// <summary>
        /// показать сообщение
        /// </summary>
        /// <param name="context"></param>
        /// <param name="text"></param>
        public static void GetToast(Activity context, string text)
        {
            Toast.MakeText(context, text, ToastLength.Long).Show();
        }

    }

}
