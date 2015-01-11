using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Map.Offline;
using Java.Lang;
using System.Collections.Generic;

namespace Baidumapsdk.Demox
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, Label = "@string/demo_name_offline", ScreenOrientation = ScreenOrientation.Sensor)]
    public class OfflineDemo : Activity, IMKOfflineMapListener
    {

        private MKOfflineMap mOffline = null;
        private TextView cidView;
        private TextView stateView;
        private EditText cityNameView;
        /**
         * �����ص����ߵ�ͼ��Ϣ�б�
         */
        private IList<MKOLUpdateElement> localMapList = null;
        private LocalMapAdapter lAdapter = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_offline);
            mOffline = new MKOfflineMap();
            mOffline.Init(this);
            InitView();

        }

        private void InitView()
        {

            cidView = FindViewById<TextView>(Resource.Id.cityid);
            cityNameView = FindViewById<EditText>(Resource.Id.city);
            stateView = FindViewById<TextView>(Resource.Id.state);

            ListView hotCityList = FindViewById<ListView>(Resource.Id.hotcitylist);
            IList<string> hotCities = new List<string>();
            //��ȡ���ֳ����б�
            IList<MKOLSearchRecord> records1 = mOffline.HotCityList;
            if (records1 != null)
            {
                foreach (MKOLSearchRecord r in records1)
                {
                    hotCities.Add(r.CityName + "(" + r.CityID + ")" + "   --"
                           + this.FormatDataSize(r.Size));
                }
            }
            IListAdapter hAdapter = (IListAdapter)new ArrayAdapter<string>(this,
                     Android.Resource.Layout.SimpleListItem1, hotCities);
            hotCityList.Adapter = hAdapter;

            ListView allCityList = FindViewById<ListView>(Resource.Id.allcitylist);
            //��ȡ����֧�����ߵ�ͼ�ĳ���
            IList<string> allCities = new List<string>();
            IList<MKOLSearchRecord> records2 = mOffline.OfflineCityList;
            if (records1 != null)
            {
                foreach (MKOLSearchRecord r in records2)
                {
                    allCities.Add(r.CityName + "(" + r.CityID + ")" + "   --"
                            + this.FormatDataSize(r.Size));
                }
            }
            IListAdapter aAdapter = (IListAdapter)new ArrayAdapter<string>(this,
                     Android.Resource.Layout.SimpleListItem1, allCities);
            allCityList.Adapter = aAdapter;

            LinearLayout cl = FindViewById<LinearLayout>(Resource.Id.citylist_layout);
            LinearLayout lm = FindViewById<LinearLayout>(Resource.Id.localmap_layout);
            lm.Visibility = ViewStates.Gone;
            cl.Visibility = ViewStates.Visible;

            //��ȡ���¹������ߵ�ͼ��Ϣ
            localMapList = mOffline.AllUpdateInfo;
            if (localMapList == null)
            {
                localMapList = new List<MKOLUpdateElement>();
            }

            ListView localMapListView = FindViewById<ListView>(Resource.Id.localmaplist);
            lAdapter = new LocalMapAdapter(this);
            localMapListView.Adapter = lAdapter;

        }

        /**
         * �л��������б�
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void ClickCityListButton(View view)
        {
            LinearLayout cl = FindViewById<LinearLayout>(Resource.Id.citylist_layout);
            LinearLayout lm = FindViewById<LinearLayout>(Resource.Id.localmap_layout);
            lm.Visibility = ViewStates.Gone;
            cl.Visibility = ViewStates.Visible;

        }

        /**
         * �л������ع����б�
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void ClickLocalMapListButton(View view)
        {
            LinearLayout cl = FindViewById<LinearLayout>(Resource.Id.citylist_layout);
            LinearLayout lm = FindViewById<LinearLayout>(Resource.Id.localmap_layout);
            lm.Visibility = ViewStates.Visible;
            cl.Visibility = ViewStates.Gone;
        }

        /**
         * ������������
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void Search(View view)
        {
            IList<MKOLSearchRecord> records = mOffline.SearchCity(cityNameView.Text);
            if (records == null || records.Count != 1)
                return;
            cidView.Text = String.ValueOf(records[0].CityID);
        }

        /**
         * ��ʼ����
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void Start(View view)
        {
            int cityid = Integer.ParseInt(cidView.Text);
            mOffline.Start(cityid);
            ClickLocalMapListButton(null);
            Toast.MakeText(this, "��ʼ�������ߵ�ͼ. cityid: " + cityid, ToastLength.Short)
                   .Show();
        }

        /**
         * ��ͣ����
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void Stop(View view)
        {
            int cityid = Integer.ParseInt(cidView.Text);
            mOffline.Pause(cityid);
            Toast.MakeText(this, "��ͣ�������ߵ�ͼ. cityid: " + cityid, ToastLength.Short)
                   .Show();
        }

        /**
         * ɾ�����ߵ�ͼ
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void Remove(View view)
        {
            int cityid = Integer.ParseInt(cidView.Text);
            mOffline.Remove(cityid);
            Toast.MakeText(this, "ɾ�����ߵ�ͼ. cityid: " + cityid, ToastLength.Short)
                      .Show();
        }

        /**
         * ��SD���������ߵ�ͼ��װ��
         * 
         * @param view
         */
        [Java.Interop.Export]
        public void ImportFromSDCard(View view)
        {
            int num = mOffline.ImportOfflineData();
            string msg = "";
            if (num == 0)
            {
                msg = "û�е������߰�������������߰�����λ�ò���ȷ�������߰��Ѿ������";
            }
            else
            {
                msg = String.Format("�ɹ����� %d �����߰������������ع���鿴", num);
                UpdateView();
            }
            Toast.MakeText(this, msg, ToastLength.Short).Show();
        }

        /**
         * ����״̬��ʾ 
         */
        public void UpdateView()
        {
            localMapList = mOffline.AllUpdateInfo;
            if (localMapList == null)
            {
                localMapList = new List<MKOLUpdateElement>();
            }
            lAdapter.NotifyDataSetChanged();
        }

        protected override void OnPause()
        {
            int cityid = Integer.ParseInt(cidView.Text);
            mOffline.Pause(cityid);
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        public string FormatDataSize(int size)
        {
            string ret = "";
            if (size < (1024 * 1024))
            {
                ret = String.Format("%dK", size / 1024);
            }
            else
            {
                ret = String.Format("%.1fM", size / (1024 * 1024.0));
            }
            return ret;
        }

        protected override void OnDestroy()
        {
            /**
             * �˳�ʱ���������ߵ�ͼģ��
             */
            mOffline.Destroy();
            base.OnDestroy();
        }

        public void OnGetOfflineMapState(int type, int state)
        {
            switch (type)
            {
                case MKOfflineMap.TypeDownloadUpdate:
                    {
                        MKOLUpdateElement update = mOffline.GetUpdateInfo(state);
                        //�������ؽ��ȸ�����ʾ
                        if (update != null)
                        {
                            stateView.Text = String.Format("%s : %d%%", update.CityName,
                                update.Ratio);
                            UpdateView();
                        }
                    }
                    break;
                case MKOfflineMap.TypeNewOffline:
                    //�������ߵ�ͼ��װ
                    Log.Debug("OfflineDemo", String.Format("add offlinemap num:%d", state));
                    break;
                case MKOfflineMap.TypeVerUpdate:
                    // �汾������ʾ
                    //	MKOLUpdateElement e = mOffline.GetUpdateInfo(state);

                    break;
            }

        }

        /**
         * ���ߵ�ͼ�����б�������
         */
        public class LocalMapAdapter : BaseAdapter
        {
            private OfflineDemo offlineDemo;

            public LocalMapAdapter(OfflineDemo offlineDemo)
            {
                this.offlineDemo = offlineDemo;
            }

            public override int Count
            {
                get { return offlineDemo.localMapList.Count; }
            }

            public override Object GetItem(int index)
            {
                return offlineDemo.localMapList[index];
            }

            public override long GetItemId(int index)
            {
                return index;
            }

            public override View GetView(int index, View view, ViewGroup arg2)
            {
                MKOLUpdateElement e = (MKOLUpdateElement)GetItem(index);
                view = View.Inflate(offlineDemo,
                        Resource.Layout.offline_localmap_list, null);
                InitViewItem(view, e);
                return view;
            }

            void InitViewItem(View view, MKOLUpdateElement e)
            {
                Button display = view.FindViewById<Button>(Resource.Id.display);
                Button remove = view.FindViewById<Button>(Resource.Id.remove);
                TextView title = view.FindViewById<TextView>(Resource.Id.title);
                TextView update = view.FindViewById<TextView>(Resource.Id.update);
                TextView ratio = view.FindViewById<TextView>(Resource.Id.ratio);
                ratio.Text = e.Ratio + "%";
                title.Text = e.CityName;
                if (e.Update)
                {
                    update.Text = "�ɸ���";
                }
                else
                {
                    update.Text = "����";
                }
                if (e.Ratio != 100)
                {
                    display.Enabled = false;
                }
                else
                {
                    display.Enabled = true;
                }
                remove.Click += delegate
                {
                    offlineDemo.mOffline.Remove(e.CityID);
                    offlineDemo.UpdateView();
                };
                display.Click += delegate
                {
                    Intent intent = new Intent();
                    intent.PutExtra("x", e.GeoPt.Longitude);
                    intent.PutExtra("y", e.GeoPt.Latitude);
                    intent.SetClass(offlineDemo, typeof(BaseMapDemo));
                    offlineDemo.StartActivity(intent);
                };
            }

        }

    }
}