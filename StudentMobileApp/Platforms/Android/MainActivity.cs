using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotification;

namespace StudentMobileApp
{
    [Activity(Theme = "@style/Maui.SplashTheme",
              MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation |
                                     ConfigChanges.UiMode | ConfigChanges.ScreenLayout |
                                     ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            
            LocalNotificationCenter.CreateNotificationChannel();

            
            if (OperatingSystem.IsAndroidVersionAtLeast(33))
            {
                var permission = Android.Manifest.Permission.PostNotifications;
                if (CheckSelfPermission(permission) != Permission.Granted)
                {
                    RequestPermissions(new[] { permission }, 0);
                }
            }

        }
    }
}


