using Newtonsoft.Json;

namespace XamarinDemo
{
    internal class FormatRows
    {
        public object rows { get; set; }
        public object Data { get; set; }
        public int total { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(
                this, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore,});
        }
    }
}