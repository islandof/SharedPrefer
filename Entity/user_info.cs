using System;

namespace Entity
{
    public class user_info
    {
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_NAME_EN { get; set; }
        public string USER_PWD { get; set; }
        public int? DEPT_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public int? ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string USER_EMAIL { get; set; }
        public int? companyid { get; set; }
        public string companyname { get; set; }
    }
}