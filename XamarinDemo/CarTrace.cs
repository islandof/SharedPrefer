using System;
using System.Collections.Generic;
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
    public class CarTrace : Activity
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
        private List<testCase6> mDataList;
        private InfoWindow mInfoWindow;
        private string mItem;
        private string mLocation;
        private MapView mMapView;
        private Marker mMarkerA;
        private Marker mMarkerB;
        private Marker mMarkerC;
        private Marker mMarkerD;
        private Uri mUrl;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_overlay);
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
            MapStatusUpdate msu = MapStatusUpdateFactory.NewLatLng(GEO_SHENGZHENG);
            mBaiduMap.SetMapStatus(msu);
            Toast.MakeText(this, "数据加载中...", ToastLength.Long).Show();

            mItem = Intent.GetStringExtra("qicheid");
            if (!string.IsNullOrEmpty(mItem))
            {            
                mClient = new WebClient();
                mUrl = new Uri("http://cloud.tescar.cn/Vehicle/GetGpsData?isspec=1&qicheid=" + mItem);
                mClient.DownloadDataAsync(mUrl);
                mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
            }
            //InitOverlay(mItem);
            mBaiduMap.SetOnMarkerClickListener(new IOnMarkerClickListenerImpl(this));
        }

        private void mClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                try
                {
                    string json = Encoding.UTF8.GetString(e.Result);
                    var fRows = JsonConvert.DeserializeObject<FormatRows>(json);
                    mDataList = JsonConvert.DeserializeObject<List<testCase6>>(fRows.rows.ToString());
                    InitOverlay(mDataList);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                }
            });
        }

        public void InitOverlay(List<testCase6> data)
        {
            try
            {                
                IList<LatLng> points =
                    data.Select(item => new LatLng(double.Parse(item.locationy), double.Parse(item.locationx))).ToList();
                if (points.Count > 0)
                {
                    MapStatusUpdate msu = MapStatusUpdateFactory.NewLatLng(points[0]);
                    mBaiduMap.SetMapStatus(msu);
                    MapStatusUpdate u = MapStatusUpdateFactory.ZoomTo(14.0f);
                    mBaiduMap.SetMapStatus(u);

                    OverlayOptions ooPolyline = new PolylineOptions().InvokeWidth(4)
                        .InvokeColor(Color.ParseColor("#AAFF0000").ToArgb()).InvokePoints(points);
                    mBaiduMap.AddOverlay(ooPolyline);
                    mBaiduMap.AddOverlay(
                        new MarkerOptions().InvokePosition(points[points.Count - 1])
                            .InvokeIcon(bdon)
                            .InvokeZIndex(9));

                    mBaiduMap.AddOverlay(new TextOptions().InvokeBgColor(Color.ParseColor("#AAFFFF00").ToArgb())
                        .InvokeFontSize(20)
                        .InvokeFontColor(Color.ParseColor("#FFFF00FF").ToArgb())
                        .InvokeText(data[0].chepaino).InvokeZIndex(10)
                        .InvokePosition(points[points.Count - 1]));
                }
                else
                {
                    Toast.MakeText(this, "没有找到该车的行车数据", ToastLength.Long).Show();
                }
                //string[] strlist = data.Locations.Split(';');
                //IList<LatLng> points =
                //    strlist.Select(s => new LatLng(double.Parse(s.Split(',')[1]), double.Parse(s.Split(',')[0])))
                //        .ToList();
                ////OverlayOptions ooPolyline = new PolylineOptions().InvokeWidth(10)
                ////    .InvokeColor(Color.ParseColor("#AAFF0000").ToArgb()).InvokePoints(points);
                //OverlayOptions ooPolygon = new PolygonOptions().InvokePoints(points)
                //    .InvokeStroke(new Stroke(5, Color.ParseColor("#AA00FF00").ToArgb()))
                //    .InvokeFillColor(Color.ParseColor("#AAFFFF00").ToArgb());

                //mBaiduMap.AddOverlay(ooPolygon);
                //mBaiduMap.AddOverlay(new TextOptions().InvokeBgColor(Color.ParseColor("#AAFFFF00").ToArgb())
                //        .InvokeFontSize(25)
                //        .InvokeFontColor(Color.ParseColor("#FFFF00FF").ToArgb())
                //        .InvokeText(data.areaname).InvokeZIndex(10)
                //        .InvokePosition(points[0]));
                //MapStatusUpdate msu = MapStatusUpdateFactory.NewLatLng(points[0]);
                //mBaiduMap.SetMapStatus(msu);
                //mBaiduMap.AddOverlay(ooPolyline);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
            }

            #region 注释

            // 添加折线
            //LatLng p1 = new LatLng(39.97923, 116.357428);
            //LatLng p2 = new LatLng(39.94923, 116.397428);
            //LatLng p3 = new LatLng(39.97923, 116.437428);
            //IList<LatLng> points = new List<LatLng>();
            //points.Add(p1);
            //points.Add(p2);
            //points.Add(p3);

            // Android.Graphics.Color.Argb(0xAA, 0xFF, 0x00, 0x00).ToArgb();
            // Android.Graphics.Color.ParseColor("#AAFF0000").ToArgb();


            // add marker overlay
            //foreach (testCase4 item in data)
            //{
            //    if (!(string.IsNullOrEmpty(item.currentlocationx) || string.IsNullOrEmpty(item.currentlocationy)))
            //    {
            //        Extensions.JavaCast<Marker>(
            //            mBaiduMap.AddOverlay(
            //                new MarkerOptions().InvokePosition(new LatLng(double.Parse(item.currentlocationy),
            //                    double.Parse(item.currentlocationx)))
            //                    .InvokeIcon(item.currentstatus == "0" ? bdoff : bdon)
            //                    .InvokeZIndex(9)));

            //        mBaiduMap.AddOverlay(new TextOptions().InvokeBgColor(Color.ParseColor("#AAFFFF00").ToArgb())
            //            .InvokeFontSize(20)
            //            .InvokeFontColor(Color.ParseColor("#FFFF00FF").ToArgb())
            //            .InvokeText(item.chepaino).InvokeZIndex(10)
            //            .InvokePosition(new LatLng(double.Parse(item.currentlocationy),
            //                double.Parse(item.currentlocationx))));
            //    }
            //}


            //LatLng llA = new LatLng(22.540, 114.044);
            //LatLng llB = new LatLng(22.560, 114.064);
            //LatLng llA = new LatLng(22.540, 114.044);
            //LatLng llB = new LatLng(22.560, 114.064);
            //LatLng llC = new LatLng(22.570, 114.074);
            //LatLng llD = new LatLng(22.580, 114.084);

            //OverlayOptions ooA = new MarkerOptions().InvokePosition(llA).InvokeIcon(bdA)
            //        .InvokeZIndex(9);
            //mMarkerA = Android.Runtime.Extensions.JavaCast<Marker>(mBaiduMap.AddOverlay(ooA));
            //OverlayOptions ooB = new MarkerOptions().InvokePosition(llB).InvokeIcon(bdB)
            //        .InvokeZIndex(5);
            //mMarkerB = Android.Runtime.Extensions.JavaCast<Marker>(mBaiduMap.AddOverlay(ooB));
            //OverlayOptions ooC = new MarkerOptions().InvokePosition(llC).InvokeIcon(bdC)
            //        .Perspective(false).Anchor(0.5f, 0.5f).InvokeRotate(30).InvokeZIndex(7);
            //mMarkerC = Android.Runtime.Extensions.JavaCast<Marker>(mBaiduMap.AddOverlay(ooC));
            //OverlayOptions ooD = new MarkerOptions().InvokePosition(llD).InvokeIcon(bdD)
            //        .Perspective(false).InvokeZIndex(7);
            //mMarkerD = Android.Runtime.Extensions.JavaCast<Marker>(mBaiduMap.AddOverlay(ooD));

            // add ground overlay
            //LatLng southwest = new LatLng (22.540, 114.044);
            //LatLng northeast = new LatLng (22.560, 114.064);
            //LatLngBounds bounds = new LatLngBounds.Builder ().Include (northeast)
            //        .Include (southwest).Build ();

            //OverlayOptions ooGround = new GroundOverlayOptions ()
            //        .PositionFromBounds (bounds).InvokeImage (bdGround).InvokeTransparency (0.8f);
            //mBaiduMap.AddOverlay (ooGround);

            //MapStatusUpdate u = MapStatusUpdateFactory
            //        .NewLatLng (bounds.Center);
            //mBaiduMap.SetMapStatus (u);

            #endregion
        }

        /**
         * 清除所有Overlay
         * 
         * @param view
         */

        [Export]
        public void ClearOverlay(View view)
        {
            mBaiduMap.Clear();
        }

        /**
         * 重新添加Overlay
         * 
         * @param view
         */

        [Export]
        public void ResetOverlay(View view)
        {
            ClearOverlay(null);
            //InitOverlay ();
        }

        protected override void OnPause()
        {
            // MapView的生命周期与Activity同步，当activity挂起时需调用MapView.OnPause()
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            // MapView的生命周期与Activity同步，当activity恢复时需调用MapView.OnResume()
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            // MapView的生命周期与Activity同步，当activity销毁时需调用MapView.destroy()
            mMapView.OnDestroy();
            base.OnDestroy();
            // 回收 bitmap 资源
            bdA.Recycle();
            bdB.Recycle();
            bdC.Recycle();
            bdD.Recycle();
            bd.Recycle();
            bdGround.Recycle();
        }

        private class IOnInfoWindowClickListenerImplA : Object, InfoWindow.IOnInfoWindowClickListener
        {
            private readonly IOnMarkerClickListenerImpl iOnMarkerClickListenerImpl;
            private readonly LatLng ll;
            private readonly Marker marker;

            public IOnInfoWindowClickListenerImplA(IOnMarkerClickListenerImpl iOnMarkerClickListenerImpl, LatLng ll,
                Marker marker)
            {
                this.iOnMarkerClickListenerImpl = iOnMarkerClickListenerImpl;
                this.ll = ll;
                this.marker = marker;
            }

            public void OnInfoWindowClick()
            {
                var llNew = new LatLng(ll.Latitude + 0.005,
                    ll.Longitude + 0.005);
                marker.Position = llNew;
                iOnMarkerClickListenerImpl.overlayDemo.mBaiduMap.HideInfoWindow();
            }
        }

        private class IOnInfoWindowClickListenerImplB : Object, InfoWindow.IOnInfoWindowClickListener
        {
            private readonly IOnMarkerClickListenerImpl iOnMarkerClickListenerImpl;
            private readonly Marker marker;

            public IOnInfoWindowClickListenerImplB(IOnMarkerClickListenerImpl iOnMarkerClickListenerImpl, Marker marker)
            {
                this.iOnMarkerClickListenerImpl = iOnMarkerClickListenerImpl;
                this.marker = marker;
            }

            public void OnInfoWindowClick()
            {
                marker.Icon = iOnMarkerClickListenerImpl.overlayDemo.bd;
                iOnMarkerClickListenerImpl.overlayDemo.mBaiduMap.HideInfoWindow();
            }
        }

        private class IOnInfoWindowClickListenerImplC : Object, InfoWindow.IOnInfoWindowClickListener
        {
            private readonly IOnMarkerClickListenerImpl iOnMarkerClickListenerImpl;
            private readonly Marker marker;

            public IOnInfoWindowClickListenerImplC(IOnMarkerClickListenerImpl iOnMarkerClickListenerImpl, Marker marker)
            {
                this.iOnMarkerClickListenerImpl = iOnMarkerClickListenerImpl;
                this.marker = marker;
            }

            public void OnInfoWindowClick()
            {
                marker.Remove();
                iOnMarkerClickListenerImpl.overlayDemo.mBaiduMap.HideInfoWindow();
            }
        }

        private class IOnMarkerClickListenerImpl : Object, BaiduMap.IOnMarkerClickListener
        {
            public readonly CarTrace overlayDemo;

            public IOnMarkerClickListenerImpl(CarTrace overlayDemo)
            {
                this.overlayDemo = overlayDemo;
            }

            public bool OnMarkerClick(Marker marker)
            {
                var button = new Button(overlayDemo.ApplicationContext);
                button.SetBackgroundResource(Resource.Drawable.popup);
                LatLng ll = marker.Position; // 常量
                Point p = overlayDemo.mBaiduMap.Projection.ToScreenLocation(ll);
                p.Y -= 47;
                LatLng llInfo = overlayDemo.mBaiduMap.Projection.FromScreenLocation(p);
                InfoWindow.IOnInfoWindowClickListener listener = null;
                if (marker.Equals(overlayDemo.mMarkerA) || marker.Equals(overlayDemo.mMarkerD))
                {
                    button.Text = "更改位置";
                    listener = new IOnInfoWindowClickListenerImplA(this, ll, marker);
                }
                else if (marker.Equals(overlayDemo.mMarkerB))
                {
                    button.Text = "更改图标";
                    listener = new IOnInfoWindowClickListenerImplB(this, marker);
                }
                else if (marker.Equals(overlayDemo.mMarkerC))
                {
                    button.Text = "删除";
                    listener = new IOnInfoWindowClickListenerImplC(this, marker);
                }
                overlayDemo.mInfoWindow = new InfoWindow(button, llInfo, listener);
                overlayDemo.mBaiduMap.ShowInfoWindow(overlayDemo.mInfoWindow);
                return true;
            }
        }
    }
}