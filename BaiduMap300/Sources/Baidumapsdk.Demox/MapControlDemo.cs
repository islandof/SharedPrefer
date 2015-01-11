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
     * ��ʾ��ͼ���ţ���ת���ӽǿ���
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_control", ScreenOrientation = ScreenOrientation.Sensor)]
    public class MapControlDemo : Activity
    {
        /**
	     * MapView �ǵ�ͼ���ؼ�
	     */
        private MapView mMapView;
        private BaiduMap mBaiduMap;

        /**
         * ��ǰ�ص����
         */
        private LatLng currentPt;
        /**
         * ���ư�ť
         */
        private Button zoomButton;
        private Button rotateButton;
        private Button overlookButton;
        private Button saveScreenButton;

        private string touchType;

        /**
         * ������ʾ��ͼ״̬�����
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
                mapControlDemo.touchType = "����";
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
                mapControlDemo.touchType = "����";
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
                mapControlDemo.touchType = "˫��";
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
                            "��Ļ��ͼ�ɹ���ͼƬ����: " + file.ToString(),
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
                    // ��ͼ����SnapshotReadyCallback�б���ͼƬ�� sd ��
                    mapControlDemo.mBaiduMap.Snapshot(new ISnapshotReadyCallbackImpl(this));
                    Toast.MakeText(mapControlDemo, "���ڽ�ȡ��ĻͼƬ...",
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
         * �������� sdk ���ż���Χ�� [3.0,19.0]
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
                Toast.MakeText(this, "��������ȷ�����ż���", ToastLength.Short).Show();
            }
        }

        /**
         * ������ת ��ת�Ƿ�Χ�� -180 ~ 180 , ��λ���� ��ʱ����ת
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
                Toast.MakeText(this, "��������ȷ����ת�Ƕ�", ToastLength.Short).Show();
            }
        }

        /**
         * ������ ���Ƿ�Χ�� -45 ~ 0 , ��λ�� ��
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
                Toast.MakeText(this, "��������ȷ�ĸ���", ToastLength.Short).Show();
            }
        }

        /**
         * ���µ�ͼ״̬��ʾ���
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
                state = "�����������˫����ͼ�Ի�ȡ��γ�Ⱥ͵�ͼ״̬";
            }
            else
            {
                state = String.Format(touchType + ",��ǰ���ȣ� %f ��ǰγ�ȣ�%f",
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
            // MapView������������Activityͬ������activity����ʱ�����MapView.onPause()
            mMapView.OnPause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            // MapView������������Activityͬ������activity�ָ�ʱ�����MapView.onResume()
            mMapView.OnResume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            // MapView������������Activityͬ������activity����ʱ�����MapView.destroy()
            mMapView.OnDestroy();
            base.OnDestroy();
        }
    }
}