﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Com.Baidu.Mapapi.Search.Core;
using Com.Baidu.Mapapi.Search.Geocode;
using Java.Interop;
using Java.Lang;

namespace XamarinDemo.Maps
{
    /**
     * 此demo用来展示如何进行地理编码搜索（用地址检索坐标）、反地理编码搜索（用坐标检索地址）
     */

    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
        Label = "@string/demo_name_geocode", ScreenOrientation = ScreenOrientation.Sensor)]
    public class GeoCoderDemo : Activity, IOnGetGeoCoderResultListener
    {
        private BaiduMap mBaiduMap;
        private MapView mMapView;
        private GeoCoder mSearch; // 搜索模块，也可去掉地图模块独立使用

        public void OnGetGeoCodeResult(GeoCodeResult result)
        {
            if (result == null || result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "抱歉，未能找到结果", ToastLength.Long)
                    .Show();
            }
            mBaiduMap.Clear();
            mBaiduMap.AddOverlay(new MarkerOptions().InvokePosition(result.Location)
                .InvokeIcon(BitmapDescriptorFactory
                    .FromResource(Resource.Drawable.icon_marka)));
            mBaiduMap.SetMapStatus(MapStatusUpdateFactory.NewLatLng(result
                .Location));
            string strInfo = string.Format("纬度：{0:F6} 经度：{1:F6}",
                result.Location.Latitude, result.Location.Longitude);
            Toast.MakeText(this, strInfo, ToastLength.Long).Show();
        }

        public void OnGetReverseGeoCodeResult(ReverseGeoCodeResult result)
        {
            if (result == null || result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "抱歉，未能找到结果", ToastLength.Long)
                    .Show();
            }
            mBaiduMap.Clear();
            mBaiduMap.AddOverlay(new MarkerOptions().InvokePosition(result.Location)
                .InvokeIcon(BitmapDescriptorFactory
                    .FromResource(Resource.Drawable.icon_marka)));
            mBaiduMap.SetMapStatus(MapStatusUpdateFactory.NewLatLng(result
                .Location));
            Toast.MakeText(this, result.Address,
                ToastLength.Long).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_geocoder);
            ICharSequence titleLable = new String("地理编码功能");
            Title = titleLable.ToString();

            // 地图初始化
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;

            // 初始化搜索模块，注册事件监听
            mSearch = GeoCoder.NewInstance();
            mSearch.SetOnGetGeoCodeResultListener(this);
        }

        /**
         * 发起搜索
         * 
         * @param v
        */

        [Export]
        public void SearchButtonProcess(View v)
        {
            if (v.Id == Resource.Id.reversegeocode)
            {
                var lat = FindViewById<EditText>(Resource.Id.lat);
                var lon = FindViewById<EditText>(Resource.Id.lon);
                var ptCenter = new LatLng((float.Parse(lat.Text)), (float.Parse(lon.Text)));
                // 反Geo搜索
                mSearch.ReverseGeoCode(new ReverseGeoCodeOption()
                    .Location(ptCenter));
            }
            else if (v.Id == Resource.Id.geocode)
            {
                var editCity = FindViewById<EditText>(Resource.Id.city);
                var editGeoCodeKey = FindViewById<EditText>(Resource.Id.geocodekey);
                // Geo搜索
                mSearch.Geocode(new GeoCodeOption().City(
                    editCity.Text).Address(
                        editGeoCodeKey.Text));
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
            mMapView.OnDestroy();
            mSearch.Destroy();
            base.OnDestroy();
        }
    }
}