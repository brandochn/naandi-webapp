using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static DateTime ToCentralMexicoTime(this DateTime dateTime)
        {
            var mxTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            DateTime mxTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, mxTimeZone);
           return mxTime;
        }
    }
}
