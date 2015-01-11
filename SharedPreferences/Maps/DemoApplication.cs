using Android.App;
using Android.Runtime;
using Com.Baidu.Mapapi;
using System;

namespace SharedPreferences
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
            // ��ʹ�� SDK �����֮ǰ��ʼ�� context ��Ϣ������ ApplicationContext
            SDKInitializer.Initialize(this);
        }
    }
}