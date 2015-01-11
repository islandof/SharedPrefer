using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Com.Baidu.Location;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;

namespace Baidumapsdk.Demox
{
    /**
     * ��demo����չʾ��ν�϶�λSDKʵ�ֶ�λ����ʹ��MyLocationOverlay���ƶ�λλ�� ͬʱչʾ���ʹ���Զ���ͼ����Ʋ����ʱ��������
     * 
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_location", ScreenOrientation = ScreenOrientation.Sensor)]
    public class LocationOverlayDemo : Activity
    {
        // ��λ���
        LocationClient mLocClient;
        public MyLocationListenner myListener = new MyLocationListenner();
        private MyLocationConfigeration.LocationMode mCurrentMode;
        BitmapDescriptor mCurrentMarker;

        MapView mMapView;
        BaiduMap mBaiduMap;

        // UI���
        RadioGroup.IOnCheckedChangeListener radioButtonListener;
        Button requestLocButton;
        bool isFirstLoc = true;// �Ƿ��״ζ�λ

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_location);
            myListener = new MyLocationListenner(this);
            requestLocButton = FindViewById<Button>(Resource.Id.button1);
            mCurrentMode = MyLocationConfigeration.LocationMode.Normal;
            requestLocButton.Text = "��ͨ";
            requestLocButton.Click += delegate
            {
                if (mCurrentMode.Equals(MyLocationConfigeration.LocationMode.Normal))
                {
                    requestLocButton.Text = "����";
                    mCurrentMode = MyLocationConfigeration.LocationMode.Following;
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, mCurrentMarker));
                }
                else if (mCurrentMode.Equals(MyLocationConfigeration.LocationMode.Compass))
                {
                    requestLocButton.Text = "��ͨ";
                    mCurrentMode = MyLocationConfigeration.LocationMode.Normal;
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, mCurrentMarker));
                }
                else if (mCurrentMode.Equals(MyLocationConfigeration.LocationMode.Following))
                {
                    requestLocButton.Text = "����";
                    mCurrentMode = MyLocationConfigeration.LocationMode.Compass;
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, mCurrentMarker));
                }

                #region [ �ӵ�, ���˼�������, ��ʱ�޷���C#ʵ��, ʹ������� if/else... ]
                //switch (mCurrentMode)
                //{
                //    case MyLocationConfigeration.LocationMode.Normal:
                //        requestLocButton.Text = "����";
                //        mCurrentMode = MyLocationConfigeration.LocationMode.Following;
                //        mBaiduMap
                //                .SetMyLocationConfigeration(new MyLocationConfigeration(
                //                        mCurrentMode, true, mCurrentMarker));
                //        break;
                //    case MyLocationConfigeration.LocationMode.Compass:
                //        requestLocButton.Text = "��ͨ";
                //        mCurrentMode = MyLocationConfigeration.LocationMode.Normal;
                //        mBaiduMap
                //                .SetMyLocationConfigeration(new MyLocationConfigeration(
                //                        mCurrentMode, true, mCurrentMarker));
                //        break;
                //    case MyLocationConfigeration.LocationMode.Following:
                //        requestLocButton.Text = "����";
                //        mCurrentMode = MyLocationConfigeration.LocationMode.Compass;
                //        mBaiduMap
                //                .SetMyLocationConfigeration(new MyLocationConfigeration(
                //                        mCurrentMode, true, mCurrentMarker));
                //        break;
                //}
                #endregion
            };

            RadioGroup group = this.FindViewById<RadioGroup>(Resource.Id.radioGroup);
            // group.CheckedChange += delegate(object sender, RadioGroup.CheckedChangeEventArgs args) { };
            group.CheckedChange += (sender, args) =>
            {
                int CheckedId = args.CheckedId;

                if (CheckedId == Resource.Id.defaulticon)
                {
                    // ����null�򣬻ָ�Ĭ��ͼ��
                    mCurrentMarker = null;
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, null));
                }
                if (CheckedId == Resource.Id.customicon)
                {
                    // �޸�Ϊ�Զ���marker
                    mCurrentMarker = BitmapDescriptorFactory
                            .FromResource(Resource.Drawable.icon_geo);
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, mCurrentMarker));
                }
            };

            // ��ͼ��ʼ��
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
            // ������λͼ��
            mBaiduMap.MyLocationEnabled = true;
            // ��λ��ʼ��
            mLocClient = new LocationClient(this);
            mLocClient.RegisterLocationListener(myListener);
            LocationClientOption option = new LocationClientOption();
            option.OpenGps = true;// ��gps
            option.CoorType = "bd09ll"; // ������������
            option.ScanSpan = 1000;
            mLocClient.LocOption = option;
            mLocClient.Start();
        }

        /**
	     * ��λSDK��������
	     */
        public class MyLocationListenner : Java.Lang.Object, IBDLocationListener
        {
            LocationOverlayDemo locationOverlayDemo;

            public MyLocationListenner()
            {
            }

            public MyLocationListenner(LocationOverlayDemo locationOverlayDemo)
            {
                this.locationOverlayDemo = locationOverlayDemo;
            }

            public void OnReceiveLocation(BDLocation location)
            {
                // map view ���ٺ��ڴ����½��յ�λ��
                if (location == null || locationOverlayDemo.mMapView == null)
                    return;
                MyLocationData locData = new MyLocationData.Builder()
                        .Accuracy(location.Radius)
                    // �˴����ÿ����߻�ȡ���ķ�����Ϣ��˳ʱ��0-360
                        .Direction(100).Latitude(location.Latitude)
                        .Longitude(location.Longitude).Build();
                locationOverlayDemo.mBaiduMap.SetMyLocationData(locData);
                if (locationOverlayDemo.isFirstLoc)
                {
                    locationOverlayDemo.isFirstLoc = false;
                    LatLng ll = new LatLng(location.Latitude,
                            location.Longitude);
                    MapStatusUpdate u = MapStatusUpdateFactory.NewLatLng(ll);
                    locationOverlayDemo.mBaiduMap.AnimateMapStatus(u);
                }
            }

            public void OnReceivePoi(BDLocation poiLocation)
            {
            }
        }

        protected override void OnPause()
        {
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            // �˳�ʱ���ٶ�λ
            mLocClient.Stop();
            // �رն�λͼ��
            mBaiduMap.MyLocationEnabled = false;
            mMapView.OnDestroy();
            mMapView = null;
            base.OnDestroy();
        }
    }
}