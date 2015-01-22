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

using Android.Views.InputMethods;

namespace XamarinDemo
{
	[Activity (Label = "超速栅栏列表")]
    public class ZhalanchaosuList : Activity
	{
		private List<testCase789> mDataList;
		private ListView mListView;
		private EditText mSearch;
		private LinearLayout mContainer;
		private bool mAnimatedDown;
		private bool mIsAnimating;
        private Testcase789sAdapter mAdapter;
		private WebClient mClient;
		private Uri mUrl;		

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ZhalanchaosuList);
			mListView = FindViewById<ListView> (Resource.Id.listView);

			mListView.ItemClick += mListView_ItemClick;
			mSearch = FindViewById<EditText> (Resource.Id.etSearch);
			mContainer = FindViewById<LinearLayout> (Resource.Id.llContainer);
			mSearch.Alpha = 0;
			mSearch.TextChanged += mSearch_TextChanged;
			Toast.MakeText (this, "数据加载中...", ToastLength.Long).Show ();
			mClient = new WebClient ();
            mUrl = new Uri("http://cloud.tescar.cn/Vehicle/GetZhalanchaosuData?isspec=1");
			mClient.DownloadDataAsync (mUrl);
			mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
			// Create your application here
		}

		void mClient_DownloadDataCompleted (object sender, DownloadDataCompletedEventArgs e)
		{
			RunOnUiThread (() => {
				try {
					string json = Encoding.UTF8.GetString (e.Result);
					var fRows = JsonConvert.DeserializeObject<FormatRows> (json);
					mDataList = JsonConvert.DeserializeObject<List<testCase789>> (fRows.rows.ToString ());
                    mAdapter = new Testcase789sAdapter(this, Resource.Layout.row_testcase3, mDataList,8);
					mListView.Adapter = mAdapter;

				} catch (Exception ex) {
					Toast.MakeText (this, ex.ToString (), ToastLength.Long).Show ();
				}

			});
		}

		void mSearch_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			if (mDataList != null) {
                List<testCase789> searchedData = (from data in mDataList
                                                  where data.timename.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || data.chaosuname.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || data.daisuname.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
                                                  select data).ToList<testCase789>();

                mAdapter = new Testcase789sAdapter(this, Resource.Layout.row_testcase, searchedData,8);
                mListView.Adapter = mAdapter;
			}
           

		}

		void mListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.actionbar, menu);
			return base.OnCreateOptionsMenu (menu);
		}


		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {

			case Resource.Id.search:
                    //Search icon has been clicked

				if (mIsAnimating) {
					return true;
				}

				if (!mAnimatedDown) {
					//Listview is up
					MyAnimation anim = new MyAnimation (mListView, mListView.Height - mSearch.Height);
					anim.Duration = 500;
					mListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartDown;
					anim.AnimationEnd += anim_AnimationEndDown;
					mContainer.Animate ().TranslationYBy (mSearch.Height).SetDuration (500).Start ();
				} else {
					//Listview is down
					MyAnimation anim = new MyAnimation (mListView, mListView.Height + mSearch.Height);
					anim.Duration = 500;
					mListView.StartAnimation (anim);
					anim.AnimationStart += anim_AnimationStartUp;
					anim.AnimationEnd += anim_AnimationEndUp;
					mContainer.Animate ().TranslationYBy (-mSearch.Height).SetDuration (500).Start ();
				}

				mAnimatedDown = !mAnimatedDown;
				return true;

			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		void anim_AnimationEndUp (object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
			mSearch.ClearFocus ();
			InputMethodManager inputManager = (InputMethodManager)this.GetSystemService (Context.InputMethodService);
			inputManager.HideSoftInputFromWindow (this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
		}

		void anim_AnimationEndDown (object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
		{
			mIsAnimating = false;
		}

		void anim_AnimationStartDown (object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate ().AlphaBy (1.0f).SetDuration (500).Start ();
		}

		void anim_AnimationStartUp (object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
		{
			mIsAnimating = true;
			mSearch.Animate ().AlphaBy (-1.0f).SetDuration (300).Start ();
		}
	}
}