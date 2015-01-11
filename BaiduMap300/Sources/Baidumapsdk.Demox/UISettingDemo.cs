using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;

namespace Baidumapsdk.Demox
{
    /**
     * ��ʾ��ͼUI���ƹ���
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_ui", ScreenOrientation = ScreenOrientation.Sensor)]
    public class UISettingDemo : Activity
    {
        /**
         * MapView �ǵ�ͼ���ؼ�
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
         * �Ƿ�������������
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SetZoomEnable(View v)
        {
            mUiSettings.ZoomGesturesEnabled = ((CheckBox)v).Checked;
        }

        /**
         * �Ƿ�����ƽ������
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SetScrollEnable(View v)
        {
            mUiSettings.ScrollGesturesEnabled = ((CheckBox)v).Checked;
        }

        /**
         * �Ƿ�������ת����
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SetRotateEnable(View v)
        {
            mUiSettings.RotateGesturesEnabled = ((CheckBox)v).Checked;
        }

        /**
         * �Ƿ����ø�������
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SetOverlookEnable(View v)
        {
            mUiSettings.OverlookingGesturesEnabled = ((CheckBox)v).Checked;
        }

        /**
         * �Ƿ�����ָ����ͼ��
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
             *  MapView������������Activityͬ������activity����ʱ�����MapView.OnPause()
             */
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            /**
             *  MapView������������Activityͬ������activity�ָ�ʱ�����MapView.OnResume()
             */
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            /**
             *  MapView������������Activityͬ������activity����ʱ�����MapView.destroy()
             */
            mMapView.OnDestroy();
            base.OnDestroy();
        }
    }
}