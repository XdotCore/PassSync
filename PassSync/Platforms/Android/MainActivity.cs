﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using MauiBlazor3.Platform.Android;
using Microsoft.Maui;

namespace PassSync {
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            WebViewSoftInputPatch.Initialize();
        }
    }
}
