﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using Entity;
using Newtonsoft.Json;

namespace XamarinDemo
{
    [Activity(Label = "用户列表", ScreenOrientation = ScreenOrientation.Sensor)]
    public class UserLActivity : Activity
    {
        private TestcasesAdapter mAdapter;
        private bool mAnimatedDown;
        private WebClient mClient;
        private LinearLayout mContainer;
        private bool mIsAnimating;
        private ListView mListView;
        private EditText mSearch;
        private Uri mUrl;
        private List<user_info> mUserinfo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Create your application here
            SetContentView(Resource.Layout.TestCase);
            mListView = FindViewById<ListView>(Resource.Id.listView);
            mSearch = FindViewById<EditText>(Resource.Id.etSearch);
            mContainer = FindViewById<LinearLayout>(Resource.Id.llContainer);
            mSearch.Alpha = 0;
            mSearch.TextChanged += mSearch_TextChanged;
            Toast.MakeText(this, "数据加载中...", ToastLength.Long).Show();
            mClient = new WebClient();
            mUrl = new Uri("http://cloud.tescar.cn/manage/getuserinfodata?isspec=1");
            mClient.DownloadDataAsync(mUrl);
            mClient.DownloadDataCompleted += mClient_DownloadDataCompleted;
        }


        private void mSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (mUserinfo != null)
            {
                List<user_info> searchedFriends = (from userinfo in mUserinfo
                    where
                        userinfo.USER_NAME.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase) ||
                        userinfo.ROLE_NAME.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
                        || userinfo.USER_EMAIL.Contains(mSearch.Text, StringComparison.OrdinalIgnoreCase)
                    select userinfo).ToList<user_info>();

                mAdapter = new TestcasesAdapter(this, Resource.Layout.row_testcase, searchedFriends);
                mListView.Adapter = mAdapter;
            }
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
                        var anim = new MyAnimation(mListView, mListView.Height - mSearch.Height);
                        anim.Duration = 500;
                        mListView.StartAnimation(anim);
                        anim.AnimationStart += anim_AnimationStartDown;
                        anim.AnimationEnd += anim_AnimationEndDown;
                        mContainer.Animate().TranslationYBy(mSearch.Height).SetDuration(500).Start();
                    }
                    else
                    {
                        //Listview is down
                        var anim = new MyAnimation(mListView, mListView.Height + mSearch.Height);
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

        private void mClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                try
                {
                    string json = Encoding.UTF8.GetString(e.Result);
                    //mUserinfo = JsonConvert.DeserializeObject<List<user_info>>(json);
                    //var fRows = new FormatRows { rows = new List<user_info>() };
                    var fRows = JsonConvert.DeserializeObject<FormatRows>(json);
                    mUserinfo = JsonConvert.DeserializeObject<List<user_info>>(fRows.rows.ToString());
                    //Action<ImageView> action = PicSelected;
                    //mAdapter = new UserinfosAdapter(this, Resource.Layout.row_userinfo, mUserinfo, action);
                    //mListView.Adapter = mAdapter;
                    mAdapter = new TestcasesAdapter(this, Resource.Layout.row_testcase, mUserinfo);
                    mListView.Adapter = mAdapter;
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                }
            });
        }

        private void anim_AnimationEndUp(object sender, Animation.AnimationEndEventArgs e)
        {
            mIsAnimating = false;
            mSearch.ClearFocus();
            var inputManager = (InputMethodManager) GetSystemService(InputMethodService);
            inputManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
        }

        private void anim_AnimationEndDown(object sender, Animation.AnimationEndEventArgs e)
        {
            mIsAnimating = false;
        }

        private void anim_AnimationStartDown(object sender, Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(1.0f).SetDuration(500).Start();
        }

        private void anim_AnimationStartUp(object sender, Animation.AnimationStartEventArgs e)
        {
            mIsAnimating = true;
            mSearch.Animate().AlphaBy(-1.0f).SetDuration(300).Start();
        }
    }
}