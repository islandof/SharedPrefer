using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace XamarinDemo
{
    [Activity(Label = "MainMonitor")]
    public class MainMonitor : Activity
    {
        private static readonly DemoInfo<Activity>[] demos =
        {
            new DemoInfo<Activity>(Resource.String.monitor_name_ddrivel, Resource.String.monitor_desc_ddrivel,
                new DdriveLActivity()),
            new DemoInfo<Activity>(Resource.String.monitor_name_zhalanalarml, Resource.String.monitor_desc_zhalanalarml,
                new ZhalanAlarmLActivity()),
            new DemoInfo<Activity>(Resource.String.monitor_name_carposition, Resource.String.monitor_desc_carposition,
                new Carposition()),
            new DemoInfo<Activity>(Resource.String.monitor_name_siji, Resource.String.monitor_desc_siji, new SijiList()),
            new DemoInfo<Activity>(Resource.String.monitor_name_cartracelist, Resource.String.monitor_desc_cartracelist,
                new CartraceList())
        };

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainMonitor);
            var text = FindViewById<TextView>(Resource.Id.text_Info);
            text.SetTextColor(Color.Yellow);
            text.Text = "监控模块";
            var mListView = FindViewById<ListView>(Resource.Id.listView);

            //mListView.Adapter = new DemoListAdapter (this);
            mListView.Adapter = new DemoListAdapter(this);
            mListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                int index = e.Position;
                OnListItemClick(index);
            };
            // Create your application here
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

        private class DemoInfo<T> : Object where T : Activity
        {
            public readonly T demoClass;
            public readonly int desc;
            public readonly int image;
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
            private readonly MainMonitor bMapApiDemoMain;

            public DemoListAdapter(MainMonitor bMapApiDemoMain)
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
    }
}