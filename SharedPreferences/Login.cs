using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Opengl;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using Newtonsoft.Json;

namespace SharedPreferences
{
    [Activity(Label = "SharedPreferences", MainLauncher = true, Icon = "@drawable/icon")]
    public class Login : Activity
    {
        RelativeLayout mRelativeLayout;
        Button mButton;
        private EditText mUserText;
        private EditText mPwd;
        private WebClient mClient;
        private Uri mUrl;
        private user_info mUserinfo;
        private ProgressBar mProgressBar;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            mRelativeLayout = FindViewById<RelativeLayout>(Resource.Id.mainView);
            mButton = FindViewById<Button>(Resource.Id.btnLogin);
            mUserText = FindViewById<EditText>(Resource.Id.txtUser);
            mPwd = FindViewById<EditText>(Resource.Id.txtPwd);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            mButton.Click += mButton_Click;
            mRelativeLayout.Click += mRelativeLayout_Click;

        }

        void mButton_Click(object sender, EventArgs e)
        {            
            mClient = new WebClient();
            mUrl = new Uri("http://192.168.1.101:8011/home/LoginSubmit2?UserName=" + mUserText.Text + "&Pwd=" + mPwd.Text + "&isspe=1");
            mClient.DownloadDataAsync(mUrl);
            mProgressBar.Visibility = ViewStates.Visible;
            mButton.Enabled = false;
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
            
        }

        void mClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                mUserinfo = JsonConvert.DeserializeObject<user_info>(json);
                //Action<ImageView> action = PicSelected;
                //mAdapter = new UserinfosAdapter(this, Resource.Layout.row_userinfo, mUserinfo, action);
                //mListView.Adapter = mAdapter;
                if (mUserinfo.USER_NAME != null)
                {
                    mButton.Enabled = true;
                    mProgressBar.Visibility = ViewStates.Invisible;
                    Intent intent = new Intent(this, typeof(Activity2));
                    this.StartActivity(intent);
                    this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);    
                }
                mButton.Enabled = true;
            });
        } 

        void mRelativeLayout_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
        }
    }
}

