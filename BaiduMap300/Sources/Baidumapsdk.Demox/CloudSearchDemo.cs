using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;

namespace Baidumapsdk.Demox
{
    [Activity(Label = "@string/title_activity_cloud_search_demo")]
    public class CloudSearchDemo : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cloud_search_demo);
        }

        [Java.Interop.Export]
        public void StartCloudSearchDemo(View view)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(CloudSearchActivity));
            StartActivity(intent);
        }
    }
}