using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Opengl;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using Entity;
using Newtonsoft.Json;

//using Baidumapsdk.Demox;
//using XamarinDemo.Maps;

namespace XamarinDemo
{
	[Activity (Label = "Login", ScreenOrientation = ScreenOrientation.Sensor)]
	public class Login : Activity
	{
		RelativeLayout mRelativeLayout;
		Button mButton;
		private EditText mUserText;
		private EditText mPwd;
		private CheckBox checkbox;
		private WebClient mClient;
		private Uri mUrl;
		private user_info mUserinfo;
		private ProgressBar mProgressBar;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

			mRelativeLayout = FindViewById<RelativeLayout> (Resource.Id.mainView);
			mButton = FindViewById<Button> (Resource.Id.btnLogin);
			mUserText = FindViewById<EditText> (Resource.Id.txtUser);
			mPwd = FindViewById<EditText> (Resource.Id.txtPwd);

			mProgressBar = FindViewById<ProgressBar> (Resource.Id.progressBar1);
			mButton.Click += mButton_Click;
			mRelativeLayout.Click += mRelativeLayout_Click;
			checkbox = FindViewById<CheckBox> (Resource.Id.checkBox1);
			checkbox.CheckedChange += (s, e) => {
				if (checkbox.Checked) {
					retrieveset ();
				}
			};


		}


		void mButton_Click (object sender, EventArgs e)
		{            
			mClient = new WebClient ();
			mUrl = new Uri ("http://cloud.tescar.cn/home/LoginSubmit2?UserName=" + mUserText.Text + "&Pwd=" + mPwd.Text + "&isspe=1");
			mClient.DownloadDataAsync (mUrl);
			mProgressBar.Visibility = ViewStates.Visible;
			mButton.Enabled = false;
			mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
			retrieveset ();
		}

		void mClient_DownloadDataCompleted (object sender, DownloadDataCompletedEventArgs e)
		{

		
			RunOnUiThread (() => {
				try {
					string json = Encoding.UTF8.GetString (e.Result);
					mUserinfo = JsonConvert.DeserializeObject<user_info> (json);
					//Action<ImageView> action = PicSelected;
					//mAdapter = new UserinfosAdapter(this, Resource.Layout.row_userinfo, mUserinfo, action);
					//mListView.Adapter = mAdapter;

					if (mUserinfo.USER_NAME != null) {
						//判断是否保持状态
						if (checkbox.Checked == true) {
							saveset (mUserinfo);
						}

						mButton.Enabled = true;
						mProgressBar.Visibility = ViewStates.Invisible;
						Intent intent = new Intent (this, typeof(Main));

						this.StartActivity (intent);
						this.OverridePendingTransition (Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);    
					}
					mButton.Enabled = true;
				} catch (Exception ex) {
					mButton.Enabled = true;
					Toast.MakeText (this, ex.ToString (), ToastLength.Long).Show ();
				}
			});
			
		}

		void mRelativeLayout_Click (object sender, EventArgs e)
		{
			InputMethodManager inputManager = (InputMethodManager)this.GetSystemService (Activity.InputMethodService);
			inputManager.HideSoftInputFromWindow (this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
		}

		// Function called from OnDestroy
		protected void saveset (user_info userinfo)
		{
			//store
			var prefs = Application.Context.GetSharedPreferences("VehicleMonitor", FileCreationMode.Private);
			var prefEditor = prefs.Edit ();
			prefEditor.PutInt ("UserId", userinfo.USER_ID);
			prefEditor.Commit ();

		}

		protected void retrieveset ()
		{
			//retreive 
			var prefs = Application.Context.GetSharedPreferences("VehicleMonitor", FileCreationMode.Private);              
			var somePref = prefs.GetInt ("UserId", 0);

			//Show a toast
			RunOnUiThread (() => Toast.MakeText (this, somePref.ToString (), ToastLength.Long).Show ());

		}
	}
}

