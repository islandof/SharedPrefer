using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi;
using Java.Lang;

namespace XamarinDemo.Maps
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, Label = "MapsDemo",
        ScreenOrientation = ScreenOrientation.Sensor)]
    public class BMapApiDemoMain : Activity
    {
        private static readonly string LTAG = typeof (BMapApiDemoMain).Name;

        private static readonly DemoInfo<Activity>[] demos =
        {
            new DemoInfo<Activity>(Resource.String.demo_title_basemap, Resource.String.demo_desc_basemap,
                new BaseMapDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_map_fragment, Resource.String.demo_desc_map_fragment,
                new MapFragmentDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_multimap, Resource.String.demo_desc_multimap,
                new MutiMapViewDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_layers, Resource.String.demo_desc_layers, new LayersDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_control, Resource.String.demo_desc_control,
                new MapControlDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_ui, Resource.String.demo_desc_ui, new UISettingDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_location, Resource.String.demo_desc_location,
                new LocationOverlayDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_geometry, Resource.String.demo_desc_geometry,
                new GeometryDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_overlay, Resource.String.demo_desc_overlay,
                new OverlayDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_offline, Resource.String.demo_desc_offline,
                new OfflineDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_poi, Resource.String.demo_desc_poi, new PoiSearchDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_geocode, Resource.String.demo_desc_geocode,
                new GeoCoderDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_route, Resource.String.demo_desc_route,
                new RoutePlanDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_bus, Resource.String.demo_desc_bus,
                new BusLineSearchDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_share, Resource.String.demo_desc_share, new ShareDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_cloud, Resource.String.demo_desc_cloud,
                new CloudSearchDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_navi, Resource.String.demo_desc_navi, new NaviDemo())
        };

        // new BMapApiDemoMain().Class.SimpleName;

        /**
         * 构造广播监听类，监听 SDK key 验证以及网络异常广播
         */

        private SDKReceiver mReceiver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Mapsmain);
            var text = FindViewById<TextView>(Resource.Id.text_Info);
            text.SetTextColor(Color.Yellow);
            text.Text = "欢迎使用百度地图Android SDK v" + VersionInfo.ApiVersion;
            var mListView = FindViewById<ListView>(Resource.Id.listView);
            // 添加ListItem，设置事件响应
            mListView.Adapter = new DemoListAdapter(this);

            // mListView.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args) { };
            mListView.ItemClick += (sender, args) =>
            {
                int index = args.Position;
                OnListItemClick(index);
            };

            // 注册 SDK 广播监听者
            var iFilter = new IntentFilter();
            //iFilter.AddAction(SDKInitializer.SdkBroadtcastActionStringPermissionCheckError);
            iFilter.AddAction(SDKInitializer.SdkBroadcastActionStringNetworkError);
            mReceiver = new SDKReceiver(this);
            RegisterReceiver(mReceiver, iFilter);
        }

        private void OnListItemClick(int index)
        {
            Intent intent = null;
            intent = new Intent(this, demos[index].demoClass.GetType());
            StartActivity(intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            // 取消监听 SDK 广播
            UnregisterReceiver(mReceiver);
        }

        private class DemoInfo<T> : Object where T : Activity
        {
            public readonly T demoClass;
            public readonly int desc;
            public readonly int title;

            public DemoInfo(int title, int desc, T demoClass)
            {
                this.title = title;
                this.desc = desc;
                this.demoClass = demoClass;
            }
        }

        private class DemoListAdapter : BaseAdapter
        {
            private readonly BMapApiDemoMain bMapApiDemoMain;

            public DemoListAdapter(BMapApiDemoMain bMapApiDemoMain)
            {
                this.bMapApiDemoMain = bMapApiDemoMain;
            }

            public override int Count
            {
                get { return demos.Length; }
            }

            public override View GetView(int index, View convertView, ViewGroup parent)
            {
                convertView = View.Inflate(bMapApiDemoMain,
                    Resource.Layout.demo_info_item, null);
                var title = convertView.FindViewById<TextView>(Resource.Id.title);
                var desc = convertView.FindViewById<TextView>(Resource.Id.desc);
                title.SetText(demos[index].title);
                desc.SetText(demos[index].desc);
                return convertView;
            }

            public override Object GetItem(int index)
            {
                return demos[index];
            }

            public override long GetItemId(int id)
            {
                return id;
            }
        }

        [BroadcastReceiver]
        public class SDKReceiver : BroadcastReceiver
        {
            private readonly BMapApiDemoMain bMapApiDemoMain;

            public SDKReceiver()
            {
            }

            public SDKReceiver(BMapApiDemoMain bMapApiDemoMain)
            {
                this.bMapApiDemoMain = bMapApiDemoMain;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                string s = intent.Action;
                Log.Debug(LTAG, "action: " + s);
                var text = bMapApiDemoMain.FindViewById<TextView>(Resource.Id.text_Info);
                text.SetTextColor(Color.Red);
                if (s.Equals(SDKInitializer.SdkBroadtcastActionStringPermissionCheckError))
                {
                    text.Text = "key 验证出错! 请在 AndroidManifest.xml 文件中检查 key 设置";
                }
                else if (s
                    .Equals(SDKInitializer.SdkBroadcastActionStringNetworkError))
                {
                    text.Text = "网络出错";
                }
            }
        }
    }
}