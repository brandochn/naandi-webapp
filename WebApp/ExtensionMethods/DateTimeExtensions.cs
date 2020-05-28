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
            TimeZoneInfo mxTimeZone = null;
            try
            {
                // Microsoft Windows time zone
                mxTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            }
            catch { }

            if (mxTimeZone == null)
            {
                //  IANA time zone
                mxTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City");
            }

            DateTime mxTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, mxTimeZone);
            return mxTime;
        }
    }
}
