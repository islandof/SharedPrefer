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

namespace SharedPreferences
{
    public class Testcase2sAdapter : BaseAdapter<testCase1>
    {
        private Context mContext;
        private int mRowLayout;
        private List<testCase1> mTestcase1s;
        private int [] mAlternatingColors;

        public Testcase2sAdapter(Context context, int rowLayout, List<testCase1> Testcase1s)
        {
            mContext = context;
            mRowLayout = rowLayout;
            mTestcase1s = Testcase1s;
            mAlternatingColors = new int[] { 0xF2F2F2, 0x009900 };
        }

        public override int Count
        {
            get { return mTestcase1s.Count; }
        }

        public override testCase1 this[int position]
        {
            get { return mTestcase1s[position]; }
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

            row.SetBackgroundColor(GetColorFromInteger(mAlternatingColors[position % mAlternatingColors.Length]));

            row.FindViewById<TextView>(Resource.Id.txtchepaino).Text = mTestcase1s[position].chepaino;            
            row.FindViewById<TextView>(Resource.Id.txtAla15).Text = mTestcase1s[position].Ala15;
            row.FindViewById<TextView>(Resource.Id.txtAla16).Text = mTestcase1s[position].Ala16;
            row.FindViewById<TextView>(Resource.Id.txtAla17).Text = mTestcase1s[position].Ala17;
            row.FindViewById<TextView>(Resource.Id.txtAla18).Text = mTestcase1s[position].Ala18;

            return row;
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }
    }
}