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
	public class Testcase5sAdapter : BaseAdapter<testCase5>
	{
		private Context mContext;
		private int mRowLayout;
		private List<testCase5> mDataList;
		private int[] mAlternatingColors;

		public Testcase5sAdapter (Context context, int rowLayout, List<testCase5> testCase5S)
		{
			mContext = context;
			mRowLayout = rowLayout;
			mDataList = testCase5S;
			mAlternatingColors = new int[] { 0xF2F2F2, 0x009900 };
		}

		public override int Count {
			get { return mDataList.Count; }
		}

		public override testCase5 this [int position] {
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

            row.FindViewById<TextView>(Resource.Id.txtrow1).Text = mDataList[position].areaname;
            var strrow3 = "";
            if (mDataList[position].condition == "1")
            {
                strrow3 += "进栅栏";
            }
            else
            {
                strrow3 += "出栅栏";
            }
            row.FindViewById<TextView>(Resource.Id.txtrow2).Text = strrow3;
            row.FindViewById<TextView>(Resource.Id.txtrow3).Text = "百度地图";
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