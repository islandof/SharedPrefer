using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Java.Lang;
using System.IO;

namespace Baidumapsdk.Demox
{
    /**
     * 演示地图缩放，旋转，视角控制
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_control", ScreenOrientation = ScreenOrientation.Sensor)]
    public class MapControlDemo : Activity
    {
        /**
	     * MapView 是地图主控件
	     */
        private MapView mMapView;
        private BaiduMap mBaiduMap;

        /**
         * 当前地点击点
         */
        private LatLng currentPt;
        /**
         * 控制按钮
         */
        private Button zoomButton;
        private Button rotateButton;
        private Button overlookButton;
        private Button saveScreenButton;

        private string touchType;

        /**
         * 用于显示地图状态的面板
         */
        private TextView mStateBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_mapcontrol);
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
            mStateBar = FindViewById<TextView>(Resource.Id.state);
            InitListener();
        }

        class IOnMapClickListenerImpl : Java.Lang.Object, BaiduMap.IOnMapClickListener
        {
            MapControlDemo mapControlDemo;

            public IOnMapClickListenerImpl(MapControlDemo mapControlDemo)
            {
                this.mapControlDemo = mapControlDemo;
            }

            public void OnMapClick(LatLng point)
            {
                mapControlDemo.touchType = "单击";
                mapControlDemo.currentPt = point;
                mapControlDemo.UpdateMapState();
            }

            public bool OnMapPoiClick(MapPoi poi)
            {
                return false;
            }
        }

        class IOnMapLongClickListenerImpl : Java.Lang.Object, BaiduMap.IOnMapLongClickListener
        {
            MapControlDemo mapControlDemo;

            public IOnMapLongClickListenerImpl(MapControlDemo mapControlDemo)
            {
                this.mapControlDemo = mapControlDemo;
            }

            public void OnMapLongClick(LatLng point)
            {
                mapControlDemo.touchType = "长按";
                mapControlDemo.currentPt = point;
                mapControlDemo.UpdateMapState();
            }
        }

        class IOnMapDoubleClickListenerImpl : Java.Lang.Object, BaiduMap.IOnMapDoubleClickListener
        {
            MapControlDemo mapControlDemo;

            public IOnMapDoubleClickListenerImpl(MapControlDemo mapControlDemo)
            {
                this.mapControlDemo = mapControlDemo;
            }

            public void OnMapDoubleClick(LatLng point)
            {
                mapControlDemo.touchType = "双击";
                mapControlDemo.currentPt = point;
                mapControlDemo.UpdateMapState();
            }
        }

        class IOnMapStatusChangeListenerImpl : Java.Lang.Object, BaiduMap.IOnMapStatusChangeListener
        {
            MapControlDemo mapControlDemo;

            public IOnMapStatusChangeListenerImpl(MapControlDemo mapControlDemo)
            {
                this.mapControlDemo = mapControlDemo;
            }

            public void OnMapStatusChange(MapStatus status)
            {
                mapControlDemo.UpdateMapState();
            }

            public void OnMapStatusChangeFinish(MapStatus status)
            {
                mapControlDemo.UpdateMapState();
            }

            public void OnMapStatusChangeStart(MapStatus status)
            {
                mapControlDemo.UpdateMapState();
            }
        }

        class ISnapshotReadyCallbackImpl : Java.Lang.Object, BaiduMap.ISnapshotReadyCallback
        {
            IOnClickListenerImpl iOnClickListenerImpl;

            public ISnapshotReadyCallbackImpl(IOnClickListenerImpl iOnClickListenerImpl)
            {
                this.iOnClickListenerImpl = iOnClickListenerImpl;
            }

            public void OnSnapshotReady(Bitmap snapshot)
            {
                string file = "/mnt/sdcard/test.png";// Java.IO.File file = new Java.IO.File("/mnt/sdcard/test.png");
                FileStream outX; //FileOutputStream outX;
                try
                {
                    outX = new System.IO.FileStream(file, FileMode.Create); // outX = new FileOutputStream(file);
                    if (snapshot.Compress(
                            Bitmap.CompressFormat.Png, 100, outX))
                    {
                        outX.Flush();
                        outX.Close();
                    }
                    Toast.MakeText(iOnClickListenerImpl.mapControlDemo,
                            "屏幕截图成功，图片存在: " + file.ToString(),
                            ToastLength.Short).Show();
                }
                catch (FileNotFoundException e)
                {
                    throw e;
                }
                catch (IOException e)
                {
                    throw e;
                }
            }
        }

        class IOnClickListenerImpl : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            public MapControlDemo mapControlDemo;

            public IOnClickListenerImpl(MapControlDemo mapControlDemo)
            {
                this.mapControlDemo = mapControlDemo;
            }

            public void OnClick(Android.Views.View view)
            {
                if (view.Equals(mapControlDemo.zoomButton))
                {
                    mapControlDemo.PerfomZoom();
                }
                else if (view.Equals(mapControlDemo.rotateButton))
                {
                    mapControlDemo.PerfomRotate();
                }
                else if (view.Equals(mapControlDemo.overlookButton))
                {
                    mapControlDemo.PerfomOverlook();
                }
                else if (view.Equals(mapControlDemo.saveScreenButton))
                {
                    // 截图，在SnapshotReadyCallback中保存图片到 sd 卡
                    mapControlDemo.mBaiduMap.Snapshot(new ISnapshotReadyCallbackImpl(this));
                    Toast.MakeText(mapControlDemo, "正在截取屏幕图片...",
                            ToastLength.Short).Show();

                }
                mapControlDemo.UpdateMapState();
            }
        }

        private void InitListener()
        {
            mBaiduMap.SetOnMapClickListener(new IOnMapClickListenerImpl(this));
            mBaiduMap.SetOnMapLongClickListener(new IOnMapLongClickListenerImpl(this));
            mBaiduMap.SetOnMapDoubleClickListener(new IOnMapDoubleClickListenerImpl(this));
            mBaiduMap.SetOnMapStatusChangeListener(new IOnMapStatusChangeListenerImpl(this));
            zoomButton = FindViewById<Button>(Resource.Id.zoombutton);
            rotateButton = FindViewById<Button>(Resource.Id.rotatebutton);
            overlookButton = FindViewById<Button>(Resource.Id.overlookbutton);
            saveScreenButton = FindViewById<Button>(Resource.Id.savescreen);
            Android.Views.View.IOnClickListener onClickListener = new IOnClickListenerImpl(this);
            zoomButton.SetOnClickListener(onClickListener);
            rotateButton.SetOnClickListener(onClickListener);
            overlookButton.SetOnClickListener(onClickListener);
            saveScreenButton.SetOnClickListener(onClickListener);
        }



        /**
         * 处理缩放 sdk 缩放级别范围： [3.0,19.0]
         */
        private void PerfomZoom()
        {
            EditText t = FindViewById<EditText>(Resource.Id.zoomlevel);
            try
            {
                float zoomLevel = Float.ParseFloat(t.Text);
                MapStatusUpdate u = MapStatusUpdateFactory.ZoomTo(zoomLevel);
                mBaiduMap.AnimateMapStatus(u);
            }
            catch (NumberFormatException e)
            {
                Toast.MakeText(this, "请输入正确的缩放级别", ToastLength.Short).Show();
            }
        }

        /**
         * 处理旋转 旋转角范围： -180 ~ 180 , 单位：度 逆时针旋转
         */
        private void PerfomRotate()
        {
            EditText t = FindViewById<EditText>(Resource.Id.rotateangle);
            try
            {
                int rotateAngle = Integer.ParseInt(t.Text);
                MapStatus ms = new MapStatus.Builder(mBaiduMap.MapStatus).Rotate(rotateAngle).Build();
                MapStatusUpdate u = MapStatusUpdateFactory.NewMapStatus(ms);
                mBaiduMap.AnimateMapStatus(u);
            }
            catch (NumberFormatException e)
            {
                Toast.MakeText(this, "请输入正确的旋转角度", ToastLength.Short).Show();
            }
        }

        /**
         * 处理俯视 俯角范围： -45 ~ 0 , 单位： 度
         */
        private void PerfomOverlook()
        {
            EditText t = FindViewById<EditText>(Resource.Id.overlookangle);
            try
            {
                int overlookAngle = Integer.ParseInt(t.Text);
                MapStatus ms = new MapStatus.Builder(mBaiduMap.MapStatus).Overlook(overlookAngle).Build();
                MapStatusUpdate u = MapStatusUpdateFactory.NewMapStatus(ms);
                mBaiduMap.AnimateMapStatus(u);
            }
            catch (NumberFormatException e)
            {
                Toast.MakeText(this, "请输入正确的俯角", ToastLength.Short).Show();
            }
        }

        /**
         * 更新地图状态显示面板
         */
        private void UpdateMapState()
        {
            if (mStateBar == null)
            {
                return;
            }
            string state = "";
            if (currentPt == null)
            {
                state = "点击、长按、双击地图以获取经纬度和地图状态";
            }
            else
            {
                state = String.Format(touchType + ",当前经度： %f 当前纬度：%f",
                        currentPt.Longitude, currentPt.Latitude);
            }
            state += "\n";
            MapStatus ms = mBaiduMap.MapStatus;
            state += String.Format(
                    "zoom=%.1f rotate=%d overlook=%d",
                    ms.Zoom, (int)ms.Rotate, (int)ms.Overlook);
            mStateBar.Text = state;
        }

        protected override void OnPause()
        {
            // MapView的生命周期与Activity同步，当activity挂起时需调用MapView.onPause()
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            // MapView的生命周期与Activity同步，当activity恢复时需调用MapView.onResume()
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            // MapView的生命周期与Activity同步，当activity销毁时需调用MapView.destroy()
            mMapView.OnDestroy();
            base.OnDestroy();
        }
    }
}