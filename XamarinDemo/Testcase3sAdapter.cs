using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Entity;

namespace XamarinDemo
{
	class TestCase3sAdapter : BaseAdapter<testCase3>
	{
		private Context mContext;
		private int mLayout;
		private List<testCase3> mtestCase3s;
		private Action<ImageView> mActionPicSelected;

		public TestCase3sAdapter (Context context, int layout, List<testCase3> contacts)
		{
			mContext = context;
			mLayout = layout;
			mtestCase3s = contacts;
		}

		public override testCase3 this [int position] {
			get { return mtestCase3s [position]; }
		}

		public override int Count {
			get { return mtestCase3s.Count; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View row = convertView;

			if (row == null) {
				row = LayoutInflater.From (mContext).Inflate (mLayout, parent, false);
			}

			row.FindViewById<TextView> (Resource.Id.txtName).Text = mtestCase3s [position].ownercompanyname + ":" + mtestCase3s [position].sijiname;
			row.FindViewById<TextView> (Resource.Id.txtNumber).Text = mtestCase3s [position].lianxidianhua;

			ImageView pic = row.FindViewById<ImageView> (Resource.Id.imgPic);

			//if (mtestCase3s[position].Image != null)
			//{
			//    pic.SetImageBitmap(BitmapFactory.DecodeByteArray(mtestCase3s[position].Image, 0, mtestCase3s[position].Image.Length));
			//}

//            pic.Click -= pic_Click;
//            pic.Click += pic_Click;
			return row;
		}

		void pic_Click (object sender, EventArgs e)
		{
			//Picture has been clicked
			mActionPicSelected.Invoke ((ImageView)sender);
		}
	}
}