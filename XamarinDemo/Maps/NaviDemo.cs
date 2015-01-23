using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Baidu.Mapapi.Model;
using Com.Baidu.Mapapi.Navi;
using Java.Interop;
using Java.Lang;

namespace XamarinDemo.Maps
{
    /**
     * 在一个Activity中展示多个地图
     */

    [Activity(Label = "@string/demo_name_navi")]
    public class NaviDemo : Activity
    {
        //天安门坐标
        private double mLat1 = 39.915291;
        //百度大厦坐标
        private double mLat2 = 40.056858;
        private double mLon1 = 116.403857;
        private double mLon2 = 116.308194;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_navi_demo);
            var text = (TextView) FindViewById(Resource.Id.navi_info);
            text.Text = String.Format("起点:(%f,%f)\n终点:(%f,%f)", mLat1, mLon1,
                mLat2, mLon2);
        }

        /**
        * 开始导航		
        * @param view
        */

        [Export]
        public void StartNavi(View view)
        {
            var pt1 = new LatLng(mLat1, mLon1);
            var pt2 = new LatLng(mLat2, mLon2);
            // 构建 导航参数
            var para = new NaviPara();
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
                string classBaiduMapAppNotSupportNaviExceptionSimpleName =
                    typeof (BaiduMapAppNotSupportNaviException).Name;
                // string classIllegalNaviArgumentExceptionSimpleName = typeof(IllegalNaviArgumentException).Name;

                if (classBaiduMapAppNotSupportNaviExceptionSimpleName == currentExceptionSimpleName)
                {
                    e.PrintStackTrace();
                    var builder = new AlertDialog.Builder(this);
                    builder.SetMessage("您尚未安装百度地图app或app版本过低，点击确认安装？");
                    builder.SetTitle("提示");
                    // builder.SetPositiveButton("确认", delegate {});
                    // builder.SetPositiveButton("确认", delegate(object sender, DialogClickEventArgs args) {int which = args.Which;});
                    // builder.SetPositiveButton("确认", (sender, args) => {int which = args.Which;});
                    // builder.SetPositiveButton("确认", (object sender, DialogClickEventArgs args) => {int which = args.Which;});
                    builder.SetPositiveButton("确认", (sender, args) =>
                    {
                        var dialog = (IDialogInterface) sender;
                        // IDialogInterface dialog = Android.Runtime.Extensions.JavaCast<IDialogInterface>(sender);
                        dialog.Dismiss();
                        BaiduMapNavigation.GetLatestBaiduMapApp(this);
                    });

                    builder.SetNegativeButton("取消", (sender, args) =>
                    {
                        var dialog = (IDialogInterface) sender;
                        dialog.Dismiss();
                    });

                    builder.Create().Show();
                }
            }
        }

        [Export]
        public void StartWebNavi(View view)
        {
            var pt1 = new LatLng(mLat1, mLon1);
            var pt2 = new LatLng(mLat2, mLon2);
            // 构建 导航参数
            var para = new NaviPara();
            para.StartPoint = pt1;
            para.EndPoint = pt2;
            BaiduMapNavigation.OpenWebBaiduMapNavi(para, this);
        }
    }
}