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
     * ��ʾpoi�������� 
     */
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_poi", ScreenOrientation = ScreenOrientation.Sensor)]
    public class PoiSearchDemo : FragmentActivity, IOnGetPoiSearchResultListener, IOnGetSuggestionResultListener
    {

        private PoiSearch mPoiSearch = null;
        private SuggestionSearch mSuggestionSearch = null;
        private BaiduMap mBaiduMap = null;
        /**
         * �����ؼ������봰��
         */
        private AutoCompleteTextView keyWorldsView = null;
        private ArrayAdapter<string> sugAdapter = null;
        private int load_Index = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_poisearch);
            // ��ʼ������ģ�飬ע�������¼�����
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
             * ������ؼ��ֱ仯ʱ����̬���½����б�
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
                 * ʹ�ý������������ȡ�����б������onSuggestionResult()�и���
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
         * Ӱ��������ť����¼�
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

                // ������ؼ����ڱ���û���ҵ����������������ҵ�ʱ�����ذ����ùؼ�����Ϣ�ĳ����б�
                string strInfo = "��";
                foreach (CityInfo cityInfo in result.SuggestCityList)
                {
                    strInfo += cityInfo.City;
                    strInfo += ",";
                }
                strInfo += "�ҵ����";
                Toast.MakeText(this, strInfo, ToastLength.Long)
                        .Show();
            }
        }

        public void OnGetPoiDetailResult(PoiDetailResult result)
        {
            if (result.Error != SearchResult.ERRORNO.NoError)
            {
                Toast.MakeText(this, "��Ǹ��δ�ҵ����", ToastLength.Short)
                        .Show();
            }
            else
            {
                Toast.MakeText(this, "�ɹ����鿴����ҳ��", ToastLength.Short)
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