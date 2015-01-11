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
     * ��ʾpoi�������� 
     */
    [Activity(Label = "@string/demo_name_share")]
    public class ShareDemoActivity : Activity, IOnGetPoiSearchResultListener, IOnGetShareUrlResultListener,
        IOnGetGeoCoderResultListener, BaiduMap.IOnMarkerClickListener
    {
        private MapView mMapView = null;
        private PoiSearch mPoiSearch = null; // ����ģ�飬Ҳ��ȥ����ͼģ�����ʹ��
        private ShareUrlSearch mShareUrlSearch = null;
        private GeoCoder mGeoCoder = null;
        // �������������ַ
        private string currentAddr = null;
        // ��������
        private string mCity = "����";
        // �����ؼ���
        private string searchKey = "�͹�";
        // ��������������
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
            // ����poi����
            mPoiSearch.SearchInCity((new PoiCitySearchOption()).City(mCity)
                    .Keyword(searchKey));
            Toast.MakeText(this,
                    "��" + mCity + "���� " + searchKey,
                    ToastLength.Short).Show();
        }

        [Java.Interop.Export]
        public void ShareAddr(View view)
        {
            // ���𷴵����������
            mGeoCoder.ReverseGeoCode(new ReverseGeoCodeOption().Location(mPoint));
            Toast.MakeText(
                   this,
                   String.Format("����λ�ã� %f��%f", (mPoint.Latitude), (mPoint.Longitude)),
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

            // ����̴����
            Intent it = new Intent(Intent.ActionSend);
            it.PutExtra(Intent.ExtraText, "��������ͨ���ٶȵ�ͼSDK��������һ��λ��: " + currentAddr
                    + " -- " + result.Url);
            it.SetType("text/plain");
            StartActivity(Intent.CreateChooser(it, "���̴�����"));

        }

        public void OnGetLocationShareUrlResult(ShareUrlResult result)
        {

            // ����̴����
            Intent it = new Intent(Intent.ActionSend);
            it.PutExtra(Intent.ExtraText, "��������ͨ���ٶȵ�ͼSDK��������һ��λ��: " + currentAddr
                    + " -- " + result.Url);
            it.SetType("text/plain");
            StartActivity(Intent.CreateChooser(it, "���̴�����"));

        }

        public void OnGetGeoCodeResult(GeoCodeResult result)
        {

        }

        public void OnGetReverseGeoCodeResult(ReverseGeoCodeResult result)
        {
            if (result == null || result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "��Ǹ��δ�ҵ����",
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
                                .Location(marker.Position).Snippet("���Է����")
                                .Name(marker.Title));
            }
            return true;
        }

        /**
         * ʹ��PoiOverlay չʾpoi�㣬��poi�����ʱ����̴�����.
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