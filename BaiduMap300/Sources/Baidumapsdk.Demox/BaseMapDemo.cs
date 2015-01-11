using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;

namespace Baidumapsdk.Demox
{
    /**
     * 演示MapView的基本用法
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_basemap", ScreenOrientation = ScreenOrientation.Sensor)]
    public class BaseMapDemo : Activity
    {
#pragma warning disable 0169, 0414
        private static readonly string LTAG = typeof(BaseMapDemo).Name;// new BaseMapDemo().Class.SimpleName
        private MapView mMapView;
        private BaiduMap mBaiduMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Intent intent = Intent;
            if (intent.HasExtra("x") && intent.HasExtra("y"))
            {
                // 当用intent参数时，设置中心点为指定点
                Bundle b = intent.Extras;
                LatLng p = new LatLng(b.GetDouble("y"), b.GetDouble("x"));
                mMapView = new MapView(this,
                        new BaiduMapOptions().MapStatus(new MapStatus.Builder()
                                .Target(p).Build()));
            }
            else
            {
                mMapView = new MapView(this, new BaiduMapOptions());
            }
            SetContentView(mMapView);
            mBaiduMap = mMapView.Map;
        }

        protected override void OnPause()
        {
            base.OnPause();
            // activity 暂停时同时暂停地图控件
            mMapView.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            // activity 恢复时同时恢复地图控件
            mMapView.OnResume();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            // activity 销毁时同时销毁地图控件
            mMapView.OnDestroy();
        }
    }
}