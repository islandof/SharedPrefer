using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;

namespace Baidumapsdk.Demox
{
    /**
     * 演示地图UI控制功能
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_ui", ScreenOrientation = ScreenOrientation.Sensor)]
    public class UISettingDemo : Activity
    {
        /**
         * MapView 是地图主控件
         */
        private MapView mMapView;
        private BaiduMap mBaiduMap;
        private UiSettings mUiSettings;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_uisetting);

            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
            mUiSettings = mBaiduMap.UiSettings;

            MapStatus ms = new MapStatus.Builder().Overlook(-30).Build();
            MapStatusUpdate u = MapStatusUpdateFactory.NewMapStatus(ms);
            mBaiduMap.AnimateMapStatus(u, 1000);
        }

        /**
         * 是否启用缩放手势
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SetZoomEnable(View v)
        {
            mUiSettings.ZoomGesturesEnabled = ((CheckBox)v).Checked;
        }

        /**
         * 是否启用平移手势
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SetScrollEnable(View v)
        {
            mUiSettings.ScrollGesturesEnabled = ((CheckBox)v).Checked;
        }

        /**
         * 是否启用旋转手势
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SetRotateEnable(View v)
        {
            mUiSettings.RotateGesturesEnabled = ((CheckBox)v).Checked;
        }

        /**
         * 是否启用俯视手势
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SetOverlookEnable(View v)
        {
            mUiSettings.OverlookingGesturesEnabled = ((CheckBox)v).Checked;
        }

        /**
         * 是否启用指南针图层
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void SetCompassEnable(View v)
        {
            mUiSettings.CompassEnabled = ((CheckBox)v).Checked;
        }

        protected override void OnPause()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity挂起时需调用MapView.OnPause()
             */
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity恢复时需调用MapView.OnResume()
             */
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            /**
             *  MapView的生命周期与Activity同步，当activity销毁时需调用MapView.destroy()
             */
            mMapView.OnDestroy();
            base.OnDestroy();
        }
    }
}