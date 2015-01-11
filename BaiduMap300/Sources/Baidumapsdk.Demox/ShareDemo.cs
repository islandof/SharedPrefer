using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;

namespace Baidumapsdk.Demox
{
    [Activity(Label = "@string/demo_name_share")]
    public class ShareDemo : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_share_demo);
        }

       [Java.Interop.Export]
        public void StartShareDemo(View view)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(ShareDemoActivity));
            StartActivity(intent);

        }

    }
}