using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Common
{
    /// <summary>
    /// Json帮助类(Json与对象之前相互转换)
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// Json字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 对象转换为json字符串 (序列化时间格式,默认("yyyy-MM-dd HH:mm:ss"))
        /// </summary>
        /// <param name="t"></param>
        /// <param name="timeFormat">序列化时间格式,默认("yyyy-MM-dd HH:mm:ss")</param>
        /// <returns></returns>
        public static string ObjectToJson(object t, string timeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            IsoDateTimeConverter tFormat = new IsoDateTimeConverter();
            tFormat.DateTimeFormat = timeFormat;
            return Newtonsoft.Json.JsonConvert.SerializeObject(t, Newtonsoft.Json.Formatting.Indented, tFormat);
        }
        public static string SerializeObjectExceptNull(object o)
        {
            var json = JsonConvert.SerializeObject(o, new JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
            });
            return json;
        }
    }
}
