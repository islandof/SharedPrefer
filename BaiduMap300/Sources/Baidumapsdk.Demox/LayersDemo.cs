using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;

namespace Baidumapsdk.Demox
{
    /**
     * 演示地图图层显示的控制方法
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_layers", ScreenOrientation = ScreenOrientation.Sensor)]
    public class LayersDemo : Activity
    {
        /**
          * MapView 是地图主控件
          */
        private MapView mMapView;
        private BaiduMap mBaiduMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_layers);
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
        }

        /**
         * 设置底图显示模式
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void SetMapMode(View view)
        {
            bool checkedX = ((RadioButton)view).Checked;
            switch (view.Id)
            {
                case Resource.Id.normal:
                    if (checkedX)
                        mBaiduMap.MapType = BaiduMap.MapTypeNormal;
                    break;
                case Resource.Id.statellite:
                    if (checkedX)
                        mBaiduMap.MapType = BaiduMap.MapTypeSatellite;
                    break;
            }
        }

        /**
         * 设置是否显示交通图
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void SetTraffic(View view)
        {
            mBaiduMap.TrafficEnabled = ((CheckBox)view).Checked;
        }

        protected override void OnPause()
        {
            // MapView的生命周期与Activity同步，当activity挂起时需调用MapView.onPause()
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            // MapView的生命周期与Activity同步，当activity恢复时需调用MapView.onResume()
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            // MapView的生命周期与Activity同步，当activity销毁时需调用MapView.destroy()
            mMapView.OnDestroy();
            base.OnDestroy();
        }
    }
}