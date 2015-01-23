using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Java.Interop;

namespace XamarinDemo.Maps
{
    [Activity(Label = "@string/title_activity_cloud_search_demo")]
    public class CloudSearchDemo : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cloud_search_demo);
        }

        [Export]
        public void StartCloudSearchDemo(View view)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof (CloudSearchActivity));
            StartActivity(intent);
        }
    }
}