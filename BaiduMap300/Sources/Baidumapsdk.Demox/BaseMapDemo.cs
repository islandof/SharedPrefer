using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;

namespace Baidumapsdk.Demox
{
    /**
     * ��ʾMapView�Ļ����÷�
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
                // ����intent����ʱ���������ĵ�Ϊָ����
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
            // activity ��ͣʱͬʱ��ͣ��ͼ�ؼ�
            mMapView.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            // activity �ָ�ʱͬʱ�ָ���ͼ�ؼ�
            mMapView.OnResume();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            // activity ����ʱͬʱ���ٵ�ͼ�ؼ�
            mMapView.OnDestroy();
        }
    }
}