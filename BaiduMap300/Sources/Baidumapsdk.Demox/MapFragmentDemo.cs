using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Com.Baidu.Mapapi.Map;

namespace Baidumapsdk.Demox
{
    [Android.App.Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_map_fragment", ScreenOrientation = ScreenOrientation.Sensor)]
    public class MapFragmentDemo : FragmentActivity
    {
#pragma warning disable 0169, 0414
        private static readonly string LTAG = typeof(MapFragmentDemo).Name;// new MapFragmentDemo().Class.SimpleName
        SupportMapFragment map;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_fragment);
            MapStatus ms = new MapStatus.Builder().Overlook(-20).Zoom(15).Build();
            BaiduMapOptions bo = new BaiduMapOptions().MapStatus(ms)
                    .CompassEnabled(false).ZoomControlsEnabled(false);
            map = SupportMapFragment.NewInstance(bo);
            FragmentManager manager = SupportFragmentManager;
            manager.BeginTransaction().Add(Resource.Id.map, map, "map_fragment").Commit();
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
        }

    }
}