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
    [Activity(Label = "Activity2")]
    public class Testcase : Activity
    {
        private List<user_info> mUserinfo;
        private ListView mListView;
        private EditText mSearch;
        private LinearLayout mContainer;
        private bool mAnimatedDown;
        private bool mIsAnimating;
        private TestcasesAdapter mAdapter;
        private WebClient mClient;
        private Uri mUrl;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.TesctCase);
            mListView = FindViewById<ListView>(Resource.Id.listView);
            mSearch = FindViewById<EditText>(Resource.Id.etSearch);
            mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer);

            mSearch.Alpha = 0;
            mSearch.TextChanged += mSearch_TextChanged;
            mClient = new WebClient();
            mUrl = new Uri("http://cloud.tescar.cn/manage/getuserinfodata?isspec=1");
            mClient.DownloadDataAsync(mUrl);
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;

            //mUserinfo = new List<user_info>();
            //mUserinfo.Add(new user_info { USER_NAME = "Bob", DEPT_NAME = "Smith", USER_PWD = "33", USER_EMAIL = "Male" });
            //mUserinfo.Add(new user_info { USER_NAME = "Tom", DEPT_NAME = "Smith", USER_PWD = "45", USER_EMAIL = "Male" });
            //mUserinfo.Add(new user_info { USER_NAME = "Julie", DEPT_NAME = "Smith", USER_PWD = "2020", USER_EMAIL = "Unknown" });
            //mUserinfo.Add(new user_info { USER_NAME = "Molly", DEPT_NAME = "Smith", USER_PWD = "21", USER_EMAIL = "Female" });
            //mUserinfo.Add(new user_info { USER_NAME = "Joe", DEPT_NAME = "Lopez", USER_PWD = "22", USER_EMAIL = "Male" });
            //mUserinfo.Add(new user_info { USER_NAME = "Ruth", DEPT_NAME = "White", USER_PWD = "81", USER_EMAIL = "Female" });
            //mUserinfo.Add(new user_info { USER_NAME = "Sally", DEPT_NAME = "Johnson", USER_PWD = "54", USER_EMAIL = "Female" });
            //mAdapter = new UserinfosAdapter(this, Resource.Layout.row_userinfo, mUserinfo);
            //mListView.Adapter = mAdapter;
        }

        void mSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {

            List<user_info> searchedFriends = (from userinfo in mUserinfo
                                               where userinfo.USER_NAME.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || userinfo.ROLE_NAME.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
                                            || userinfo.USER_PWD.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) || userinfo.USER_EMAIL.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
                                               select userinfo).ToList<user_info>();

            mAdapter = new TestcasesAdapter(this, Resource.Layout.row_userinfo, searchedFriends);
            mListView.Adapter = mAdapter;


        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {

                case Resource.Id.search:
                    //Search icon has been clicked

                    if (mIsAnimating)
                    {
                        return true;
                    }

                    if (!mAnimatedDown)
                    {
                        //Listview is up
                        MyAnimation anim = new MyAnimation(mListView, mListView.Height - mSearch.Height);
                        anim.Duration = 500;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartDown;
                        anim.AnimationEnd += anim_AnimationEndDown;
                        mContainer.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
                    }

                    else
                    {
                        //Listview is down
                        MyAnimation anim = new MyAnimation(mListView, mListView.Height + mSearch.Height);
                        anim.Duration = 500;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartUp;
                        anim.AnimationEnd += anim_AnimationEndUp;
                        mContainer.Animate().TranslationYBy(-mSearch.Height).SetDuration(500).Start();
                    }

                    mAnimatedDown = !mAnimatedDown;
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        void mClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                //mUserinfo = JsonConvert.DeserializeObject<List<user_info>>(json);
                //var fRows = new FormatRows { rows = new List<user_info>() };
                var fRows = JsonConvert.DeserializeObject<FormatRows>(json);
                mUserinfo = JsonConvert.DeserializeObject<List<user_info>>(fRows.rows.ToString());
                //Action<ImageView> action = PicSelected;
                //mAdapter = new UserinfosAdapter(this, Resource.Layout.row_userinfo, mUserinfo, action);
                //mListView.Adapter = mAdapter;
                mAdapter = new TestcasesAdapter(this, Resource.Layout.row_userinfo, mUserinfo);
                mListView.Adapter = mAdapter;
            });
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