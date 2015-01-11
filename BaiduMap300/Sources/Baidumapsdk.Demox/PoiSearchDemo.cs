using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Overlayutil;
using Com.Baidu.Mapapi.Search.Core;
using Com.Baidu.Mapapi.Search.Poi;
using Com.Baidu.Mapapi.Search.Sug;
using Java.Lang;

namespace Baidumapsdk.Demox
{
    /**
     * 演示poi搜索功能 
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_poi", ScreenOrientation = ScreenOrientation.Sensor)]
    public class PoiSearchDemo : FragmentActivity, IOnGetPoiSearchResultListener, IOnGetSuggestionResultListener
    {

        private PoiSearch mPoiSearch = null;
        private SuggestionSearch mSuggestionSearch = null;
        private BaiduMap mBaiduMap = null;
        /**
         * 搜索关键字输入窗口
         */
        private AutoCompleteTextView keyWorldsView = null;
        private ArrayAdapter<string> sugAdapter = null;
        private int load_Index = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_poisearch);
            // 初始化搜索模块，注册搜索事件监听
            mPoiSearch = PoiSearch.NewInstance();
            mPoiSearch.SetOnGetPoiSearchResultListener(this);
            mSuggestionSearch = SuggestionSearch.NewInstance();
            mSuggestionSearch.SetOnGetSuggestionResultListener(this);
            keyWorldsView = FindViewById<AutoCompleteTextView>(Resource.Id.searchkey);
            sugAdapter = new ArrayAdapter<string>(this,
                    Android.Resource.Layout.SimpleDropDownItem1Line);
            keyWorldsView.Adapter = sugAdapter;
            mBaiduMap = (Android.Runtime.Extensions.JavaCast<SupportMapFragment>(SupportFragmentManager
                    .FindFragmentById(Resource.Id.map))).BaiduMap;

            /**
             * 当输入关键字变化时，动态更新建议列表
             */
            keyWorldsView.AfterTextChanged += (sender, e) => { };
            keyWorldsView.BeforeTextChanged += (sender, e) => { };
            keyWorldsView.TextChanged += (sender, e) =>
            {
                ICharSequence cs = new String(e.Text.ToString());
                if (cs.Length() <= 0)
                {
                    return;
                }
                string city = (FindViewById<EditText>(Resource.Id.city)).Text;
                /**
                 * 使用建议搜索服务获取建议列表，结果在onSuggestionResult()中更新
                 */
                mSuggestionSearch
                        .RequestSuggestion((new SuggestionSearchOption())
                                .Keyword(cs.ToString()).City(city));
            };
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
            mPoiSearch.Destroy();
            mSuggestionSearch.Destroy();
            base.OnDestroy();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
        }

        /**
         * 影响搜索按钮点击事件
         * 
         * @param v
         */
        [Java.Interop.Export]
        public void SearchButtonProcess(View v)
        {
            EditText editCity = FindViewById<EditText>(Resource.Id.city);
            EditText editSearchKey = FindViewById<EditText>(Resource.Id.searchkey);
            mPoiSearch.SearchInCity((new PoiCitySearchOption())
                    .City(editCity.Text)
                    .Keyword(editSearchKey.Text)
                    .PageNum(load_Index));
        }

        [Java.Interop.Export]
        public void GoToNextPage(View v)
        {
            load_Index++;
            SearchButtonProcess(null);
        }


        public void OnGetPoiResult(PoiResult result)
        {
            if (result == null
                    || result.Error == SearchResult.ERRORNO.ResultNotFound)
            {
                return;
            }
            if (result.Error == SearchResult.ERRORNO.NoError)
            {
                mBaiduMap.Clear();
                PoiOverlay overlay = new MyPoiOverlay(this, mBaiduMap);
                mBaiduMap.SetOnMarkerClickListener(overlay);
                overlay.SetData(result);
                overlay.AddToMap();
                overlay.ZoomToSpan();
                return;
            }
            if (result.Error == SearchResult.ERRORNO.AmbiguousKeyword)
            {

                // 当输入关键字在本市没有找到，但在其他城市找到时，返回包含该关键字信息的城市列表
                string strInfo = "在";
                foreach (CityInfo cityInfo in result.SuggestCityList)
                {
                    strInfo += cityInfo.City;
                    strInfo += ",";
                }
                strInfo += "找到结果";
                Toast.MakeText(this, strInfo, ToastLength.Long)
                        .Show();
            }
        }

        public void OnGetPoiDetailResult(PoiDetailResult result)
        {
            if (result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "抱歉，未找到结果", ToastLength.Short)
                        .Show();
            }
            else
            {
                Toast.MakeText(this, "成功，查看详情页面", ToastLength.Short)
                        .Show();
            }
        }

        public void OnGetSuggestionResult(SuggestionResult res)
        {
            if (res == null || res.AllSuggestions == null)
            {
                return;
            }
            sugAdapter.Clear();
            foreach (SuggestionResult.SuggestionInfo info in res.AllSuggestions)
            {
                if (info.Key != null)
                    sugAdapter.Add(info.Key);
            }
            sugAdapter.NotifyDataSetChanged();
        }

        private class MyPoiOverlay : PoiOverlay
        {
            PoiSearchDemo poiSearchDemo;

            public MyPoiOverlay(PoiSearchDemo poiSearchDemo, BaiduMap baiduMap) :
                base(baiduMap)
            {
                this.poiSearchDemo = poiSearchDemo;
            }

            public override bool OnPoiClick(int index)
            {
                base.OnPoiClick(index);
                PoiInfo poi = PoiResult.AllPoi[index];
                if (poi.HasCaterDetails)
                {
                    poiSearchDemo.mPoiSearch.SearchPoiDetail((new PoiDetailSearchOption())
                            .PoiUid(poi.Uid));
                }
                return true;
            }
        }
    }
}