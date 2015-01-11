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
     * ��demo����չʾ��ν��й�����·�����������ʹ��RouteOverlay�ڵ�ͼ�ϻ��� ͬʱչʾ������·�߽ڵ㲢��������
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_bus", ScreenOrientation = ScreenOrientation.Sensor)]
    public class BusLineSearchDemo : FragmentActivity,
        IOnGetPoiSearchResultListener, IOnGetBusLineSearchResultListener,
        BaiduMap.IOnMapClickListener
    {
        private Button mBtnPre = null;// ��һ���ڵ�
        private Button mBtnNext = null;// ��һ���ڵ�
        private int nodeIndex = -2;// �ڵ�����,������ڵ�ʱʹ��
        private BusLineResult route = null;// ����ݳ�/����·�����ݵı�����������ڵ�ʱʹ��
        private IList<string> busLineIDList = null;
        private int busLineIndex = 0;
        // �������
        private PoiSearch mSearch = null; // ����ģ�飬Ҳ��ȥ����ͼģ�����ʹ��
        private BusLineSearch mBusLineSearch = null;
        private BaiduMap mBaiduMap = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_busline);
            ICharSequence titleLable = new String("������·��ѯ����");
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
         * �������
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
            // ����poi�������ӵõ�����poi���ҵ�������·���͵�poi����ʹ�ø�poi��uid���й�����������
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
         * �ڵ����ʾ��
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
            // ��һ���ڵ�
            if (mBtnPre.Equals(v) && nodeIndex > 0)
            {
                // ������
                nodeIndex--;
                // �ƶ���ָ������������
                mBaiduMap.SetMapStatus(MapStatusUpdateFactory.NewLatLng(route
                        .Stations[nodeIndex].Location));
                // ��������
                popupText.Text = route.Stations[nodeIndex].Title;
                popupText.SetBackgroundResource(Resource.Drawable.popup);
                mBaiduMap.ShowInfoWindow(new InfoWindow(popupText, route
                        .Stations[nodeIndex].Location, null));
            }
            // ��һ���ڵ�
            if (mBtnNext.Equals(v) && nodeIndex < (route.Stations.Count - 1))
            {
                // ������
                nodeIndex++;
                // �ƶ���ָ������������
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
                Toast.MakeText(this, "��Ǹ��δ�ҵ����",
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
                Toast.MakeText(this, "��Ǹ��δ�ҵ����",
                        ToastLength.Long).Show();
                return;
            }
            // ��������poi���ҵ�����Ϊ������·��poi
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