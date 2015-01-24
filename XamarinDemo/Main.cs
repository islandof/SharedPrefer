using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;

namespace XamarinDemo
{
    [Activity(Label = "主界面", MainLauncher = true, ScreenOrientation = ScreenOrientation.Sensor)]
    public class Main : TabActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            //retrieveset();

            CreateTab(typeof (MainMonitor), "Monitor", "监控中心", Resource.Drawable.ic_tab_whats_on);
            CreateTab(typeof (CarManage), "carmanage", "车辆管理", Resource.Drawable.ic_tab_speakers);
            CreateTab(typeof (SijiList), "siji", "司机管理", Resource.Drawable.ic_tab_sessions);
            CreateTab(typeof (UserLActivity), "author", "权限管理", Resource.Drawable.ic_tab_my_schedule);
            // Create your application here
        }

        private void CreateTab(Type activityType, string tag, string label, int drawableId)
        {
            var intent = new Intent(this, activityType);
            intent.AddFlags(ActivityFlags.NewTask);

            TabHost.TabSpec spec = TabHost.NewTabSpec(tag);
            Drawable drawableIcon = Resources.GetDrawable(drawableId);
            spec.SetIndicator(label, drawableIcon);
            spec.SetContent(intent);

            TabHost.AddTab(spec);
        }

        protected void retrieveset()
        {
            //retreive 
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("VehicleMonitor",
                FileCreationMode.Private);
            int somePref = prefs.GetInt("UserId", 0);

            //Show a toast
            RunOnUiThread(() => Toast.MakeText(this, somePref.ToString(), ToastLength.Long).Show());
        }
    }
}