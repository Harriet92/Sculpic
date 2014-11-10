using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Sources.DatabaseServer.JsonFx
{
    public class DateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type t)
        {
            return t == typeof(DateTime);
        }

        public override Dictionary<string, object> WriteJson(Type type, object value)
        {
            DateTime v = (DateTime)value;
            Dictionary<string, object> dict = new Dictionary<string, object>();

            DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeSpan elapsedTime = new TimeSpan(v.ToUniversalTime().Ticks - Epoch.Ticks);
            long timestamp = (long)elapsedTime.TotalMilliseconds;

            dict.Add("value", "/Date(" + timestamp + ")/");
            return dict;
        }

        public override object ReadJson(Type type, Dictionary<string, object> value)
        {
            System.Text.RegularExpressions.Regex regExp = new System.Text.RegularExpressions.Regex(@"[^\d]");
            string data = regExp.Replace(value["value"].ToString(), "");
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime dotnetTime;
            try
            {
                dotnetTime = unixEpoch.AddMilliseconds(Int64.Parse(data));
            }
            catch (ArgumentOutOfRangeException)
            {
                return unixEpoch;
            }
            return dotnetTime;
        }
    }
}
