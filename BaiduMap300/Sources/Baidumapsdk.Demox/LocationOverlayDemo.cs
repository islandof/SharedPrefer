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
     * 此demo用来展示如何结合定位SDK实现定位，并使用MyLocationOverlay绘制定位位置 同时展示如何使用自定义图标绘制并点击时弹出泡泡
     * 
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_location", ScreenOrientation = ScreenOrientation.Sensor)]
    public class LocationOverlayDemo : Activity
    {
        // 定位相关
        LocationClient mLocClient;
        public MyLocationListenner myListener = new MyLocationListenner();
        private MyLocationConfigeration.LocationMode mCurrentMode;
        BitmapDescriptor mCurrentMarker;

        MapView mMapView;
        BaiduMap mBaiduMap;

        // UI相关
        RadioGroup.IOnCheckedChangeListener radioButtonListener;
        Button requestLocButton;
        bool isFirstLoc = true;// 是否首次定位

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_location);
            myListener = new MyLocationListenner(this);
            requestLocButton = FindViewById<Button>(Resource.Id.button1);
            mCurrentMode = MyLocationConfigeration.LocationMode.Normal;
            requestLocButton.Text = "普通";
            requestLocButton.Click += delegate
            {
                if (mCurrentMode.Equals(MyLocationConfigeration.LocationMode.Normal))
                {
                    requestLocButton.Text = "跟随";
                    mCurrentMode = MyLocationConfigeration.LocationMode.Following;
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, mCurrentMarker));
                }
                else if (mCurrentMode.Equals(MyLocationConfigeration.LocationMode.Compass))
                {
                    requestLocButton.Text = "普通";
                    mCurrentMode = MyLocationConfigeration.LocationMode.Normal;
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, mCurrentMarker));
                }
                else if (mCurrentMode.Equals(MyLocationConfigeration.LocationMode.Following))
                {
                    requestLocButton.Text = "罗盘";
                    mCurrentMode = MyLocationConfigeration.LocationMode.Compass;
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, mCurrentMarker));
                }

                #region [ 坑爹, 本人技术不够, 暂时无法用C#实现, 使用上面的 if/else... ]
                //switch (mCurrentMode)
                //{
                //    case MyLocationConfigeration.LocationMode.Normal:
                //        requestLocButton.Text = "跟随";
                //        mCurrentMode = MyLocationConfigeration.LocationMode.Following;
                //        mBaiduMap
                //                .SetMyLocationConfigeration(new MyLocationConfigeration(
                //                        mCurrentMode, true, mCurrentMarker));
                //        break;
                //    case MyLocationConfigeration.LocationMode.Compass:
                //        requestLocButton.Text = "普通";
                //        mCurrentMode = MyLocationConfigeration.LocationMode.Normal;
                //        mBaiduMap
                //                .SetMyLocationConfigeration(new MyLocationConfigeration(
                //                        mCurrentMode, true, mCurrentMarker));
                //        break;
                //    case MyLocationConfigeration.LocationMode.Following:
                //        requestLocButton.Text = "罗盘";
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
                    // 传入null则，恢复默认图标
                    mCurrentMarker = null;
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, null));
                }
                if (CheckedId == Resource.Id.customicon)
                {
                    // 修改为自定义marker
                    mCurrentMarker = BitmapDescriptorFactory
                            .FromResource(Resource.Drawable.icon_geo);
                    mBaiduMap
                            .SetMyLocationConfigeration(new MyLocationConfigeration(
                                    mCurrentMode, true, mCurrentMarker));
                }
            };

            // 地图初始化
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
            // 开启定位图层
            mBaiduMap.MyLocationEnabled = true;
            // 定位初始化
            mLocClient = new LocationClient(this);
            mLocClient.RegisterLocationListener(myListener);
            LocationClientOption option = new LocationClientOption();
            option.OpenGps = true;// 打开gps
            option.CoorType = "bd09ll"; // 设置坐标类型
            option.ScanSpan = 1000;
            mLocClient.LocOption = option;
            mLocClient.Start();
        }

        /**
	     * 定位SDK监听函数
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
                // map view 销毁后不在处理新接收的位置
                if (location == null || locationOverlayDemo.mMapView == null)
                    return;
                MyLocationData locData = new MyLocationData.Builder()
                        .Accuracy(location.Radius)
                    // 此处设置开发者获取到的方向信息，顺时针0-360
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
            // 退出时销毁定位
            mLocClient.Stop();
            // 关闭定位图层
            mBaiduMap.MyLocationEnabled = false;
            mMapView.OnDestroy();
            mMapView = null;
            base.OnDestroy();
        }
    }
}