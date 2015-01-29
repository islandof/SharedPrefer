using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Entity;
using Java.Interop;
using Newtonsoft.Json;
using Object = Java.Lang.Object;

namespace XamarinDemo
{
    /**
     * 演示覆盖物的用法
     */

    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
        Label = "@string/monitor_name_carposition", ScreenOrientation = ScreenOrientation.Sensor)]
    public class AreaDetailDialog : DialogFragment
    {
        /**
        * MapView 是地图主控件
        */
        private static readonly LatLng GEO_SHENGZHENG = new LatLng(22.560, 114.064);

        private readonly BitmapDescriptor bd = BitmapDescriptorFactory
            .FromResource(Resource.Drawable.icon_gcoding);

        // 初始化全局 bitmap 信息，不用时及时 Recycle
        private readonly BitmapDescriptor bdA = BitmapDescriptorFactory
            .FromResource(Resource.Drawable.icon_marka);

        private readonly BitmapDescriptor bdB = BitmapDescriptorFactory
            .FromResource(Resource.Drawable.icon_markb);

        private readonly BitmapDescriptor bdC = BitmapDescriptorFactory
            .FromResource(Resource.Drawable.icon_markc);

        private readonly BitmapDescriptor bdD = BitmapDescriptorFactory
            .FromResource(Resource.Drawable.icon_markd);

        private readonly BitmapDescriptor bdGround = BitmapDescriptorFactory
            .FromResource(Resource.Drawable.ground_overlay);

        private readonly BitmapDescriptor bdoff = BitmapDescriptorFactory
            .FromResource(Resource.Drawable.gray_car);

        private readonly BitmapDescriptor bdon = BitmapDescriptorFactory
            .FromResource(Resource.Drawable.green_car);

        private BaiduMap mBaiduMap;
        private WebClient mClient;
        private List<testCase4> mDataList;
        private InfoWindow mInfoWindow;
        private testCase5 mItem;
        private string mLocation;
        private MapView mMapView;
        private Marker mMarkerA;
        private Marker mMarkerB;
        private Marker mMarkerC;
        private Marker mMarkerD;
        private Uri mUrl;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            
            var view = inflater.Inflate(Resource.Layout.popup_Map, container, false);
            if (savedInstanceState != null)
            {
                savedInstanceState.GetString("area");
            }
            return view;
            
        }


        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}