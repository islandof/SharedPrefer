using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Com.Baidu.Mapapi.Overlayutil;
using Com.Baidu.Mapapi.Search.Core;
using Com.Baidu.Mapapi.Search.Geocode;
using Com.Baidu.Mapapi.Search.Poi;
using Com.Baidu.Mapapi.Search.Share;
using Java.Lang;

namespace Baidumapsdk.Demox
{
    /**
     * 演示poi搜索功能 
     */
    [Activity(Label = "@string/demo_name_share")]
    public class ShareDemoActivity : Activity, IOnGetPoiSearchResultListener, IOnGetShareUrlResultListener,
        IOnGetGeoCoderResultListener, BaiduMap.IOnMarkerClickListener
    {
        private MapView mMapView = null;
        private PoiSearch mPoiSearch = null; // 搜索模块，也可去掉地图模块独立使用
        private ShareUrlSearch mShareUrlSearch = null;
        private GeoCoder mGeoCoder = null;
        // 保存搜索结果地址
        private string currentAddr = null;
        // 搜索城市
        private string mCity = "北京";
        // 搜索关键字
        private string searchKey = "餐馆";
        // 反地理编译点坐标
        private LatLng mPoint = new LatLng(40.056878, 116.308141);
        private BaiduMap mBaiduMap = null;
        private Marker mAddrMarker = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_share_demo_activity);
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
            mPoiSearch = PoiSearch.NewInstance();
            mPoiSearch.SetOnGetPoiSearchResultListener(this);
            mShareUrlSearch = ShareUrlSearch.NewInstance();
            mShareUrlSearch.SetOnGetShareUrlResultListener(this);
            mGeoCoder = GeoCoder.NewInstance();
            mGeoCoder.SetOnGetGeoCodeResultListener(this);
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
            mPoiSearch.Destroy();
            mShareUrlSearch.Destroy();
            base.OnDestroy();
        }

        [Java.Interop.Export]
        public void SharePoi(View view)
        {
            // 发起poi搜索
            mPoiSearch.SearchInCity((new PoiCitySearchOption()).City(mCity)
                    .Keyword(searchKey));
            Toast.MakeText(this,
                    "在" + mCity + "搜索 " + searchKey,
                    ToastLength.Short).Show();
        }

        [Java.Interop.Export]
        public void ShareAddr(View view)
        {
            // 发起反地理编码请求
            mGeoCoder.ReverseGeoCode(new ReverseGeoCodeOption().Location(mPoint));
            Toast.MakeText(
                   this,
                   String.Format("搜索位置： %f，%f", (mPoint.Latitude), (mPoint.Longitude)),
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
            mBaiduMap.Clear();
            PoiShareOverlay poiOverlay = new PoiShareOverlay(this, mBaiduMap);
            mBaiduMap.SetOnMarkerClickListener(poiOverlay);
            poiOverlay.SetData(result);
            poiOverlay.AddToMap();
            poiOverlay.ZoomToSpan();
        }

        public void OnGetPoiDetailResult(PoiDetailResult result)
        {

        }

        public void OnGetPoiDetailShareUrlResult(ShareUrlResult result)
        {

            // 分享短串结果
            Intent it = new Intent(Intent.ActionSend);
            it.PutExtra(Intent.ExtraText, "您的朋友通过百度地图SDK与您分享一个位置: " + currentAddr
                    + " -- " + result.Url);
            it.SetType("text/plain");
            StartActivity(Intent.CreateChooser(it, "将短串分享到"));

        }

        public void OnGetLocationShareUrlResult(ShareUrlResult result)
        {

            // 分享短串结果
            Intent it = new Intent(Intent.ActionSend);
            it.PutExtra(Intent.ExtraText, "您的朋友通过百度地图SDK与您分享一个位置: " + currentAddr
                    + " -- " + result.Url);
            it.SetType("text/plain");
            StartActivity(Intent.CreateChooser(it, "将短串分享到"));

        }

        public void OnGetGeoCodeResult(GeoCodeResult result)
        {

        }

        public void OnGetReverseGeoCodeResult(ReverseGeoCodeResult result)
        {
            if (result == null || result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "抱歉，未找到结果",
                        ToastLength.Long).Show();
                return;
            }
            mBaiduMap.Clear();
            mBaiduMap.SetOnMarkerClickListener(this);
            mAddrMarker = Android.Runtime.Extensions.JavaCast<Marker>(mBaiduMap.AddOverlay(new MarkerOptions()
                    .InvokeIcon(BitmapDescriptorFactory
                            .FromResource(Resource.Drawable.icon_marka))
                    .InvokeTitle(result.Address).InvokePosition(result.Location)));

        }

        public bool OnMarkerClick(Marker marker)
        {
            if (marker == mAddrMarker)
            {
                mShareUrlSearch
                        .RequestLocationShareUrl(new LocationShareURLOption()
                                .Location(marker.Position).Snippet("测试分享点")
                                .Name(marker.Title));
            }
            return true;
        }

        /**
         * 使用PoiOverlay 展示poi点，在poi被点击时发起短串请求.
         */
        private class PoiShareOverlay : PoiOverlay
        {
            ShareDemoActivity shareDemoActivity;

            public PoiShareOverlay(ShareDemoActivity shareDemoActivity, BaiduMap baiduMap)
                : base(baiduMap)
            {
                this.shareDemoActivity = shareDemoActivity;
            }

            public override bool OnPoiClick(int i)
            {
                PoiInfo info = PoiResult.AllPoi[i];
                shareDemoActivity.currentAddr = info.Address;
                shareDemoActivity.mShareUrlSearch
                        .RequestPoiDetailShareUrl(new PoiDetailShareURLOption()
                                .PoiUid(info.Uid));
                return true;
            }
        }
    }
}