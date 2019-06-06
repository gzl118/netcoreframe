using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Common
{
    public class DateTools
    {
        public static DateTime DoubleToTime(double d)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddMilliseconds(d);
            return dt;
        }
        public static double TimeToDouble(DateTime t)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            return (t - dt).TotalSeconds;
        }
        public static List<string> Hours24()
        {
            return new List<string> {"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12",
                "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
        }
        public static List<string> Hours3()
        {
            return new List<string> { "0", "1", "2" };
        }
    }
}
