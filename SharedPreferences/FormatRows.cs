using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SharedPreferences
{
    class FormatRows
    {
        public object rows { get; set; }
        public object Data { get; set; }
        public int total { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(
                this, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, });
        }
    }
}
