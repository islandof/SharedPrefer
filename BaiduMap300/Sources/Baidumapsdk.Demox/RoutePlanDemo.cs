using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Com.Baidu.Mapapi.Overlayutil;
using Com.Baidu.Mapapi.Search.Core;
using Com.Baidu.Mapapi.Search.Route;
using Java.Lang;
using System.Reflection;

namespace Baidumapsdk.Demox
{
    /**
     * ��demo����չʾ��ν��мݳ������С�����·���������ڵ�ͼʹ��RouteOverlay��TransitOverlay����
     * ͬʱչʾ��ν��нڵ��������������
     *
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_route", ScreenOrientation = ScreenOrientation.Sensor)]
    public class RoutePlanDemo : Activity, BaiduMap.IOnMapClickListener,
        IOnGetRoutePlanResultListener
    {
        //���·�߽ڵ����
        Button mBtnPre = null;//��һ���ڵ�
        Button mBtnNext = null;//��һ���ڵ�
        int nodeIndex = -2;//�ڵ�����,������ڵ�ʱʹ��
        RouteLine route = null;
        OverlayManager routeOverlay = null;
        bool useDefaultIcon = false;
        private TextView popupText = null;//����view
        private View viewCache = null;

        //��ͼ��أ�ʹ�ü̳�MapView��MyRouteMapViewĿ������дtouch�¼�ʵ�����ݴ���
        //���������touch�¼���������̳У�ֱ��ʹ��MapView����
        MapView mMapView = null;    // ��ͼView
        BaiduMap mBaidumap = null;
        //�������
        RoutePlanSearch mSearch = null;    // ����ģ�飬Ҳ��ȥ����ͼģ�����ʹ��

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_routeplan);
            ICharSequence titleLable = new String("·�߹滮����");
            Title = titleLable.ToString();
            //��ʼ����ͼ
            mMapView = FindViewById<MapView>(Resource.Id.map);
            mBaidumap = mMapView.Map;
            mBtnPre = FindViewById<Button>(Resource.Id.pre);
            mBtnNext = FindViewById<Button>(Resource.Id.next);
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;
            //��ͼ����¼�����
            mBaidumap.SetOnMapClickListener(this);
            // ��ʼ������ģ�飬ע���¼�����
            mSearch = RoutePlanSearch.NewInstance();
            mSearch.SetOnGetRoutePlanResultListener(this);
        }

        /**
         * ����·�߹滮����ʾ��
         *
         * @param v
         */
        [Java.Interop.Export]
        public void SearchButtonProcess(View v)
        {
            //��������ڵ��·������
            route = null;
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;
            mBaidumap.Clear();
            // ����������ť��Ӧ
            EditText editSt = FindViewById<EditText>(Resource.Id.start);
            EditText editEn = FindViewById<EditText>(Resource.Id.end);
            //�������յ���Ϣ������tranist search ��˵��������������
            PlanNode stNode = PlanNode.WithCityNameAndPlaceName("����", editSt.Text);
            PlanNode enNode = PlanNode.WithCityNameAndPlaceName("����", editEn.Text);

            // ʵ��ʹ�����������յ���н�����ȷ���趨
            if (v.Id == Resource.Id.drive)
            {
                mSearch.DrivingSearch((new DrivingRoutePlanOption())
                        .From(stNode)
                        .To(enNode));
            }
            else if (v.Id == Resource.Id.transit)
            {
                mSearch.TransitSearch((new TransitRoutePlanOption())
                        .From(stNode)
                        .City("����")
                        .To(enNode));
            }
            else if (v.Id == Resource.Id.walk)
            {
                mSearch.WalkingSearch((new WalkingRoutePlanOption())
                        .From(stNode)
                        .To(enNode));
            }
        }

        /**
         * �ڵ����ʾ��
         *
         * @param v
         */
        [Java.Interop.Export]
        public void NodeClick(View v)
        {
            if (nodeIndex < -1 || route == null ||
                    route.AllStep == null
                    || nodeIndex > route.AllStep.Count)
            {
                return;
            }
            //���ýڵ�����
            if (v.Id == Resource.Id.next && nodeIndex < route.AllStep.Count - 1)
            {
                nodeIndex++;
            }
            else if (v.Id == Resource.Id.pre && nodeIndex > 1)
            {
                nodeIndex--;
            }
            if (nodeIndex < 0 || nodeIndex >= route.AllStep.Count)
            {
                return;
            }

            //��ȡ�ڽ����Ϣ
            LatLng nodeLocation = null;
            string nodeTitle = null;
            object step = route.AllStep[nodeIndex];

            string stepSimpleName = ((Object)step).Class.SimpleName;
            string drivingStepName = typeof(DrivingRouteLine.DrivingStep).Name;
            string walkingStepName = typeof(WalkingRouteLine.WalkingStep).Name;
            string transitStepName = typeof(TransitRouteLine.TransitStep).Name;

            if (stepSimpleName == drivingStepName)// if (step is DrivingRouteLine.DrivingStep)
            {
                nodeLocation = (Android.Runtime.Extensions.JavaCast<DrivingRouteLine.DrivingStep>((Object)step)).Entrace.Location;
                nodeTitle = (Android.Runtime.Extensions.JavaCast<DrivingRouteLine.DrivingStep>((Object)step)).Instructions;
            }

            else if (stepSimpleName == walkingStepName)// else if (step is WalkingRouteLine.WalkingStep)
            {
                nodeLocation = (Android.Runtime.Extensions.JavaCast<WalkingRouteLine.WalkingStep>((Object)step)).Entrace.Location;
                nodeTitle = (Android.Runtime.Extensions.JavaCast<WalkingRouteLine.WalkingStep>((Object)step)).Instructions;
            }

            else if (stepSimpleName == transitStepName)//  else if (step is TransitRouteLine.TransitStep)
            {
                nodeLocation = (Android.Runtime.Extensions.JavaCast<TransitRouteLine.TransitStep>((Object)step)).Entrace.Location;
                nodeTitle = (Android.Runtime.Extensions.JavaCast<TransitRouteLine.TransitStep>((Object)step)).Instructions;
            }

            if (nodeLocation == null || nodeTitle == null)
            {
                return;
            }
            //�ƶ��ڵ�������
            mBaidumap.SetMapStatus(MapStatusUpdateFactory.NewLatLng(nodeLocation));
            // Show popup
            viewCache = LayoutInflater.Inflate(Resource.Layout.custom_text_view, null);
            popupText = viewCache.FindViewById<TextView>(Resource.Id.textcache);
            popupText.SetBackgroundResource(Resource.Drawable.popup);
            popupText.Text = nodeTitle;
            mBaidumap.ShowInfoWindow(new InfoWindow(popupText, nodeLocation, null));

        }

        /**
         * �л�·��ͼ�꣬ˢ�µ�ͼʹ����Ч
         * ע�⣺ ���յ�ͼ��ʹ�����Ķ���.
         */
        [Java.Interop.Export]
        public void ChangeRouteIcon(View v)
        {
            if (routeOverlay == null)
            {
                return;
            }
            if (useDefaultIcon)
            {
                ((Button)v).Text = "�Զ������յ�ͼ��";
                Toast.MakeText(this,
                        "��ʹ��ϵͳ���յ�ͼ��",
                        ToastLength.Short).Show();

            }
            else
            {
                ((Button)v).Text = "ϵͳ���յ�ͼ��";
                Toast.MakeText(this,
                        "��ʹ���Զ������յ�ͼ��",
                        ToastLength.Short).Show();

            }
            useDefaultIcon = !useDefaultIcon;
            routeOverlay.RemoveFromMap();
            routeOverlay.AddToMap();
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
        }

        public void OnGetWalkingRouteResult(WalkingRouteResult result)
        {
            if (result == null || result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "��Ǹ��δ�ҵ����", ToastLength.Short).Show();
            }
            if (result.Error == SearchResult.ERRORNO.AmbiguousRoureAddr)
            {
                //���յ��;�����ַ����壬ͨ�����½ӿڻ�ȡ�����ѯ��Ϣ
                //result.getSuggestAddrInfo()
                return;
            }
            if (result.Error == SearchResult.ERRORNO.NoError)
            {
                nodeIndex = -1;
                mBtnPre.Visibility = ViewStates.Visible;
                mBtnNext.Visibility = ViewStates.Visible;
                route = result.RouteLines[0];
                WalkingRouteOverlay overlay = new MyWalkingRouteOverlay(this, mBaidumap);
                mBaidumap.SetOnMarkerClickListener(overlay);
                routeOverlay = overlay;
                overlay.SetData(result.RouteLines[0]);
                overlay.AddToMap();
                overlay.ZoomToSpan();
            }

        }

        public void OnGetTransitRouteResult(TransitRouteResult result)
        {
            if (result == null || result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "��Ǹ��δ�ҵ����", ToastLength.Short).Show();
            }
            if (result.Error == SearchResult.ERRORNO.AmbiguousRoureAddr)
            {
                //���յ��;�����ַ����壬ͨ�����½ӿڻ�ȡ�����ѯ��Ϣ
                //result.getSuggestAddrInfo()
                return;
            }
            if (result.Error == SearchResult.ERRORNO.NoError)
            {
                nodeIndex = -1;
                mBtnPre.Visibility = ViewStates.Visible;
                mBtnNext.Visibility = ViewStates.Visible;
                route = result.RouteLines[0];
                TransitRouteOverlay overlay = new MyTransitRouteOverlay(this, mBaidumap);
                mBaidumap.SetOnMarkerClickListener(overlay);
                routeOverlay = overlay;
                overlay.SetData(result.RouteLines[0]);
                overlay.AddToMap();
                overlay.ZoomToSpan();
            }
        }

        public void OnGetDrivingRouteResult(DrivingRouteResult result)
        {
            if (result == null || result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "��Ǹ��δ�ҵ����", ToastLength.Short).Show();
            }
            if (result.Error == SearchResult.ERRORNO.AmbiguousRoureAddr)
            {
                //���յ��;�����ַ����壬ͨ�����½ӿڻ�ȡ�����ѯ��Ϣ
                //result.getSuggestAddrInfo()
                return;
            }
            if (result.Error == SearchResult.ERRORNO.NoError)
            {
                nodeIndex = -1;
                mBtnPre.Visibility = ViewStates.Visible;
                mBtnNext.Visibility = ViewStates.Visible;
                route = result.RouteLines[0];
                DrivingRouteOvelray overlay = new MyDrivingRouteOverlay(this, mBaidumap);
                routeOverlay = overlay;
                mBaidumap.SetOnMarkerClickListener(overlay);
                overlay.SetData(result.RouteLines[0]);
                overlay.AddToMap();
                overlay.ZoomToSpan();
            }
        }

        //����RouteOverly
        private class MyDrivingRouteOverlay : DrivingRouteOvelray
        {
            RoutePlanDemo routePlanDemo;

            public MyDrivingRouteOverlay(RoutePlanDemo routePlanDemo, BaiduMap baiduMap) :
                base(baiduMap)
            {
                this.routePlanDemo = routePlanDemo;
            }

            public BitmapDescriptor GetStartMarker()
            {
                if (routePlanDemo.useDefaultIcon)
                {
                    return BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_st);
                }
                return null;
            }

            public BitmapDescriptor GetTerminalMarker()
            {
                if (routePlanDemo.useDefaultIcon)
                {
                    return BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_en);
                }
                return null;
            }
        }

        private class MyWalkingRouteOverlay : WalkingRouteOverlay
        {
            RoutePlanDemo routePlanDemo;
            public MyWalkingRouteOverlay(RoutePlanDemo routePlanDemo, BaiduMap baiduMap) :
                base(baiduMap)
            {
                this.routePlanDemo = routePlanDemo;
            }

            public BitmapDescriptor GetStartMarker()
            {
                if (routePlanDemo.useDefaultIcon)
                {
                    return BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_st);
                }
                return null;
            }

            public BitmapDescriptor GetTerminalMarker()
            {
                if (routePlanDemo.useDefaultIcon)
                {
                    return BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_en);
                }
                return null;
            }
        }

        private class MyTransitRouteOverlay : TransitRouteOverlay
        {
            RoutePlanDemo routePlanDemo;

            public MyTransitRouteOverlay(RoutePlanDemo routePlanDemo, BaiduMap baiduMap) :
                base(baiduMap)
            {
                this.routePlanDemo = routePlanDemo;
            }

            public BitmapDescriptor GetStartMarker()
            {
                if (routePlanDemo.useDefaultIcon)
                {
                    return BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_st);
                }
                return null;
            }

            public BitmapDescriptor GetTerminalMarker()
            {
                if (routePlanDemo.useDefaultIcon)
                {
                    return BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_en);
                }
                return null;
            }
        }

        public void OnMapClick(LatLng point)
        {
            mBaidumap.HideInfoWindow();
        }

        public bool OnMapPoiClick(MapPoi poi)
        {
            return false;
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
            mSearch.Destroy();
            mMapView.OnDestroy();
            base.OnDestroy();
        }

    }
}