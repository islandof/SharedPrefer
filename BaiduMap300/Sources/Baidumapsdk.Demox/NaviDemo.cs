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
     * 在一个Activity中展示多个地图
     */
    [Activity(Label = "@string/demo_name_navi")]
    public class NaviDemo : Activity
    {
        //天安门坐标
        double mLat1 = 39.915291;
        double mLon1 = 116.403857;
        //百度大厦坐标
        double mLat2 = 40.056858;
        double mLon2 = 116.308194;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_navi_demo);
            TextView text = (TextView)FindViewById(Resource.Id.navi_info);
            text.Text = String.Format("起点:(%f,%f)\n终点:(%f,%f)", mLat1, mLon1,
                    mLat2, mLon2);
        }

        /**
        * 开始导航		
        * @param view
        */
        [Java.Interop.Export]
        public void StartNavi(View view)
        {
            LatLng pt1 = new LatLng(mLat1, mLon1);
            LatLng pt2 = new LatLng(mLat2, mLon2);
            // 构建 导航参数
            NaviPara para = new NaviPara();
            para.StartPoint = pt1;
            para.StartName = "从这里开始";
            para.EndPoint = pt2;
            para.EndName = "到这里结束";

            try
            {

                BaiduMapNavigation.OpenBaiduMapNavi(para, this);

            }
            catch (BaiduMapAppNotSupportNaviException e)
            {
                e.PrintStackTrace();

                // 居然不走原版的这个, 可能是IKVM的bug, 也可能是我能力不够, 哎, 求大神指路
                // 用下面的 RuntimeException + if/else 是对的了, 坑爹
            }
            catch (RuntimeException e)
            {
                // (imknown 注)具体原因可以用这个属性 : e.Message

                // Android.Runtime.Extensions.JavaCast<BaiduMapAppNotSupportNaviException>(e);
                // Android.Runtime.Extensions.JavaCast<IllegalNaviArgumentException>(e);

                string currentExceptionSimpleName = e.Class.SimpleName;
                string classBaiduMapAppNotSupportNaviExceptionSimpleName = typeof(BaiduMapAppNotSupportNaviException).Name;
                // string classIllegalNaviArgumentExceptionSimpleName = typeof(IllegalNaviArgumentException).Name;

                if (classBaiduMapAppNotSupportNaviExceptionSimpleName == currentExceptionSimpleName)
                {
                    e.PrintStackTrace();
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetMessage("您尚未安装百度地图app或app版本过低，点击确认安装？");
                    builder.SetTitle("提示");
                    // builder.SetPositiveButton("确认", delegate {});
                    // builder.SetPositiveButton("确认", delegate(object sender, DialogClickEventArgs args) {int which = args.Which;});
                    // builder.SetPositiveButton("确认", (sender, args) => {int which = args.Which;});
                    // builder.SetPositiveButton("确认", (object sender, DialogClickEventArgs args) => {int which = args.Which;});
                    builder.SetPositiveButton("确认", (sender, args) =>
                    {
                        IDialogInterface dialog = (IDialogInterface)sender;
                        // IDialogInterface dialog = Android.Runtime.Extensions.JavaCast<IDialogInterface>(sender);
                        dialog.Dismiss();
                        BaiduMapNavigation.GetLatestBaiduMapApp(this);
                    });

                    builder.SetNegativeButton("取消", (sender, args) =>
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
            // 构建 导航参数
            NaviPara para = new NaviPara();
            para.StartPoint = pt1;
            para.EndPoint = pt2;
            BaiduMapNavigation.OpenWebBaiduMapNavi(para, this);
        }
    }
}