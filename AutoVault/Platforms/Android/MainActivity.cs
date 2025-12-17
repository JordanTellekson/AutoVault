using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace AutoVault
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        ConfigurationChanges =
            ConfigChanges.ScreenSize |
            ConfigChanges.Orientation |
            ConfigChanges.UiMode |
            ConfigChanges.ScreenLayout |
            ConfigChanges.SmallestScreenSize |
            ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window?.SetDecorFitsSystemWindows(false);

            Window?.SetStatusBarColor(Android.Graphics.Color.Transparent);

            Window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
        }
    }
}