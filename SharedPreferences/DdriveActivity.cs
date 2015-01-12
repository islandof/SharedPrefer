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
    [Activity(Label = "XamarinDemo", MainLauncher = true, Icon = "@drawable/xs")]
    public class DdriveActivity : Activity
    {
        private WebClient mClient;
        private Uri mUrl;
        private List<testCase1> mTestcase1;
        //private Testcase1sAdapter mAdapter;
        private ListView mListView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetCustomView(Resource.Layout.action_bar);
            ActionBar.SetDisplayShowCustomEnabled(true);
            ActionBar.SetDisplayShowHomeEnabled(false);
            FindViewById<ImageView>(Resource.Id.imageView1).Click += (s, e) =>
            {
                Intent intent = new Intent(this, typeof(BMapApiDemoMain));
                this.StartActivity(intent);
                this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
            };
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Ddrive);

            
            mListView = FindViewById<ListView>(Resource.Id.listView);
            //mSearch = FindViewById<EditText>(Resource.Id.etSearch1);
            //mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer1);
            //LoadXamarin();
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
                var fRows = JsonConvert.DeserializeObject<FormatRows>(json);
                mTestcase1 = JsonConvert.DeserializeObject<List<testCase1>>(fRows.rows.ToString());                
                int[] rowList = { Resource.Layout.row_testcase11, Resource.Layout.row_testcase12, Resource.Layout.row_testcase13, Resource.Layout.row_testcase14, Resource.Layout.row_testcase15 };                
                Testcase1sAdapter mAdapter1 = new Testcase1sAdapter(this, rowList, mTestcase1);                
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                SlidingTabsFragment fragment = new SlidingTabsFragment(mTestcase1, mAdapter1);
                transaction.Replace(Resource.Id.sample_content_fragment, fragment);
                transaction.Commit();
                
            });
            
        } 

         public async void LoadXamarin()
         {
             //测试用
             string url = "http://cloud.tescar.cn/vehicle/GetTboxalarmintimeData?isspec=1";
             
             //创建一个请求
             var httpReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
             var httpRes = (HttpWebResponse)await httpReq.GetResponseAsync();
             if (httpRes.StatusCode == HttpStatusCode.OK)
             {
                 string json = httpRes.GetResponseStream().ToString();                 
                 var fRows = JsonConvert.DeserializeObject<FormatRows>(json);
                 mTestcase1 = JsonConvert.DeserializeObject<List<testCase1>>(fRows.rows.ToString());
                 
                 int[] rowList = { Resource.Layout.row_testcase11, Resource.Layout.row_testcase12 };
                 
                 Testcase1sAdapter mAdapter1 = new Testcase1sAdapter(this, rowList, mTestcase1);
                 
                 FragmentTransaction transaction = FragmentManager.BeginTransaction();
                 SlidingTabsFragment fragment = new SlidingTabsFragment(mTestcase1, mAdapter1);
                 transaction.Replace(Resource.Id.sample_content_fragment, fragment);
                 transaction.Commit();
             }
         }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }
       
    }
}

