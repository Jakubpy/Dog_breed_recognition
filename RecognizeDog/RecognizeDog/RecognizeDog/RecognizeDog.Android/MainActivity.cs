using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.Collections.ObjectModel;
using Java.Lang;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Plugin.Media;
using System.Threading.Tasks;
using System.Collections.Generic;
using Plugin.Media.Abstractions;
using RecognizeDog.Data;

namespace RecognizeDog.Droid
{
    [Activity(Label = "RecognizeDog", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            MainPage.imagesPath = Application.Context.GetExternalFilesDir("Pictures") + "/RecognizeDog";
            CheckDir(MainPage.imagesPath);
            JsonManager.SetPath(Application.Context.GetExternalFilesDir("data").ToString());
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            
            //LoadTreeViewItems();
        }
        private void CheckDir(string path)
        {
            bool exist = Directory.Exists(path);
            if (!exist)
            {
                Directory.CreateDirectory(path);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}