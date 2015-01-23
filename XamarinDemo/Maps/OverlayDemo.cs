using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Java.Interop;
using Java.Lang;

namespace XamarinDemo.Maps
{
    /**
     * 演示覆盖物的用法
     */

    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
        Label = "@string/demo_name_overlay", ScreenOrientation = ScreenOrientation.Sensor)]
    public class OverlayDemo : Activity
    {
        /**
        * MapView 是地图主控件
        */

        // 初始化全局 bitmap 信息，不用时及时 Recycle
        private readonly BitmapDescriptor bd = BitmapDescriptorFactory
            .FromResource(Resource.Drawable.icon_gcoding);

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

        private BaiduMap mBaiduMap;
        private InfoWindow mInfoWindow;
        private MapView mMapView;
        private Marker mMarkerA;
        private Marker mMarkerB;
        private Marker mMarkerC;
        private Marker mMarkerD;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_overlay);
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
            MapStatusUpdate msu = MapStatusUpdateFactory.ZoomTo(14.0f);
            mBaiduMap.SetMapStatus(msu);
            InitOverlay();
            mBaiduMap.SetOnMarkerClickListener(new IOnMarkerClickListenerImpl(this));
        }


        public void InitOverlay()
        {
            // add marker overlay
            var llA = new LatLng(39.963175, 116.400244);
            var llB = new LatLng(39.942821, 116.369199);
            var llC = new LatLng(39.939723, 116.425541);
            var llD = new LatLng(39.906965, 116.401394);

            OverlayOptions ooA = new MarkerOptions().InvokePosition(llA).InvokeIcon(bdA)
                .InvokeZIndex(9);
            mMarkerA = Extensions.JavaCast<Marker>(mBaiduMap.AddOverlay(ooA));
            OverlayOptions ooB = new MarkerOptions().InvokePosition(llB).InvokeIcon(bdB)
                .InvokeZIndex(5);
            mMarkerB = Extensions.JavaCast<Marker>(mBaiduMap.AddOverlay(ooB));
            OverlayOptions ooC = new MarkerOptions().InvokePosition(llC).InvokeIcon(bdC)
                .Perspective(false).Anchor(0.5f, 0.5f).InvokeRotate(30).InvokeZIndex(7);
            mMarkerC = Extensions.JavaCast<Marker>(mBaiduMap.AddOverlay(ooC));
            OverlayOptions ooD = new MarkerOptions().InvokePosition(llD).InvokeIcon(bdD)
                .Perspective(false).InvokeZIndex(7);
            mMarkerD = Extensions.JavaCast<Marker>(mBaiduMap.AddOverlay(ooD));

            // add ground overlay
            var southwest = new LatLng(39.92235, 116.380338);
            var northeast = new LatLng(39.947246, 116.414977);
            LatLngBounds bounds = new LatLngBounds.Builder().Include(northeast)
                .Include(southwest).Build();

            OverlayOptions ooGround = new GroundOverlayOptions()
                .PositionFromBounds(bounds).InvokeImage(bdGround).InvokeTransparency(0.8f);
            mBaiduMap.AddOverlay(ooGround);

            MapStatusUpdate u = MapStatusUpdateFactory
                .NewLatLng(bounds.Center);
            mBaiduMap.SetMapStatus(u);
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
            InitOverlay();
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
            public readonly OverlayDemo overlayDemo;

            public IOnMarkerClickListenerImpl(OverlayDemo overlayDemo)
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