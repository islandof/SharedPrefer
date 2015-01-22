
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
using XamarinDemo.Maps;

namespace XamarinDemo
{
    [Activity(Label = "汽车管理模块")]			
	public class CarManage: Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.MainMonitor);
			TextView text = FindViewById<TextView> (Resource.Id.text_Info);
			text.SetTextColor (Color.Yellow);
			text.Text = "监控模块";
			ListView mListView = FindViewById<ListView> (Resource.Id.listView);

			//mListView.Adapter = new DemoListAdapter (this);
			mListView.Adapter = new DemoListAdapter (this);
			mListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				int index = e.Position;
				OnListItemClick (index);

			};
			// Create your application here
		}


		void OnListItemClick (int index)
		{
			Intent intent = null;
			intent = new Intent (this, demos [index].demoClass.GetType ());
			this.StartActivity (intent);
		}

		private static readonly DemoInfo<Activity>[] demos = {
			new DemoInfo<Activity> (Resource.String.monitor_name_qiche, Resource.String.monitor_desc_qiche, new QicheList()),
            new DemoInfo<Activity> (Resource.String.monitor_name_zhalanarea, Resource.String.monitor_desc_zhalanarea, new ZhalanareaList()),            
            new DemoInfo<Activity> (Resource.String.monitor_name_zhalantime, Resource.String.monitor_desc_zhalantime, new ZhalantimeList()),            
            new DemoInfo<Activity> (Resource.String.monitor_name_zhalanchaosu, Resource.String.monitor_desc_zhalanchaosu, new ZhalanchaosuList()),            
            new DemoInfo<Activity> (Resource.String.monitor_name_zhalandaisu, Resource.String.monitor_desc_zhalandaisu, new ZhalandaisuList())
		};

		protected override void OnResume ()
		{
			base.OnResume ();
		}

		private class DemoListAdapter : BaseAdapter
		{
			CarManage bMapApiDemoMain;

			public DemoListAdapter (CarManage bMapApiDemoMain)
				: base ()
			{
				this.bMapApiDemoMain = bMapApiDemoMain;
			}

			public override View GetView (int index, View convertView, ViewGroup parent)
			{
				convertView = View.Inflate (bMapApiDemoMain,
					Resource.Layout.demo_info_item, null);
				TextView title = convertView.FindViewById<TextView> (Resource.Id.title);
				TextView desc = convertView.FindViewById<TextView> (Resource.Id.desc);
				title.SetText (demos [index].title);
				desc.SetText (demos [index].desc);
				return convertView;
			}

			public override int Count {
				get { return demos.Length; }
			}

			public override Java.Lang.Object GetItem (int index)
			{
				return demos [index];
			}

			public override long GetItemId (int id)
			{
				return id;
			}
		}

		private class DemoInfo<T> : Java.Lang.Object where T : Activity
		{
			public readonly int title;
			public readonly int image;
			public readonly int desc;
			public readonly T demoClass;

			public DemoInfo (int title, int desc, T demoClass)
			{
				this.title = title;
				this.desc = desc;
				this.demoClass = demoClass;
			}
		}
	}
}

