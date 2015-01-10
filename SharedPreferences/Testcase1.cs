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
using Android.Views.InputMethods;
using Android.Widget;
using Entity;
using Newtonsoft.Json;

namespace SharedPreferences
{
    [Activity(Label = "Testcase1")]
    public class Testcase1 : Activity
    {
        private List<testCase1> mTestcase1;
        private ListView mListView;
        private EditText mSearch;
        private LinearLayout mContainer;
        private bool mAnimatedDown;
        private bool mIsAnimating;
        private Testcase1sAdapter mAdapter;
        private WebClient mClient;
        private Uri mUrl;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TestCase1);
            mListView = FindViewById<ListView>(Resource.Id.listView);
            mSearch = FindViewById<EditText>(Resource.Id.etSearch1);
            mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer1);

            mSearch.Alpha = 0;
            mSearch.TextChanged += mSearch_TextChanged;
            mClient = new WebClient();
            mUrl = new Uri("http://cloud.tescar.cn/vehicle/GetTboxalarmintimeData?isspec=1");
            mClient.DownloadDataAsync(mUrl);
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
            // Create your application here
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
                mAdapter = new Testcase1sAdapter(this, Resource.Layout.row_testcase11, mTestcase1);
                mListView.Adapter = mAdapter;
            });
        } 

        void mSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {

            //List<user_info> searchedFriends = (from userinfo in mUserinfo
            //                                   where userinfo.USER_NAME.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || userinfo.ROLE_NAME.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
            //                                || userinfo.USER_PWD.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || userinfo.USER_EMAIL.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
            //                                   select userinfo).ToList<user_info>();

            //mAdapter = new UserinfosAdapter(this, Resource.Layout.row_userinfo, searchedFriends);
            //mListView.Adapter = mAdapter;


        }

        void anim_AnimationEndUp(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {
            mIsAnimating = false;
            mSearch.ClearFocus();
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
        }

        void anim_AnimationEndDown(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {
            mIsAnimating = false;
        }

        void anim_AnimationStartDown(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(1.0f).SetDuration(500).Start();
        }

        void anim_AnimationStartUp(object sender, Android.Views.Animations.Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(-1.0f).SetDuration(300).Start();
        }
    }
}