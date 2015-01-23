using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Entity;

namespace XamarinDemo
{
    public class Testcase4sAdapter : BaseAdapter<testCase4>
    {
        private readonly int[] mAlternatingColors;
        private readonly Context mContext;
        private readonly int mRowLayout;
        private readonly List<testCase4> mTestCase4s;

        public Testcase4sAdapter(Context context, int rowLayout, List<testCase4> testCase4S)
        {
            mContext = context;
            mRowLayout = rowLayout;
            mTestCase4s = testCase4S;
            mAlternatingColors = new[] {0xF2F2F2, 0x009900};
        }

        public override int Count
        {
            get { return mTestCase4s.Count; }
        }

        public override testCase4 this[int position]
        {
            get { return mTestCase4s[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(mRowLayout, parent, false);
            }

            row.SetBackgroundColor(GetColorFromInteger(mAlternatingColors[position%mAlternatingColors.Length]));

            row.FindViewById<TextView>(Resource.Id.txtchepaino).Text = mTestCase4s[position].chepaino;
            row.FindViewById<TextView>(Resource.Id.txtrow1).Text = mTestCase4s[position].brandname + "-" +
                                                                   mTestCase4s[position].stylename;
            row.FindViewById<TextView>(Resource.Id.txtrow2).Text = mTestCase4s[position].pailiangvalue;
            row.FindViewById<TextView>(Resource.Id.txtrow3).Text = mTestCase4s[position].sijiname;
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

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }
    }
}