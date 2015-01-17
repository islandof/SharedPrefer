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
	public class Testcase1sAdapter : BaseAdapter<testCase1>
	{
		private Context mContext;
		private int mRowLayout;
		private int[] mRowLayoutList;
		private List<testCase1> mTestcase1s;
		private int[] mAlternatingColors;
		//private int _frameint;

		public Testcase1sAdapter (Context context, int rowLayout, List<testCase1> testcase1S)
		{
			mContext = context;
			mRowLayout = rowLayout;
			mTestcase1s = testcase1S;
			mAlternatingColors = new int[] { 0xF2F2F2, 0x009900 };
		}

		public Testcase1sAdapter (Context context, int[] rowLayout, List<testCase1> testcase1S)
		{
			mContext = context;
			mRowLayoutList = rowLayout;
			mTestcase1s = testcase1S;
			mAlternatingColors = new int[] { 0xF2F2F2, 0x009900 };
		}

		public int Frameint { get; set; }

		public override int Count {
			get {
				if (mTestcase1s != null) {
					return mTestcase1s.Count;
				} else {
					return 0;
				}
			}
		}

		public override testCase1 this [int position] {
			get { return mTestcase1s [position]; }
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
			switch ((Frameint + 1) % 6) {
			case 1:
				row.FindViewById<TextView> (Resource.Id.txtchepaino).Text = mTestcase1s [position].chepaino;
				row.FindViewById<TextView> (Resource.Id.txtAla10).Text = mTestcase1s [position].Ala10;
				row.FindViewById<TextView> (Resource.Id.txtAla11).Text = mTestcase1s [position].Ala11;
				row.FindViewById<TextView> (Resource.Id.txtAla12).Text = mTestcase1s [position].Ala12;
				row.FindViewById<TextView> (Resource.Id.txtAla13).Text = mTestcase1s [position].Ala13;
				break;
			case 2:
				row.FindViewById<TextView> (Resource.Id.txtchepaino).Text = mTestcase1s [position].chepaino;
				row.FindViewById<TextView> (Resource.Id.txtAla10).Text = mTestcase1s [position].Ala14;
				row.FindViewById<TextView> (Resource.Id.txtAla11).Text = mTestcase1s [position].Ala15;
				row.FindViewById<TextView> (Resource.Id.txtAla12).Text = mTestcase1s [position].Ala16;
				row.FindViewById<TextView> (Resource.Id.txtAla13).Text = mTestcase1s [position].Ala17;
				break;
			case 3:
				row.FindViewById<TextView> (Resource.Id.txtchepaino).Text = mTestcase1s [position].chepaino;
				row.FindViewById<TextView> (Resource.Id.txtAla10).Text = mTestcase1s [position].Ala18;
				row.FindViewById<TextView> (Resource.Id.txtAla11).Text = mTestcase1s [position].Ala19;
				row.FindViewById<TextView> (Resource.Id.txtAla12).Text = mTestcase1s [position].Ala20;
				row.FindViewById<TextView> (Resource.Id.txtAla13).Text = mTestcase1s [position].Ala21;
				break;
			case 4:
				row.FindViewById<TextView> (Resource.Id.txtchepaino).Text = mTestcase1s [position].chepaino;
				row.FindViewById<TextView> (Resource.Id.txtAla10).Text = mTestcase1s [position].Ala30;
				row.FindViewById<TextView> (Resource.Id.txtAla11).Text = mTestcase1s [position].Ala31;
				row.FindViewById<TextView> (Resource.Id.txtAla12).Text = mTestcase1s [position].Ala32;
				row.FindViewById<TextView> (Resource.Id.txtAla13).Text = mTestcase1s [position].Ala33;
				break;
			case 5:
				row.FindViewById<TextView> (Resource.Id.txtchepaino).Text = mTestcase1s [position].chepaino;
				row.FindViewById<TextView> (Resource.Id.txtAla10).Text = mTestcase1s [position].Ala34;
				row.FindViewById<TextView> (Resource.Id.txtAla11).Text = mTestcase1s [position].Ala35;
				row.FindViewById<TextView> (Resource.Id.txtAla12).Text = "";
				row.FindViewById<TextView> (Resource.Id.txtAla13).Text = "";
				break;

			}    
			return row;


            
		}

		private Color GetColorFromInteger (int color)
		{
			return Color.Rgb (Color.GetRedComponent (color), Color.GetGreenComponent (color), Color.GetBlueComponent (color));
		}
	}
}