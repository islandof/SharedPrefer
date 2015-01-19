using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entity;
using Newtonsoft.Json;

namespace XamarinDemo
{
    [Activity(Label = "SijiList" ,MainLauncher = true)]
    public class SijiList : Activity
    {
        private List<testCase3> mTestCase3s;
		private ListView mListView;
		private EditText mSearch;
		private LinearLayout mContainer;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		private TestCase3Adapter mAdapter;
		private WebClient mClient;
        private Uri mUrl;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TestCase);
            mListView = FindViewById<ListView>(Resource.Id.listView);
            mSearch = FindViewById<EditText>(Resource.Id.etSearch);
            mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer);
            mSearch.Alpha = 0;
            //mSearch.TextChanged += mSearch_TextChanged;
            Toast.MakeText(this, "数据加载中...", ToastLength.Long).Show();
            mClient = new WebClient();
            mUrl = new Uri("http://cloud.tescar.cn/Vehicle/GetSijiData?isspec=1");
            mClient.DownloadDataAsync(mUrl);
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
		 
            // Create your application here
        }

        void mClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                try
                {
                    string json = Encoding.UTF8.GetString(e.Result);
                    //mUserinfo = JsonConvert.DeserializeObject<List<user_info>>(json);
                    //var fRows = new FormatRows { rows = new List<user_info>() };
                    var fRows = JsonConvert.DeserializeObject<FormatRows>(json);
                    mTestCase3s = JsonConvert.DeserializeObject<List<testCase3>>(fRows.rows.ToString());
                    //Action<ImageView> action = PicSelected;
                    //mAdapter = new UserinfosAdapter(this, Resource.Layout.row_userinfo, mUserinfo, action);
                    //mListView.Adapter = mAdapter;
                    mAdapter = new TestCase3Adapter(this, Resource.Layout.row_contact,mTestCase3s);
                    mListView.Adapter = mAdapter;
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                }

            });
        }

    }
}