using System;
using System.Collections.Generic;
using Android.Runtime;
using Object = Java.Lang.Object;

namespace Com.Baidu.Mapapi.Cloud
{
    public partial class CloudSearchResult : BaseSearchResult
    {
        private static IntPtr poiList_jfieldId;

        // Metadata.xml XPath field reference: path="/api/package[@name='com.baidu.mapapi.cloud']/class[@name='CloudSearchResult']/field[@name='poiList']"

        internal static IntPtr java_class_handle_2;

        [Register("poiList")]
        public IList<CloudPoiInfo> PoiList
        {
            get
            {
                if (poiList_jfieldId == IntPtr.Zero)
                    poiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "poiList", "Ljava/util/List;");
                IntPtr __ret = JNIEnv.GetObjectField(Handle, poiList_jfieldId);
                return JavaList<CloudPoiInfo>.FromJniHandle(__ret, JniHandleOwnership.TransferLocalRef);
            }
            set
            {
                if (poiList_jfieldId == IntPtr.Zero)
                    poiList_jfieldId = JNIEnv.GetFieldID(class_ref_2, "poiList", "Ljava/util/List;");
                IntPtr native_value = JavaList<CloudPoiInfo>.ToLocalJniHandle(value);
                JNIEnv.SetField(Handle, poiList_jfieldId, native_value);
                JNIEnv.DeleteLocalRef(native_value);
            }
        }

        internal static IntPtr class_ref_2
        {
            get { return JNIEnv.FindClass("com/baidu/mapapi/cloud/CloudSearchResult", ref java_class_handle_2); }
        }
    }

    public abstract partial class BaseSearchResult : Object
    {
    }

    public partial class CloudPoiInfo : Object
    {
    }
}