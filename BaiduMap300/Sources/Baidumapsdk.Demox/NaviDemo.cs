using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Model;
using Com.Baidu.Mapapi.Navi;
using Java.Lang;

namespace Baidumapsdk.Demox
{
    /**
     * ��һ��Activity��չʾ�����ͼ
     */
    [Activity(Label = "@string/demo_name_navi")]
    public class NaviDemo : Activity
    {
        //�찲������
        double mLat1 = 39.915291;
        double mLon1 = 116.403857;
        //�ٶȴ�������
        double mLat2 = 40.056858;
        double mLon2 = 116.308194;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_navi_demo);
            TextView text = (TextView)FindViewById(Resource.Id.navi_info);
            text.Text = String.Format("���:(%f,%f)\n�յ�:(%f,%f)", mLat1, mLon1,
                    mLat2, mLon2);
        }

        /**
        * ��ʼ����		
        * @param view
        */
        [Java.Interop.Export]
        public void StartNavi(View view)
        {
            LatLng pt1 = new LatLng(mLat1, mLon1);
            LatLng pt2 = new LatLng(mLat2, mLon2);
            // ���� ��������
            NaviPara para = new NaviPara();
            para.StartPoint = pt1;
            para.StartName = "�����￪ʼ";
            para.EndPoint = pt2;
            para.EndName = "���������";

            try
            {

                BaiduMapNavigation.OpenBaiduMapNavi(para, this);

            }
            catch (BaiduMapAppNotSupportNaviException e)
            {
                e.PrintStackTrace();

                // ��Ȼ����ԭ������, ������IKVM��bug, Ҳ����������������, ��, �����ָ·
                // ������� RuntimeException + if/else �ǶԵ���, �ӵ�
            }
            catch (RuntimeException e)
            {
                // (imknown ע)����ԭ�������������� : e.Message

                // Android.Runtime.Extensions.JavaCast<BaiduMapAppNotSupportNaviException>(e);
                // Android.Runtime.Extensions.JavaCast<IllegalNaviArgumentException>(e);

                string currentExceptionSimpleName = e.Class.SimpleName;
                string classBaiduMapAppNotSupportNaviExceptionSimpleName = typeof(BaiduMapAppNotSupportNaviException).Name;
                // string classIllegalNaviArgumentExceptionSimpleName = typeof(IllegalNaviArgumentException).Name;

                if (classBaiduMapAppNotSupportNaviExceptionSimpleName == currentExceptionSimpleName)
                {
                    e.PrintStackTrace();
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetMessage("����δ��װ�ٶȵ�ͼapp��app�汾���ͣ����ȷ�ϰ�װ��");
                    builder.SetTitle("��ʾ");
                    // builder.SetPositiveButton("ȷ��", delegate {});
                    // builder.SetPositiveButton("ȷ��", delegate(object sender, DialogClickEventArgs args) {int which = args.Which;});
                    // builder.SetPositiveButton("ȷ��", (sender, args) => {int which = args.Which;});
                    // builder.SetPositiveButton("ȷ��", (object sender, DialogClickEventArgs args) => {int which = args.Which;});
                    builder.SetPositiveButton("ȷ��", (sender, args) =>
                    {
                        IDialogInterface dialog = (IDialogInterface)sender;
                        // IDialogInterface dialog = Android.Runtime.Extensions.JavaCast<IDialogInterface>(sender);
                        dialog.Dismiss();
                        BaiduMapNavigation.GetLatestBaiduMapApp(this);
                    });

                    builder.SetNegativeButton("ȡ��", (sender, args) =>
                    {
                        IDialogInterface dialog = (IDialogInterface)sender;
                        dialog.Dismiss();
                    });

                    builder.Create().Show();
                }
            }
        }

        [Java.Interop.Export]
        public void StartWebNavi(View view)
        {
            LatLng pt1 = new LatLng(mLat1, mLon1);
            LatLng pt2 = new LatLng(mLat2, mLon2);
            // ���� ��������
            NaviPara para = new NaviPara();
            para.StartPoint = pt1;
            para.EndPoint = pt2;
            BaiduMapNavigation.OpenWebBaiduMapNavi(para, this);
        }
    }
}