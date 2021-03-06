﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;

namespace XamarinDemo.Maps
{
    /**
     * 在一个Activity中展示多个地图
     */

    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
        Label = "@string/demo_name_multimap", ScreenOrientation = ScreenOrientation.Sensor)]
    public class MutiMapViewDemo : FragmentActivity
    {
        private static readonly LatLng GEO_BEIJING = new LatLng(39.945, 116.404);
        private static readonly LatLng GEO_SHANGHAI = new LatLng(31.227, 121.481);
        private static readonly LatLng GEO_GUANGZHOU = new LatLng(23.155, 113.264);
        private static readonly LatLng GEO_SHENGZHENG = new LatLng(22.560, 114.064);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_multimap);
            InitMap();
        }

        /**
         * 初始化Map
         */

        private void InitMap()
        {
            MapStatusUpdate u1 = MapStatusUpdateFactory.NewLatLng(GEO_BEIJING);
            var map1 = Extensions.JavaCast<SupportMapFragment>(SupportFragmentManager
                .FindFragmentById(Resource.Id.map1));
            map1.BaiduMap.SetMapStatus(u1);

            MapStatusUpdate u2 = MapStatusUpdateFactory.NewLatLng(GEO_SHANGHAI);
            var map2 = Extensions.JavaCast<SupportMapFragment>(SupportFragmentManager
                .FindFragmentById(Resource.Id.map2));
            map2.BaiduMap.SetMapStatus(u2);

            MapStatusUpdate u3 = MapStatusUpdateFactory.NewLatLng(GEO_GUANGZHOU);
            var map3 = Extensions.JavaCast<SupportMapFragment>(SupportFragmentManager
                .FindFragmentById(Resource.Id.map3));
            map3.BaiduMap.SetMapStatus(u3);

            MapStatusUpdate u4 = MapStatusUpdateFactory.NewLatLng(GEO_SHENGZHENG);
            var map4 = Extensions.JavaCast<SupportMapFragment>(SupportFragmentManager
                .FindFragmentById(Resource.Id.map4));
            map4.BaiduMap.SetMapStatus(u4);
        }
    }
}