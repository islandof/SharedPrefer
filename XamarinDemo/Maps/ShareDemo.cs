using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Java.Interop;

namespace XamarinDemo.Maps
{
    [Activity(Label = "@string/demo_name_share")]
    public class ShareDemo : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_share_demo);
        }

        [Export]
        public void StartShareDemo(View view)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof (ShareDemoActivity));
            StartActivity(intent);
        }
    }
}