using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SANS.Common
{
    public static class Util
    {
        #region === 由String取值 ===

        public static int ParseInt(string strValue)
        {
            int res;
            int.TryParse(strValue, out res);
            return res;
        }

        public static long ParseLong(string strValue)
        {
            long res;
            long.TryParse(strValue, out res);
            return res;
        }

        public static byte ParseByte(string strValue)
        {
            byte res;
            byte.TryParse(strValue, out res);
            return res;
        }

        public static float ParseFloat(string strValue)
        {
            float res;
            float.TryParse(strValue, out res);
            return res;
        }

        public static double ParseDouble(string strValue)
        {
            double res;
            double.TryParse(strValue, out res);
            return res;
        }

        public static decimal ParseDecimal(string strValue)
        {
            decimal res;
            decimal.TryParse(strValue, out res);
            return res;
        }

        public static bool ParseBool(string strValue)
        {
            bool res;
            bool.TryParse(strValue, out res);
            return res;
        }

        public static DateTime ParseDateTime(string strValue)
        {
            DateTime res;
            DateTime.TryParse(strValue, out res);
            return res;
        }

        #endregion

        #region === 由Object取值 ===

        /// <summary>
        /// 取得Int值
        /// </summary>
        public static int GetInt(object obj)
        {
            return obj != DBNull.Value ? Convert.ToInt32(obj) : 0;
        }

        /// <summary>
        /// 获得Long值
        /// </summary>
        public static long GetLong(object obj)
        {
            return obj != DBNull.Value ? Convert.ToInt64(obj) : 0;
        }

        /// <summary>
        /// 取得Byte值
        /// </summary>
        public static byte GetByte(object obj)
        {
            return (byte)(obj != DBNull.Value ? Convert.ToByte(obj) : 0);
        }

        /// <summary>
        /// 取得Byte[]
        /// </summary>
        public static byte[] GetBinary(object obj)
        {
            return obj != DBNull.Value ? (byte[])obj : null;
        }

        /// <summary>
        /// 取得Float值
        /// </summary>
        public static float GetFloat(object obj)
        {
            return obj != DBNull.Value ? Convert.ToSingle(obj) : 0;
        }

        /// <summary>
        /// 取得Double值
        /// </summary>
        public static double GetDouble(object obj)
        {
            return obj != DBNull.Value ? Convert.ToDouble(obj) : 0;
        }

        /// <summary>
        /// 取得Decimal值
        /// </summary>
        public static decimal GetDecimal(object obj)
        {
            return obj != DBNull.Value ? Convert.ToDecimal(obj) : 0;
        }

        /// <summary>
        /// 取得Guid值
        /// </summary>
        public static Guid GetGuid(object obj)
        {
            return obj != DBNull.Value ? new Guid(obj.ToString()) : Guid.Empty;
        }

        /// <summary>
        /// 取得Bool值
        /// </summary>
        public static bool GetBool(object obj)
        {
            return obj != DBNull.Value && Convert.ToBoolean(obj);
        }

        /// <summary>
        /// 取得DateTime值
        /// </summary>
        public static DateTime GetDateTime(object obj)
        {
            if (obj == DBNull.Value) return DateTime.MinValue;
            DateTime temp;
            return DateTime.TryParse(obj.ToString(), out temp) ? temp : DateTime.MinValue;
        }

        /// <summary>
        /// 取得Int16值
        /// </summary>
        public static short GetInt16(object obj)
        {
            return (short)(obj != DBNull.Value ? Convert.ToInt16(obj) : 0);
        }

        /// <summary>
        /// 取得Int32值
        /// </summary>
        public static int GetInt32(object obj)
        {
            return obj != DBNull.Value ? Convert.ToInt32(obj) : 0;
        }

        /// <summary>
        /// 获得Int64值
        /// </summary>
        public static long GetInt64(object obj)
        {
            return obj != DBNull.Value ? Convert.ToInt64(obj) : 0;
        }

        /// <summary>
        /// 取得UInt16值
        /// </summary>
        public static ushort GetUInt16(object obj)
        {
            return (ushort)(obj != DBNull.Value ? Convert.ToUInt16(obj) : 0);
        }

        /// <summary>
        /// 取得SByte值
        /// </summary>
        public static sbyte GetSByte(object obj)
        {
            return (sbyte)(obj != DBNull.Value ? Convert.ToSByte(obj) : 0);
        }

        /// <summary>
        /// 取得UInt32值
        /// </summary>
        public static uint GetUInt(object obj)
        {
            return obj != DBNull.Value ? Convert.ToUInt32(obj) : 0;
        }

        /// <summary>
        /// 取得UInt64值
        /// </summary>
        public static ulong GetULong(object obj)
        {
            return obj != DBNull.Value ? Convert.ToUInt64(obj) : 0;
        }

        /// <summary>
        /// 取得String值
        /// </summary>
        public static string GetString(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return obj.ToString();
        }

        #endregion   
        public static string ExceptionMessage(Exception ex)
        {
            var message = ex.Message;
            var inner = ex.InnerException;
            while (inner != null)
            {
                message = inner.Message;//+= "," + inner.Message;
                inner = inner.InnerException;
            }
            message = message.Replace("\r\n", "").Replace("'", "");
            return message;
        }
        private static JsonSerializerSettings jsonSetting;
        public static JsonSerializerSettings GetJsonSetting()
        {
            if (jsonSetting == null)
            {
                jsonSetting = new JsonSerializerSettings();
                jsonSetting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                //jsonSetting.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                //jsonSetting.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                jsonSetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }
            return jsonSetting;
        }
    }
}
