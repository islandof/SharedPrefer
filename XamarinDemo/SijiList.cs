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
	[Activity (Label = "司机列表")]
	public class SijiList : Activity
	{
		private List<testCase3> mTestCase3s;
		private ListView mListView;
		private EditText mSearch;
		private LinearLayout mContainer;
		private bool mAnimatedDown;
		private bool mIsAnimating;
		private TestCase3sAdapter mAdapter;
		private WebClient mClient;
		private Uri mUrl;
		static readonly List<string> phoneNumbers = new List<string> ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.SijiList);
			mListView = FindViewById<ListView> (Resource.Id.listView);

			mListView.ItemClick += mListView_ItemClick;
			mSearch = FindViewById<EditText> (Resource.Id.etSearch);
			mContainer = FindViewById<LinearLayout> (Resource.Id.llContainer);
			mSearch.Alpha = 0;
			mSearch.TextChanged += mSearch_TextChanged;
			Toast.MakeText (this, "数据加载中...", ToastLength.Long).Show ();
			mClient = new WebClient ();
			mUrl = new Uri ("http://cloud.tescar.cn/Vehicle/GetSijiData?isspec=1");
			mClient.DownloadDataAsync (mUrl);
			mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
			// Create your application here
		}

		void mClient_DownloadDataCompleted (object sender, DownloadDataCompletedEventArgs e)
		{
			RunOnUiThread (() => {
				try {
					string json = Encoding.UTF8.GetString (e.Result);
					//mUserinfo = JsonConvert.DeserializeObject<List<user_info>>(json);
					//var fRows = new FormatRows { rows = new List<user_info>() };
					var fRows = JsonConvert.DeserializeObject<FormatRows> (json);
					mTestCase3s = JsonConvert.DeserializeObject<List<testCase3>> (fRows.rows.ToString ());
					//Action<ImageView> action = PicSelected;
					//mAdapter = new UserinfosAdapter(this, Resource.Layout.row_userinfo, mUserinfo, action);
					//mListView.Adapter = mAdapter;
					mAdapter = new TestCase3sAdapter (this, Resource.Layout.row_contact, mTestCase3s);
					mListView.Adapter = mAdapter;

				} catch (Exception ex) {
					Toast.MakeText (this, ex.ToString (), ToastLength.Long).Show ();
				}

			});
		}

		void mSearch_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			if (mTestCase3s != null) {
				List<testCase3> searchedDriver = (from userinfo in mTestCase3s
				                                  where userinfo.lianxidianhua.Contains (mSearch.Text, StringComparison.OrdinalIgnoreCase) || userinfo.sijiname.Contains (mSearch.Text, StringComparison.OrdinalIgnoreCase)
				                                      || userinfo.ownercompanyname.Contains (mSearch.Text, StringComparison.OrdinalIgnoreCase)
				                                  select userinfo).ToList<testCase3> ();

				mAdapter = new TestCase3sAdapter (this, Resource.Layout.row_contact, searchedDriver);
				mListView.Adapter = mAdapter;
			}
            


		}

		void mListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			var callDialog = new AlertDialog.Builder (this);
			callDialog.SetMessage ("Call " + mTestCase3s [e.Position].lianxidianhua + "?");
			callDialog.SetNeutralButton ("Call", delegate {
				// add dialed number to list of called numbers.
				phoneNumbers.Add (mTestCase3s [e.Position].lianxidianhua);

				// Create intent to dial phone
				var callIntent = new Intent (Intent.ActionCall);
				callIntent.SetData (Android.Net.Uri.Parse ("tel:" + mTestCase3s [e.Position].lianxidianhua));
				StartActivity (callIntent);
			});
			callDialog.SetNegativeButton ("Cancel", delegate {
			});

			// Show the alert dialog to the user and wait for response.
			callDialog.Show ();
			Toast.MakeText (this, mTestCase3s [e.Position].lianxidianhua, ToastLength.Long).Show ();
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