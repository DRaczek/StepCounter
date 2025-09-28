using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using StepCounter.Platforms.Android;
using System.Runtime.Versioning;

namespace StepCounter
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (CheckSelfPermission(Android.Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                RequestPermissions(new[] { Android.Manifest.Permission.PostNotifications }, 0);
            }

            var intent = new Intent(this, typeof(PedometerForegroundService));
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                StartForegroundService(intent);
            else
                StartService(intent);
        }
    }
}
