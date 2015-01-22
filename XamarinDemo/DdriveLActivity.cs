using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Entity;
using Newtonsoft.Json;

namespace XamarinDemo
{
	[Activity (Label = "XamarinDemo", ScreenOrientation = ScreenOrientation.Sensor)]
	public class DdriveLActivity : Activity
	{
		private WebClient mClient;
		private Uri mUrl;
		private List<testCase1> mTestcase1;
		private ProgressBar mProgressBar;
		//private Testcase1sAdapter mAdapter;
		private ListView mListView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Ddrive);
            
			mListView = FindViewById<ListView> (Resource.Id.listView);
			//mProgressBar = FindViewById<ProgressBar> (Resource.Id.progressBar1);
			//mSearch = FindViewById<EditText>(Resource.Id.etSearch1);
			//mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer1);
			
			Toast.MakeText (this, "数据加载中...", ToastLength.Long).Show ();
			mClient = new WebClient ();
			mUrl = new Uri ("http://cloud.tescar.cn/vehicle/GetTboxalarmintimeData?isspec=1");
			mClient.DownloadDataAsync (mUrl);
			mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
		}


		void mClient_DownloadDataCompleted (object sender, DownloadDataCompletedEventArgs e)
		{
			RunOnUiThread (() => {
				try {								
					string json = Encoding.UTF8.GetString (e.Result);                
					var fRows = JsonConvert.DeserializeObject<FormatRows> (json);
					mTestcase1 = JsonConvert.DeserializeObject<List<testCase1>> (fRows.rows.ToString ());                
					Testcase1sAdapter mAdapter1 = new Testcase1sAdapter (this, Resource.Layout.row_testcase1, mTestcase1);
					FragmentTransaction transaction = FragmentManager.BeginTransaction ();
					SlidingTabsFragment fragment = new SlidingTabsFragment (mTestcase1, mAdapter1);
					transaction.Replace (Resource.Id.sample_content_fragment, fragment);
					transaction.Commit ();
				} catch (Exception ex) {
					Toast.MakeText (this, ex.ToString (), ToastLength.Long).Show ();
				}
			});
            
		}

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }
       
	}
}

