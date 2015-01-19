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
	public class Testcase2sAdapter : BaseAdapter<testCase2>
	{
		private Context mContext;
		private int mRowLayout;
		private List<testCase2> mTestCase2s;
		private int[] mAlternatingColors;

		public Testcase2sAdapter (Context context, int rowLayout, List<testCase2> TestCase2s)
		{
			mContext = context;
			mRowLayout = rowLayout;
			mTestCase2s = TestCase2s;
			mAlternatingColors = new int[] { 0xF2F2F2, 0x009900 };
		}

		public override int Count {
			get { return mTestCase2s.Count; }
		}

		public override testCase2 this [int position] {
			get { return mTestCase2s [position]; }
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

			row.FindViewById<TextView> (Resource.Id.txtchepaino).Text = mTestCase2s [position].chepaino;
			row.FindViewById<TextView> (Resource.Id.txtrow1).Text = mTestCase2s [position].dthappen.Replace ("T", "").Substring (0, 12);
			row.FindViewById<TextView> (Resource.Id.txtrow2).Text = mTestCase2s [position].zhalanname;
			var strrow3 = "";
			if (mTestCase2s [position].zhalantype == "1") {
				strrow3 = "时间";
			} else {
				strrow3 = "位置";
			}
			if (mTestCase2s [position].condition == "1") {
				strrow3 += "内";
			} else {
				strrow3 += "外";
			}

			row.FindViewById<TextView> (Resource.Id.txtrow3).Text = strrow3;

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