using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Entity;

namespace XamarinDemo
{
    public class TestcasesAdapter : BaseAdapter<user_info>
    {
        private readonly int[] mAlternatingColors;
        private readonly Context mContext;
        private readonly int mRowLayout;
        private readonly List<user_info> mUserinfos;

        public TestcasesAdapter(Context context, int rowLayout, List<user_info> Userinfos)
        {
            mContext = context;
            mRowLayout = rowLayout;
            mUserinfos = Userinfos;
            mAlternatingColors = new[] {0xF2F2F2, 0x009900};
        }

        public override int Count
        {
            get { return mUserinfos.Count; }
        }

        public override user_info this[int position]
        {
            get { return mUserinfos[position]; }
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


            var firstName = row.FindViewById<TextView>(Resource.Id.txtrow1);
            firstName.Text = mUserinfos[position].USER_NAME;

            var lastName = row.FindViewById<TextView>(Resource.Id.txtrow2);
            lastName.Text = mUserinfos[position].ROLE_NAME;

            var gender = row.FindViewById<TextView>(Resource.Id.txtrow3);
            gender.Text = mUserinfos[position].companyname;

            if ((position%2) == 1)
            {
                //Green background, set text white
                firstName.SetTextColor(Color.White);
                lastName.SetTextColor(Color.White);
                gender.SetTextColor(Color.White);
            }

            else
            {
                //White background, set text black
                firstName.SetTextColor(Color.Black);
                lastName.SetTextColor(Color.Black);
                gender.SetTextColor(Color.Black);
            }

            return row;
        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }
    }
}