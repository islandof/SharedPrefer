
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
	[Activity (Label = "MainMonitor")]			
	public class MainMonitor : Activity
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
			new DemoInfo<Activity> (Resource.String.demo_title_basemap, Resource.String.demo_desc_basemap, new BaseMapDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_map_fragment, Resource.String.demo_desc_map_fragment, new MapFragmentDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_multimap, Resource.String.demo_desc_multimap, new MutiMapViewDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_layers, Resource.String.demo_desc_layers, new LayersDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_control, Resource.String.demo_desc_control, new MapControlDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_ui, Resource.String.demo_desc_ui, new UISettingDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_location, Resource.String.demo_desc_location, new LocationOverlayDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_geometry, Resource.String.demo_desc_geometry, new GeometryDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_overlay, Resource.String.demo_desc_overlay, new OverlayDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_offline, Resource.String.demo_desc_offline, new OfflineDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_poi, Resource.String.demo_desc_poi, new PoiSearchDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_geocode, Resource.String.demo_desc_geocode, new GeoCoderDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_route, Resource.String.demo_desc_route, new RoutePlanDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_bus, Resource.String.demo_desc_bus, new BusLineSearchDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_share, Resource.String.demo_desc_share, new ShareDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_cloud, Resource.String.demo_desc_cloud, new CloudSearchDemo ()),
			new DemoInfo<Activity> (Resource.String.demo_title_navi, Resource.String.demo_desc_navi, new NaviDemo ())
		};

		protected override void OnResume ()
		{
			base.OnResume ();
		}

		private class DemoListAdapter : BaseAdapter
		{
			MainMonitor bMapApiDemoMain;

			public DemoListAdapter (MainMonitor bMapApiDemoMain)
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

