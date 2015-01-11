using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using Com.Baidu.Mapapi.Cloud;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;

namespace Baidumapsdk.Demox
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_basemap", ScreenOrientation = ScreenOrientation.Sensor)]
    public class CloudSearchActivity : Activity, ICloudListener
    {
        private static readonly string LTAG = typeof(CloudSearchActivity).Name;// new CloudSearchActivity().Class.SimpleName
        private MapView mMapView;
        private BaiduMap mBaiduMap;

        protected override void OnCreate(Bundle icicle)
        {
            base.OnCreate(icicle);
            SetContentView(Resource.Layout.activity_lbssearch);
            CloudManager.Instance.Init(this);
            mMapView = FindViewById<MapView>(Resource.Id.bmapView);
            mBaiduMap = mMapView.Map;
            FindViewById(Resource.Id.regionSearch).Click += delegate
            {
                LocalSearchInfo info = new LocalSearchInfo();
                info.Ak = "B266f735e43ab207ec152deff44fec8b";
                info.GeoTableId = 31869;
                info.Tags = "";
                info.Q = "天安门";
                info.Region = "北京市";
                CloudManager.Instance.LocalSearch(info);

            };
            FindViewById(Resource.Id.regionSearch).Click += delegate
            {
                LocalSearchInfo info = new LocalSearchInfo();
                info.Ak = "B266f735e43ab207ec152deff44fec8b";
                info.GeoTableId = 31869;
                info.Tags = "";
                info.Q = "天安门";
                info.Region = "北京市";
                CloudManager.Instance.LocalSearch(info);
            };
            FindViewById(Resource.Id.nearbySearch).Click += delegate
            {
                NearbySearchInfo info = new NearbySearchInfo();
                info.Ak = "D9ace96891048231e8777291cda45ca0";
                info.GeoTableId = 32038;
                info.Radius = 30000;
                info.Location = "116.403689,39.914957";
                CloudManager.Instance.NearbySearch(info);
            };

            FindViewById(Resource.Id.boundsSearch).Click += delegate
            {
                BoundSearchInfo info = new BoundSearchInfo();
                info.Ak = "B266f735e43ab207ec152deff44fec8b";
                info.GeoTableId = 31869;
                info.Q = "天安门";
                info.Bound = "116.401663,39.913961;116.406529,39.917396";
                CloudManager.Instance.BoundSearch(info);
            };
            FindViewById(Resource.Id.detailsSearch).Click += delegate
            {
                DetailSearchInfo info = new DetailSearchInfo();
                info.Ak = "B266f735e43ab207ec152deff44fec8b";
                info.GeoTableId = 31869;
                info.Uid = 18622266;
                CloudManager.Instance.DetailSearch(info);
            };
        }



        protected override void OnDestroy()
        {
            base.OnDestroy();
            mMapView.OnDestroy();
            CloudManager.Instance.Destroy();
        }

        protected override void OnPause()
        {
            base.OnPause();
            mMapView.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            mMapView.OnResume();
        }

        public void OnGetDetailSearchResult(DetailSearchResult result, int error)
        {
            if (result != null)
            {
                if (result.PoiInfo != null)
                {
                    Toast.MakeText(this, result.PoiInfo.Title,
                            ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this,
                            "status:" + result.Status, ToastLength.Short).Show();
                }
            }
        }

        public void OnGetSearchResult(CloudSearchResult result, int error)
        {
            if (result != null && result.PoiList != null
                    && result.PoiList.Count > 0)
            {
                Log.Debug(LTAG, "onGetSearchResult, result length: " + result.PoiList.Count);
                mBaiduMap.Clear();
                BitmapDescriptor bd = BitmapDescriptorFactory.FromResource(Resource.Drawable.icon_gcoding);
                LatLng ll;
                LatLngBounds.Builder builder = new LatLngBounds.Builder();
                foreach (CloudPoiInfo info in result.PoiList)
                {
                    ll = new LatLng(info.Latitude, info.Longitude);
                    OverlayOptions oo = new MarkerOptions().InvokeIcon(bd).InvokePosition(ll);
                    mBaiduMap.AddOverlay(oo);
                    builder.Include(ll);
                }
                LatLngBounds bounds = builder.Build();
                MapStatusUpdate u = MapStatusUpdateFactory.NewLatLngBounds(bounds);
                mBaiduMap.AnimateMapStatus(u);
            }
        }
    }
}