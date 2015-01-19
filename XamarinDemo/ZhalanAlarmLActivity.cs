﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Entity;
using Newtonsoft.Json;

namespace XamarinDemo
{
	[Activity (Label = "ZhalanAlarmLActivity")]			
	public class ZhalanAlarmLActivity : Activity
	{
		private List<testCase2> mZhalanAlarm;
		private ListView mListView;
		private EditText mSearch;
		private LinearLayout mContainer;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		private Testcase2sAdapter mAdapter;
		private WebClient mClient;
		private Uri mUrl;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Create your application here
			SetContentView (Resource.Layout.ZhalanAlarmL);
			mListView = FindViewById<ListView> (Resource.Id.listView);
			mSearch = FindViewById<EditText> (Resource.Id.etSearch);
			mContainer = FindViewById<LinearLayout> (Resource.Id.llContainer);
			mSearch.Alpha = 0;
			mSearch.TextChanged += mSearch_TextChanged;
			Toast.MakeText (this, "数据加载中...", ToastLength.Long).Show ();
			mClient = new WebClient ();
			mUrl = new Uri ("http://cloud.tescar.cn/vehicle/GetTboxxczhalanalarmData?isspec=1");
			//mProgressBar.Visibility = ViewStates.Visible;
			mClient.DownloadDataAsync (mUrl);
			mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
		}


		void mSearch_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
//			if (mZhalanAlarm != null) {
//				List<user_info> searchedFriends = (from userinfo in mUserinfo
//				                                   where userinfo.USER_NAME.Contains (mSearch.Text, StringComparison.OrdinalIgnoreCase) || userinfo.ROLE_NAME.Contains (mSearch.Text, StringComparison.OrdinalIgnoreCase)
//				                                       || userinfo.USER_PWD.Contains (mSearch.Text, StringComparison.OrdinalIgnoreCase) || userinfo.USER_EMAIL.Contains (mSearch.Text, StringComparison.OrdinalIgnoreCase)
//				                                   select userinfo).ToList<user_info> ();
//
//				mAdapter = new TestcasesAdapter (this, Resource.Layout.row_testcase, searchedFriends);
//				mListView.Adapter = mAdapter;
//			}				
		}

		//public override bool OnCreateOptionsMenu(IMenu menu)
		//{
		//    MenuInflater.Inflate(Resource.Menu.actionbar, menu);
		//    return base.OnCreateOptionsMenu(menu);
		//}

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

		void mClient_DownloadDataCompleted (object sender, DownloadDataCompletedEventArgs e)
		{
			RunOnUiThread (() => {
				try {

					string json = Encoding.UTF8.GetString (e.Result);
					var fRows = JsonConvert.DeserializeObject<FormatRows> (json);
					mZhalanAlarm = JsonConvert.DeserializeObject<List<testCase2>> (fRows.rows.ToString ());
					mAdapter = new Testcase2sAdapter (this, Resource.Layout.row_testcase2, mZhalanAlarm);
					mListView.Adapter = mAdapter;
				} catch (Exception ex) {				
					Toast.MakeText (this, ex.ToString (), ToastLength.Long).Show ();
				}
			});
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
