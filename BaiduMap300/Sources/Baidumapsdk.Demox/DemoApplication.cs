using Android.App;
using Android.Runtime;
using Com.Baidu.Mapapi;
using System;

namespace Baidumapsdk.Demox
{
    [Application]
    public class DemoApplication : Application
    {
        public DemoApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            // 在使用 SDK 各组间之前初始化 context 信息，传入 ApplicationContext
            SDKInitializer.Initialize(this);
        }
    }
}