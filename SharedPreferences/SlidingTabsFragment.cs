using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Entity;

namespace SharedPreferences
{
    public class SlidingTabsFragment : Fragment
    {
        private SlidingTabScrollView mSlidingTabScrollView;
        private ViewPager mViewPager;
        private List<testCase1> mTestcase1;
        private static ListView mListView;
        private static Testcase1sAdapter mAdapter1;
        private static Testcase2sAdapter mAdapter2;
        public SlidingTabsFragment(List<testCase1> mItemList, Testcase1sAdapter adapter1, Testcase2sAdapter adapter2)
        {
            mTestcase1 = mItemList;
            mAdapter1 = adapter1;
            mAdapter2 = adapter2;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_sample, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            mSlidingTabScrollView = view.FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            mViewPager.Adapter = new SamplePagerAdapter();
           
            mSlidingTabScrollView.ViewPager = mViewPager;
        }

        public class SamplePagerAdapter : PagerAdapter
        {
            List<string> items = new List<string>();

            public SamplePagerAdapter() : base()
            {
                items.Add("Œ£œ’º› ª1");
                items.Add("Œ£œ’º› ª2");
                items.Add("Œ£œ’º› ª3");
                items.Add("Œ£œ’º› ª4");
                items.Add("Œ£œ’º› ª5");
                items.Add("Œ£œ’º› ª6");
            }

            public override int Count
            {
                get { return items.Count; }
            }

            public override bool IsViewFromObject(View view, Java.Lang.Object obj)
            {
                return view == obj;
            }

            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                
                View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.pager_item, container, false);
                mListView = view.FindViewById<ListView>(Resource.Id.listView);
                var txtrow1 = view.FindViewById<TextView>(Resource.Id.txtrow1);
                var txtrow2 = view.FindViewById<TextView>(Resource.Id.txtrow2);
                var txtrow3 = view.FindViewById<TextView>(Resource.Id.txtrow3);
                var txtrow4 = view.FindViewById<TextView>(Resource.Id.txtrow4);
                var txtrow5 = view.FindViewById<TextView>(Resource.Id.txtrow5);
                container.AddView(view);
                
                //TextView txtTitle = view.FindViewById<TextView>(Resource.Id.item_title);
                int pos = position + 1;
                //txtTitle.Text = pos.ToString();
                if (pos%2 == 1)
                {
                    txtrow1.Text = "≥µ≈∆";
                    txtrow2.Text = "º±º””Õ";
                    txtrow3.Text = "øÏÀŸ±‰µ¿";
                    txtrow4.Text = "Õ‰µ¿º”ÀŸ";
                    txtrow5.Text = "≈ˆ◊≤";
                    mListView.Adapter = mAdapter1;
                    
                }
                else
                {
                    txtrow1.Text = "≥µ≈∆";
                    txtrow2.Text = "∆µ∑±±‰µ¿";
                    txtrow3.Text = "¿√¬∑∏ﬂÀŸ–– ª";
                    txtrow4.Text = "º±◊™Õ‰";
                    txtrow5.Text = "∑≠≥µ";
                    mListView.Adapter = mAdapter2;
                }
                return view;
            }

            public string GetHeaderTitle (int position)
            {
                return items[position];
            }

            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
            {
                container.RemoveView((View)obj);
            }
        }
    }
}