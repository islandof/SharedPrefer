using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace XamarinDemo
{
    [Activity(Label = "汽车管理模块")]
    public class CarManage : Activity
    {
        private static readonly DemoInfo<Activity>[] demos =
        {
            new DemoInfo<Activity>(Resource.String.monitor_name_qiche, Resource.String.monitor_desc_qiche,
                new QicheList()),
            new DemoInfo<Activity>(Resource.String.monitor_name_zhalanarea, Resource.String.monitor_desc_zhalanarea,
                new ZhalanareaList()),
            new DemoInfo<Activity>(Resource.String.monitor_name_zhalantime, Resource.String.monitor_desc_zhalantime,
                new ZhalantimeList()),
            new DemoInfo<Activity>(Resource.String.monitor_name_zhalanchaosu, Resource.String.monitor_desc_zhalanchaosu,
                new ZhalanchaosuList()),
            new DemoInfo<Activity>(Resource.String.monitor_name_zhalandaisu, Resource.String.monitor_desc_zhalandaisu,
                new ZhalandaisuList())
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
            private readonly CarManage bMapApiDemoMain;

            public DemoListAdapter(CarManage bMapApiDemoMain)
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