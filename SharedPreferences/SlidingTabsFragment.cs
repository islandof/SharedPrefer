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
        //private static Testcase2sAdapter mAdapter2;
        //public SlidingTabsFragment(List<testCase1> mItemList, Testcase1sAdapter adapter1, Testcase2sAdapter adapter2)
        //{
        //    mTestcase1 = mItemList;
        //    mAdapter1 = adapter1;
        //    mAdapter2 = adapter2;
        //}

        public SlidingTabsFragment(List<testCase1> mItemList, Testcase1sAdapter adapter1)
        {
            mTestcase1 = mItemList;
            mAdapter1 = adapter1;
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
                items.Add("Σ�ռ�ʻ1");
                items.Add("Σ�ռ�ʻ2");
                items.Add("Σ�ռ�ʻ3");
                items.Add("Σ�ռ�ʻ4");
                items.Add("Σ�ռ�ʻ5");             
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
                mAdapter1.Frameint = position;
                Testcase1sAdapter adapter = mAdapter1;
                //txtTitle.Text = pos.ToString();
                switch ((position+1) % 6)
                {
                    case 1:
                        txtrow1.Text = "����";
                        txtrow2.Text = "��ɲ��";
                        txtrow3.Text = "������";
                        txtrow4.Text = "���ٱ��";
                        txtrow5.Text = "�������";                        
                        break;
                    case 2:
                        txtrow1.Text = "����";
                        txtrow2.Text = "��ײ";
                        txtrow3.Text = "Ƶ�����";
                        txtrow4.Text = "��·������ʻ";
                        txtrow5.Text = "��ת��";
                        
                        break;
                    case 3:
                        txtrow1.Text = "����";
                        txtrow2.Text = "����";
                        txtrow3.Text = "�쳣��";
                        txtrow4.Text = "�����쳣";
                        txtrow5.Text = "̥ѹ����ɲ�쳣";
                        
                        break;
                    case 4:
                        txtrow1.Text = "����";
                        txtrow2.Text = "���ٱ���";
                        txtrow3.Text = "ˮ�±���";
                        txtrow4.Text = "ת�ٱ���";
                        txtrow5.Text = "��ѹ����";
                        
                        break;
                    case 5:
                        txtrow1.Text = "����";
                        txtrow2.Text = "���ϱ���";
                        txtrow3.Text = "ת��";
                        txtrow4.Text = "";
                        txtrow5.Text = "";
                        
                        break;

                }                      
                mListView.Adapter = adapter;
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