using System;
using Android.App;
using Android.OS;

namespace Kangaroo.Droid
{
    [Activity(Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true, Label = "KangarooCare")]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                this.StartActivity(typeof(MainActivity));
                Finish();
                OverridePendingTransition(0,0);
            }
            catch (Exception ex)
            {
                new AlertDialog.Builder(this)
                        .SetPositiveButton("Yes", (sender, args) =>
                        {

                        })
                        .SetNegativeButton("No", (sender, args) =>
                        {
                            // User pressed no 
                        })
                        .SetMessage(ex.Message)
                        .SetTitle("Kangaroo")
                        .Show();
            }
        }
    }
}