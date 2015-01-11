using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Com.Baidu.Mapapi.Overlayutil;
using Com.Baidu.Mapapi.Search.Busline;
using Com.Baidu.Mapapi.Search.Core;
using Com.Baidu.Mapapi.Search.Poi;
using Java.Lang;
using System.Collections.Generic;

namespace Baidumapsdk.Demox
{
    /**
     * 此demo用来展示如何进行公交线路详情检索，并使用RouteOverlay在地图上绘制 同时展示如何浏览路线节点并弹出泡泡
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_bus", ScreenOrientation = ScreenOrientation.Sensor)]
    public class BusLineSearchDemo : FragmentActivity,
        IOnGetPoiSearchResultListener, IOnGetBusLineSearchResultListener,
        BaiduMap.IOnMapClickListener
    {
        private Button mBtnPre = null;// 上一个节点
        private Button mBtnNext = null;// 下一个节点
        private int nodeIndex = -2;// 节点索引,供浏览节点时使用
        private BusLineResult route = null;// 保存驾车/步行路线数据的变量，供浏览节点时使用
        private IList<string> busLineIDList = null;
        private int busLineIndex = 0;
        // 搜索相关
        private PoiSearch mSearch = null; // 搜索模块，也可去掉地图模块独立使用
        private BusLineSearch mBusLineSearch = null;
        private BaiduMap mBaiduMap = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_busline);
            ICharSequence titleLable = new String("公交线路查询功能");
            Title = titleLable.ToString();
            mBtnPre = FindViewById<Button>(Resource.Id.pre);
            mBtnNext = FindViewById<Button>(Resource.Id.next);
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;
            mBaiduMap = (Android.Runtime.Extensions.JavaCast<SupportMapFragment>(SupportFragmentManager
                    .FindFragmentById(Resource.Id.bmapView))).BaiduMap;
            mBaiduMap.SetOnMapClickListener(this);
            mSearch = PoiSearch.NewInstance();
            mSearch.SetOnGetPoiSearchResultListener(this);
            mBusLineSearch = BusLineSearch.NewInstance();
            mBusLineSearch.SetOnGetBusLineSearchResultListener(this);
            busLineIDList = new List<string>();
        }

        /**
         * 发起检索
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SearchButtonProcess(View v)
        {
            busLineIDList.Clear();
            busLineIndex = 0;
            mBtnPre.Visibility = ViewStates.Invisible;
            mBtnNext.Visibility = ViewStates.Invisible;
            EditText editCity = FindViewById<EditText>(Resource.Id.city);
            EditText editSearchKey = FindViewById<EditText>(Resource.Id.searchkey);
            // 发起poi检索，从得到所有poi中找到公交线路类型的poi，再使用该poi的uid进行公交详情搜索
            mSearch.SearchInCity((new PoiCitySearchOption()).City(
                    editCity.Text.ToString()).Keyword(
                    editSearchKey.Text.ToString()));
        }

        [Java.Interop.Export]
        public void SearchNextBusline(View v)
        {
            if (busLineIndex >= busLineIDList.Count)
            {
                busLineIndex = 0;
            }
            if (busLineIndex >= 0 && busLineIndex < busLineIDList.Count
                    && busLineIDList.Count > 0)
            {
                mBusLineSearch.SearchBusLine((new BusLineSearchOption()
                        .City((FindViewById<EditText>(Resource.Id.city)).Text
                                .ToString()).Uid(busLineIDList[busLineIndex])));

                busLineIndex++;
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

            if (nodeIndex < -1 || route == null
                    || nodeIndex >= route.Stations.Count)
                return;
            View viewCache = LayoutInflater.Inflate(Resource.Layout.custom_text_view,
                    null);
            TextView popupText = viewCache.FindViewById<TextView>(Resource.Id.textcache);
            // 上一个节点
            if (mBtnPre.Equals(v) && nodeIndex > 0)
            {
                // 索引减
                nodeIndex--;
                // 移动到指定索引的坐标
                mBaiduMap.SetMapStatus(MapStatusUpdateFactory.NewLatLng(route
                        .Stations[nodeIndex].Location));
                // 弹出泡泡
                popupText.Text = route.Stations[nodeIndex].Title;
                popupText.SetBackgroundResource(Resource.Drawable.popup);
                mBaiduMap.ShowInfoWindow(new InfoWindow(popupText, route
                        .Stations[nodeIndex].Location, null));
            }
            // 下一个节点
            if (mBtnNext.Equals(v) && nodeIndex < (route.Stations.Count - 1))
            {
                // 索引加
                nodeIndex++;
                // 移动到指定索引的坐标
                mBaiduMap.SetMapStatus(MapStatusUpdateFactory.NewLatLng(route
                        .Stations[nodeIndex].Location));
                popupText.Text = route.Stations[nodeIndex].Title;
                popupText.SetBackgroundResource(Resource.Drawable.popup);
                mBaiduMap.ShowInfoWindow(new InfoWindow(popupText, route
                        .Stations[nodeIndex].Location, null));
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            mSearch.Destroy();
            mBusLineSearch.Destroy();
            base.OnDestroy();
        }


        public void OnGetBusLineResult(BusLineResult result)
        {
            if (result == null || result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "抱歉，未找到结果",
                        ToastLength.Long).Show();
                return;
            }
            mBaiduMap.Clear();
            route = result;
            nodeIndex = -1;
            BusLineOverlay overlay = new BusLineOverlay(mBaiduMap);
            mBaiduMap.SetOnMarkerClickListener(overlay);
            overlay.SetData(result);
            overlay.AddToMap();
            overlay.ZoomToSpan();
            mBtnPre.Visibility = ViewStates.Visible;
            mBtnNext.Visibility = ViewStates.Visible;
            Toast.MakeText(this, result.BusLineName,
                    ToastLength.Short).Show();
        }

        public void OnGetPoiResult(PoiResult result)
        {

            if (result == null || result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "抱歉，未找到结果",
                        ToastLength.Long).Show();
                return;
            }
            // 遍历所有poi，找到类型为公交线路的poi
            busLineIDList.Clear();
            foreach (PoiInfo poi in result.AllPoi)
            {
                if (poi.Type == PoiInfo.POITYPE.BusLine
                        || poi.Type == PoiInfo.POITYPE.SubwayLine)
                {
                    busLineIDList.Add(poi.Uid);
                }
            }
            SearchNextBusline(null);
            route = null;
        }

        public void OnGetPoiDetailResult(PoiDetailResult result)
        {

        }

        public void OnMapClick(LatLng point)
        {
            mBaiduMap.HideInfoWindow();
        }

        public bool OnMapPoiClick(MapPoi poi)
        {
            return false;
        }
    }
}