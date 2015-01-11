using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi;

namespace Baidumapsdk.Demox
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, Label = "BaiduMapSDKDemo", MainLauncher = true, ScreenOrientation = ScreenOrientation.Sensor)]
    public class BMapApiDemoMain : Activity
    {
        private static readonly string LTAG = typeof(BMapApiDemoMain).Name; // new BMapApiDemoMain().Class.SimpleName;

        /**
         * 构造广播监听类，监听 SDK key 验证以及网络异常广播
         */
        [BroadcastReceiver]
        public class SDKReceiver : BroadcastReceiver
        {
            BMapApiDemoMain bMapApiDemoMain;

            public SDKReceiver()
            {
            }

            public SDKReceiver(BMapApiDemoMain bMapApiDemoMain)
                : base()
            {
                this.bMapApiDemoMain = bMapApiDemoMain;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                string s = intent.Action;
                Log.Debug(LTAG, "action: " + s);
                TextView text = bMapApiDemoMain.FindViewById<TextView>(Resource.Id.text_Info);
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

        private SDKReceiver mReceiver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);
            TextView text = FindViewById<TextView>(Resource.Id.text_Info);
            text.SetTextColor(Color.Yellow);
            text.Text = "欢迎使用百度地图Android SDK v" + VersionInfo.ApiVersion;
            ListView mListView = FindViewById<ListView>(Resource.Id.listView);
            // 添加ListItem，设置事件响应
            mListView.Adapter = new DemoListAdapter(this);

            // mListView.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args) { };
            mListView.ItemClick += (sender, args) =>
            {
                int index = args.Position;
                OnListItemClick(index);
            };

            // 注册 SDK 广播监听者
            IntentFilter iFilter = new IntentFilter();
            iFilter.AddAction(SDKInitializer.SdkBroadtcastActionStringPermissionCheckError);
            iFilter.AddAction(SDKInitializer.SdkBroadcastActionStringNetworkError);
            mReceiver = new SDKReceiver(this);
            RegisterReceiver(mReceiver, iFilter);
        }

        void OnListItemClick(int index)
        {
            Intent intent = null;
            intent = new Intent(this, demos[index].demoClass.GetType());
            this.StartActivity(intent);
        }

        private static readonly DemoInfo<Activity>[] demos =
        {
            new DemoInfo<Activity>(Resource.String.demo_title_basemap,Resource.String.demo_desc_basemap, new BaseMapDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_map_fragment,Resource.String.demo_desc_map_fragment, new MapFragmentDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_multimap, Resource.String.demo_desc_multimap, new MutiMapViewDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_layers, Resource.String.demo_desc_layers, new LayersDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_control, Resource.String.demo_desc_control, new MapControlDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_ui, Resource.String.demo_desc_ui, new UISettingDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_location, Resource.String.demo_desc_location, new LocationOverlayDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_geometry, Resource.String.demo_desc_geometry,new GeometryDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_overlay, Resource.String.demo_desc_overlay,new OverlayDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_offline, Resource.String.demo_desc_offline,new OfflineDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_poi, Resource.String.demo_desc_poi,new PoiSearchDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_geocode, Resource.String.demo_desc_geocode,new GeoCoderDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_route, Resource.String.demo_desc_route,new RoutePlanDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_bus, Resource.String.demo_desc_bus,new BusLineSearchDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_share, Resource.String.demo_desc_share,new ShareDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_cloud, Resource.String.demo_desc_cloud,new CloudSearchDemo()),
            new DemoInfo<Activity>(Resource.String.demo_title_navi, Resource.String.demo_desc_navi,new NaviDemo())
	    };

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

        private class DemoListAdapter : BaseAdapter
        {
            BMapApiDemoMain bMapApiDemoMain;

            public DemoListAdapter(BMapApiDemoMain bMapApiDemoMain)
                : base()
            {
                this.bMapApiDemoMain = bMapApiDemoMain;
            }

            public override View GetView(int index, View convertView, ViewGroup parent)
            {
                convertView = View.Inflate(bMapApiDemoMain,
                        Resource.Layout.demo_info_item, null);
                TextView title = convertView.FindViewById<TextView>(Resource.Id.title);
                TextView desc = convertView.FindViewById<TextView>(Resource.Id.desc);
                title.SetText(demos[index].title);
                desc.SetText(demos[index].desc);
                return convertView;
            }

            public override int Count
            {
                get { return demos.Length; }
            }

            public override Java.Lang.Object GetItem(int index)
            {
                return demos[index];
            }

            public override long GetItemId(int id)
            {
                return id;
            }
        }

        private class DemoInfo<T> : Java.Lang.Object where T : Activity
        {
            public readonly int title;
            public readonly int desc;
            public readonly T demoClass;

            public DemoInfo(int title, int desc, T demoClass)
            {
                this.title = title;
                this.desc = desc;
                this.demoClass = demoClass;
            }
        }
    }
}