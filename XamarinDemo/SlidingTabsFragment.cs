using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Entity;
using Java.Lang;

namespace XamarinDemo
{
    public class SlidingTabsFragment : Fragment
    {
        private static ListView mListView;
        private static Testcase1sAdapter mAdapter1;
        private SlidingTabScrollView mSlidingTabScrollView;
        private List<testCase1> mTestcase1;
        private ViewPager mViewPager;
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
            private readonly List<string> items = new List<string>();

            public SamplePagerAdapter()
            {
                items.Add("危险驾驶1");
                items.Add("危险驾驶2");
                items.Add("危险驾驶3");
                items.Add("危险驾驶4");
                items.Add("危险驾驶5");
            }

            public override int Count
            {
                get { return items.Count; }
            }

            public override bool IsViewFromObject(View view, Object obj)
            {
                return view == obj;
            }

            public override Object InstantiateItem(ViewGroup container, int position)
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
                //int pos = position + 1;
                mAdapter1.Frameint = position;
                Testcase1sAdapter adapter = mAdapter1;
                //txtTitle.Text = pos.ToString();
                switch ((position + 1)%6)
                {
                    case 1:
                        txtrow1.Text = "车牌";
                        txtrow2.Text = "急刹车";
                        txtrow3.Text = "急加油";
                        txtrow4.Text = "快速变道";
                        txtrow5.Text = "弯道加速";
                        break;
                    case 2:
                        txtrow1.Text = "车牌";
                        txtrow2.Text = "碰撞";
                        txtrow3.Text = "频繁变道";
                        txtrow4.Text = "烂路高速";
                        txtrow5.Text = "急转弯";

                        break;
                    case 3:
                        txtrow1.Text = "车牌";
                        txtrow2.Text = "翻车";
                        txtrow3.Text = "异常震动";
                        txtrow4.Text = "车门异常";
                        txtrow5.Text = "胎压手刹";

                        break;
                    case 4:
                        txtrow1.Text = "车牌";
                        txtrow2.Text = "超速报警";
                        txtrow3.Text = "水温报警";
                        txtrow4.Text = "转速报警";
                        txtrow5.Text = "电压报警";

                        break;
                    case 5:
                        txtrow1.Text = "车牌";
                        txtrow2.Text = "故障报警";
                        txtrow3.Text = "转弯";
                        txtrow4.Text = "";
                        txtrow5.Text = "";

                        break;
                }
                mListView.Adapter = adapter;
                return view;
            }

            public string GetHeaderTitle(int position)
            {
                return items[position];
            }

            public override void DestroyItem(ViewGroup container, int position, Object obj)
            {
                container.RemoveView((View) obj);
            }
        }
    }
}