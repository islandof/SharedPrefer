using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedPreferences
{
    class user_info
    {
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_NAME_EN { get; set; }
        public string USER_PWD { get; set; }
        public Nullable<int> DEPT_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public Nullable<int> ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public System.DateTime CREATE_DATE { get; set; }
        public string USER_EMAIL { get; set; }
        public Nullable<int> companyid { get; set; }
        public string companyname { get; set; }
        
    }
}
