using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Entity;
using Newtonsoft.Json;

namespace SharedPreferences
{
    [Activity(Label = "Sliding Tab Layout", MainLauncher = true, Icon = "@drawable/xs")]
    public class MainActivity : Activity
    {
        private WebClient mClient;
        private Uri mUrl;
        private List<testCase1> mTestcase1;
        //private Testcase1sAdapter mAdapter;
        private ListView mListView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            
            mListView = FindViewById<ListView>(Resource.Id.listView);
            //mSearch = FindViewById<EditText>(Resource.Id.etSearch1);
            //mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer1);
            mClient = new WebClient();
            mUrl = new Uri("http://cloud.tescar.cn/vehicle/GetTboxalarmintimeData?isspec=1");
            mClient.DownloadDataAsync(mUrl);
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;

        }

        void mClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                //mUserinfo = JsonConvert.DeserializeObject<List<user_info>>(json);
                //var fRows = new FormatRows { rows = new List<user_info>() };
                var fRows = JsonConvert.DeserializeObject<FormatRows>(json);
                mTestcase1 = JsonConvert.DeserializeObject<List<testCase1>>(fRows.rows.ToString());
                //Action<ImageView> action = PicSelected;
                //mAdapter = new UserinfosAdapter(this, Resource.Layout.row_userinfo, mUserinfo, action);
                //mListView.Adapter = mAdapter;
                Testcase1sAdapter mAdapter1 = new Testcase1sAdapter(this, Resource.Layout.row_testcase11, mTestcase1);
                Testcase2sAdapter mAdapter2 = new Testcase2sAdapter(this, Resource.Layout.row_testcase12, mTestcase1);
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SlidingTabsFragment fragment = new SlidingTabsFragment(mTestcase1, mAdapter1, mAdapter2);
                transaction.Replace(Resource.Id.sample_content_fragment, fragment);
                transaction.Commit();
                //mListView.Adapter = mAdapter;
            });
        } 

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }
       
    }
}

