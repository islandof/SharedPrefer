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
     * 此demo用来展示如何进行驾车、步行、公交路线搜索并在地图使用RouteOverlay、TransitOverlay绘制
     * 同时展示如何进行节点浏览并弹出泡泡
     *
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_route", ScreenOrientation = ScreenOrientation.Sensor)]
    public class RoutePlanDemo : Activity, BaiduMap.IOnMapClickListener,
        IOnGetRoutePlanResultListener
    {
        //浏览路线节点相关
        Button mBtnPre = null;//上一个节点
        Button mBtnNext = null;//下一个节点
        int nodeIndex = -2;//节点索引,供浏览节点时使用
        RouteLine route = null;
        OverlayManager routeOverlay = null;
        bool useDefaultIcon = false;
        private TextView popupText = null;//泡泡view
        private View viewCache = null;

        //地图相关，使用继承MapView的MyRouteMapView目的是重写touch事件实现泡泡处理
        //如果不处理touch事件，则无需继承，直接使用MapView即可
        MapView mMapView = null;    // 地图View
        BaiduMap mBaidumap = null;
        //搜索相关
        RoutePlanSearch mSearch = null;    // 搜索模块，也可去掉地图模块独立使用

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_routeplan);
            ICharSequence titleLable = new String("路线规划功能");
            Title = titleLable.ToString();
            //初始化地图
            mMapView = FindViewById<MapView>(Resource.Id.map);
            mBaidumap = mMapView.Map;
            mBtnPre = FindViewById<Button>(Resource.Id.pre);
            mBtnNext = FindViewById<Button>(Resource.Id.next);
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;
            //地图点击事件处理
            mBaidumap.SetOnMapClickListener(this);
            // 初始化搜索模块，注册事件监听
            mSearch = RoutePlanSearch.NewInstance();
            mSearch.SetOnGetRoutePlanResultListener(this);
        }

        /**
         * 发起路线规划搜索示例
         *
         * @param v
         */
        [Java.Interop.Export]
        public void SearchButtonProcess(View v)
        {
            //重置浏览节点的路线数据
            route = null;
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;
            mBaidumap.Clear();
            // 处理搜索按钮响应
            EditText editSt = FindViewById<EditText>(Resource.Id.start);
            EditText editEn = FindViewById<EditText>(Resource.Id.end);
            //设置起终点信息，对于tranist search 来说，城市名无意义
            PlanNode stNode = PlanNode.WithCityNameAndPlaceName("北京", editSt.Text);
            PlanNode enNode = PlanNode.WithCityNameAndPlaceName("北京", editEn.Text);

            // 实际使用中请对起点终点城市进行正确的设定
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
                        .City("北京")
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
         * 节点浏览示例
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
            //设置节点索引
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

            //获取节结果信息
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
            //移动节点至中心
            mBaidumap.SetMapStatus(MapStatusUpdateFactory.NewLatLng(nodeLocation));
            // Show popup
            viewCache = LayoutInflater.Inflate(Resource.Layout.custom_text_view, null);
            popupText = viewCache.FindViewById<TextView>(Resource.Id.textcache);
            popupText.SetBackgroundResource(Resource.Drawable.popup);
            popupText.Text = nodeTitle;
            mBaidumap.ShowInfoWindow(new InfoWindow(popupText, nodeLocation, null));

        }

        /**
         * 切换路线图标，刷新地图使其生效
         * 注意： 起终点图标使用中心对齐.
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
                ((Button)v).Text = "自定义起终点图标";
                Toast.MakeText(this,
                        "将使用系统起终点图标",
                        ToastLength.Short).Show();

            }
            else
            {
                ((Button)v).Text = "系统起终点图标";
                Toast.MakeText(this,
                        "将使用自定义起终点图标",
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
                Toast.MakeText(this, "抱歉，未找到结果", ToastLength.Short).Show();
            }
            if (result.Error == SearchResult.ERRORNO.AmbiguousRoureAddr)
            {
                //起终点或途经点地址有岐义，通过以下接口获取建议查询信息
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
                Toast.MakeText(this, "抱歉，未找到结果", ToastLength.Short).Show();
            }
            if (result.Error == SearchResult.ERRORNO.AmbiguousRoureAddr)
            {
                //起终点或途经点地址有岐义，通过以下接口获取建议查询信息
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
                Toast.MakeText(this, "抱歉，未找到结果", ToastLength.Short).Show();
            }
            if (result.Error == SearchResult.ERRORNO.AmbiguousRoureAddr)
            {
                //起终点或途经点地址有岐义，通过以下接口获取建议查询信息
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

        //定制RouteOverly
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