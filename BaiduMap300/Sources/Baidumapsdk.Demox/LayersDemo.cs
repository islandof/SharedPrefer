using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;

namespace Baidumapsdk.Demox
{
    /**
     * ��ʾ��ͼͼ����ʾ�Ŀ��Ʒ���
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_layers", ScreenOrientation = ScreenOrientation.Sensor)]
    public class LayersDemo : Activity
    {
        /**
          * MapView �ǵ�ͼ���ؼ�
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
         * ���õ�ͼ��ʾģʽ
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
         * �����Ƿ���ʾ��ͨͼ
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
            // MapView������������Activityͬ������activity����ʱ�����MapView.onPause()
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            // MapView������������Activityͬ������activity�ָ�ʱ�����MapView.onResume()
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            // MapView������������Activityͬ������activity����ʱ�����MapView.destroy()
            mMapView.OnDestroy();
            base.OnDestroy();
        }
    }
}