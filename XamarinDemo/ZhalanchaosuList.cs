using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using Entity;
using Newtonsoft.Json;

namespace XamarinDemo
{
    [Activity(Label = "超速栅栏列表")]
    public class ZhalanchaosuList : Activity
    {
        private Testcase789sAdapter mAdapter;
        private bool mAnimatedDown;
        private WebClient mClient;
        private LinearLayout mContainer;
        private List<testCase789> mDataList;
        private bool mIsAnimating;
        private ListView mListView;
        private EditText mSearch;
        private Uri mUrl;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ZhalanchaosuList);
            mListView = FindViewById<ListView>(Resource.Id.listView);

            mListView.ItemClick += mListView_ItemClick;
            mSearch = FindViewById<EditText>(Resource.Id.etSearch);
            mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer);
            mSearch.Alpha = 0;
            mSearch.TextChanged += mSearch_TextChanged;
            Toast.MakeText(this, "数据加载中...", ToastLength.Long).Show();
            mClient = new WebClient();
            mUrl = new Uri("http://cloud.tescar.cn/Vehicle/GetZhalanchaosuData?isspec=1");
            mClient.DownloadDataAsync(mUrl);
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
            // Create your application here
        }

        private void mClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                try
                {
                    string json = Encoding.UTF8.GetString(e.Result);
                    var fRows = JsonConvert.DeserializeObject<FormatRows>(json);
                    mDataList = JsonConvert.DeserializeObject<List<testCase789>>(fRows.rows.ToString());
                    mAdapter = new Testcase789sAdapter(this, Resource.Layout.row_testcase3, mDataList, 8);
                    mListView.Adapter = mAdapter;
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                }
            });
        }

        private void mSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (mDataList != null)
            {
                List<testCase789> searchedData = (from data in mDataList
                    where
                        data.timename.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                        data.chaosuname.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                        data.daisuname.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
                    select data).ToList<testCase789>();

                mAdapter = new Testcase789sAdapter(this, Resource.Layout.row_testcase, searchedData, 8);
                mListView.Adapter = mAdapter;
            }
        }

        private void mListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.search:
                    //Search icon has been clicked

                    if (mIsAnimating)
                    {
                        return true;
                    }

                    if (!mAnimatedDown)
                    {
                        //Listview is up
                        var anim = new MyAnimation(mListView, mListView.Height - mSearch.Height);
                        anim.Duration = 500;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartDown;
                        anim.AnimationEnd += anim_AnimationEndDown;
                        mContainer.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
                    }
                    else
                    {
                        //Listview is down
                        var anim = new MyAnimation(mListView, mListView.Height + mSearch.Height);
                        anim.Duration = 500;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartUp;
                        anim.AnimationEnd += anim_AnimationEndUp;
                        mContainer.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();
                    }

                    mAnimatedDown = !mAnimatedDown;
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void anim_AnimationEndUp(object sender, Animation.AnimationEndEventArgs e)
        {
            mIsAnimating = false;
            mSearch.ClearFocus();
            var inputManager = (InputMethodManager) GetSystemService(InputMethodService);
            inputManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
        }

        private void anim_AnimationEndDown(object sender, Animation.AnimationEndEventArgs e)
        {
            mIsAnimating = false;
        }

        private void anim_AnimationStartDown(object sender, Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(1.0f).SetDuration(500).Start();
        }

        private void anim_AnimationStartUp(object sender, Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(-1.0f).SetDuration(300).Start();
        }
    }
}