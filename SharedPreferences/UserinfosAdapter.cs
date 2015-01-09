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

namespace SharedPreferences
{
    class UserinfosAdapter : BaseAdapter<user_info>
    {
        private Context mContext;
        private int mRowLayout;
        private List<user_info> mUserinfos;
        private int [] mAlternatingColors;

        public UserinfosAdapter(Context context, int rowLayout, List<user_info> Userinfos)
        {
            mContext = context;
            mRowLayout = rowLayout;
            mUserinfos = Userinfos;
            mAlternatingColors = new int[] { 0xF2F2F2, 0x009900 };
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

            row.SetBackgroundColor(GetColorFromInteger(mAlternatingColors[position % mAlternatingColors.Length]));

            
            TextView firstName = row.FindViewById<TextView>(Resource.Id.txtUserName);
            firstName.Text = mUserinfos[position].USER_NAME;

            TextView lastName = row.FindViewById<TextView>(Resource.Id.txtDeptName);
            lastName.Text = mUserinfos[position].ROLE_NAME;

            TextView age = row.FindViewById<TextView>(Resource.Id.txtUserPwd);
            age.Text = mUserinfos[position].USER_PWD;

            TextView gender = row.FindViewById<TextView>(Resource.Id.txtUserEmail);
            gender.Text = mUserinfos[position].companyname;

            if ((position % 2) == 1)
            {
                //Green background, set text white
                firstName.SetTextColor(Color.White);
                lastName.SetTextColor(Color.White);
                age.SetTextColor(Color.White);
                gender.SetTextColor(Color.White);
            }

            else
            {
                //White background, set text black
                firstName.SetTextColor(Color.Black);
                lastName.SetTextColor(Color.Black);
                age.SetTextColor(Color.Black);
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