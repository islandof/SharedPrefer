using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;

namespace XamarinDemo.Maps
{
    /**
     * 此demo用来展示如何在地图上用GraphicsOverlay添加点、线、多边形、圆 同时展示如何在地图上用TextOverlay添加文字
     * 
     */

    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
        Label = "@string/demo_name_geometry", ScreenOrientation = ScreenOrientation.Sensor)]
    public class GeometryDemo : Activity
    {
        // 地图相关
        private Button clearBtn;
        private BaiduMap mBaiduMap;
        private MapView mMapView;
        // UI相关
        private Button resetBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_geometry);
            // 初始化地图
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
            // UI初始化
            clearBtn = FindViewById<Button>(Resource.Id.button1);
            resetBtn = FindViewById<Button>(Resource.Id.button2);

            clearBtn.Click += delegate { ClearClick(); };
            resetBtn.Click += delegate { ResetClick(); };

            // 界面加载时添加绘制图层
            AddCustomElementsDemo();
        }

        /**
         * 添加点、线、多边形、圆、文字
         */

        public void AddCustomElementsDemo()
        {
            // 添加折线
            var p1 = new LatLng(39.97923, 116.357428);
            var p2 = new LatLng(39.94923, 116.397428);
            var p3 = new LatLng(39.97923, 116.437428);
            IList<LatLng> points = new List<LatLng>();
            points.Add(p1);
            points.Add(p2);
            points.Add(p3);

            // Android.Graphics.Color.Argb(0xAA, 0xFF, 0x00, 0x00).ToArgb();
            // Android.Graphics.Color.ParseColor("#AAFF0000").ToArgb();

            OverlayOptions ooPolyline = new PolylineOptions().InvokeWidth(10)
                .InvokeColor(Color.ParseColor("#AAFF0000").ToArgb()).InvokePoints(points);
            mBaiduMap.AddOverlay(ooPolyline);
            // 添加弧线
            OverlayOptions ooArc = new ArcOptions().InvokeColor(Color.ParseColor("#AA00FF00").ToArgb()).InvokeWidth(4)
                .Points(p1, p2, p3);
            mBaiduMap.AddOverlay(ooArc);
            // 添加圆
            var llCircle = new LatLng(39.90923, 116.447428);
            OverlayOptions ooCircle = new CircleOptions().InvokeFillColor(0x000000FF)
                .InvokeCenter(llCircle).InvokeStroke(new Stroke(5, Color.ParseColor("#AA000000").ToArgb()))
                .InvokeRadius(1400);
            mBaiduMap.AddOverlay(ooCircle);

            var llDot = new LatLng(39.98923, 116.397428);
            OverlayOptions ooDot = new DotOptions().InvokeCenter(llDot).InvokeRadius(6)
                .InvokeColor(Color.ParseColor("#FF0000FF").ToArgb());
            mBaiduMap.AddOverlay(ooDot);
            // 添加多边形
            var pt1 = new LatLng(39.93923, 116.357428);
            var pt2 = new LatLng(39.91923, 116.327428);
            var pt3 = new LatLng(39.89923, 116.347428);
            var pt4 = new LatLng(39.89923, 116.367428);
            var pt5 = new LatLng(39.91923, 116.387428);
            var pts = new List<LatLng>();
            pts.Add(pt1);
            pts.Add(pt2);
            pts.Add(pt3);
            pts.Add(pt4);
            pts.Add(pt5);
            OverlayOptions ooPolygon = new PolygonOptions().InvokePoints(pts)
                .InvokeStroke(new Stroke(5, Color.ParseColor("#AA00FF00").ToArgb()))
                .InvokeFillColor(Color.ParseColor("#AAFFFF00").ToArgb());
            mBaiduMap.AddOverlay(ooPolygon);
            // 添加文字
            var llText = new LatLng(39.86923, 116.397428);
            OverlayOptions ooText = new TextOptions().InvokeBgColor(Color.ParseColor("#AAFFFF00").ToArgb())
                .InvokeFontSize(24)
                .InvokeFontColor(Color.ParseColor("#FFFF00FF").ToArgb())
                .InvokeText("百度地图SDK")
                .InvokeRotate(-30)
                .InvokePosition(llText);
            mBaiduMap.AddOverlay(ooText);
        }

        public void ResetClick()
        {
            // 添加绘制元素
            AddCustomElementsDemo();
        }

        public void ClearClick()
        {
            // 清除所有图层
            mMapView.Map.Clear();
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
            base.OnDestroy();
        }
    }
}