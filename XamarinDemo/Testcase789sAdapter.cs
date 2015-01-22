using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Entity;

namespace XamarinDemo
{
	public class Testcase789sAdapter : BaseAdapter<testCase789>
	{
		private Context mContext;
		private int mRowLayout;
		private List<testCase789> mDataList;
		private int[] mAlternatingColors;
	    private int mTnum;
        public Testcase789sAdapter(Context context, int rowLayout, List<testCase789> DataList,int tnum)
		{
			mContext = context;
			mRowLayout = rowLayout;
            mDataList = DataList;
			mAlternatingColors = new int[] { 0xF2F2F2, 0x009900 };
            mTnum = tnum;
		}

		public override int Count {
			get { return mDataList.Count; }
		}

        public override testCase789 this[int position]
        {
			get { return mDataList [position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View row = convertView;

			if (row == null) {
				row = LayoutInflater.From (mContext).Inflate (mRowLayout, parent, false);
			}

			row.SetBackgroundColor (GetColorFromInteger (mAlternatingColors [position % mAlternatingColors.Length]));
		    if (mTnum == 7)
		    {
                row.FindViewById<TextView>(Resource.Id.txtrow1).Text = mDataList[position].timename;
                var strrow3 = "";
                if (mDataList[position].timetype == "1")
                {
                    strrow3 = "月循环";
                }
                else if (mDataList[position].timetype == "2")
                {
                    strrow3 = "周循环";
                }
                else
                {
                    strrow3 = "日循环";
                }
                row.FindViewById<TextView>(Resource.Id.txtrow2).Text = strrow3;
		    }
            if (mTnum == 8)
            {
                row.FindViewById<TextView>(Resource.Id.txtrow1).Text = mDataList[position].chaosuname;
                row.FindViewById<TextView>(Resource.Id.txtrow2).Text = mDataList[position].maxspeed;
            }
            if (mTnum == 9)
            {
                row.FindViewById<TextView>(Resource.Id.txtrow1).Text = mDataList[position].daisuname;
                row.FindViewById<TextView>(Resource.Id.txtrow2).Text = mDataList[position].maxsecond;
            }
                        
            //row.FindViewById<TextView> (Resource.Id.txtrow1).Text = mTestCase2s [position].dthappen.Replace ("T", "").Substring (0, 12);
            //row.FindViewById<TextView> (Resource.Id.txtrow2).Text = mTestCase2s [position].zhalanname;
            //var strrow3 = "";
            //if (mTestCase2s [position].zhalantype == "1") {
            //    strrow3 = "时间";
            //} else {
            //    strrow3 = "位置";
            //}
            //if (mTestCase2s [position].condition == "1") {
            //    strrow3 += "内";
            //} else {
            //    strrow3 += "外";
            //}

            //row.FindViewById<TextView> (Resource.Id.txtrow3).Text = strrow3;

//			if ((position % 2) == 1) {
//				//Green background, set text white
//				firstName.SetTextColor (Color.White);
//				lastName.SetTextColor (Color.White);
//				age.SetTextColor (Color.White);
//				gender.SetTextColor (Color.White);
//			} else {
//				//White background, set text black
//				firstName.SetTextColor (Color.Black);
//				lastName.SetTextColor (Color.Black);
//				age.SetTextColor (Color.Black);
//				gender.SetTextColor (Color.Black);
//			}

			return row;
		}

		private Color GetColorFromInteger (int color)
		{
			return Color.Rgb (Color.GetRedComponent (color), Color.GetGreenComponent (color), Color.GetBlueComponent (color));
		}
	}
}