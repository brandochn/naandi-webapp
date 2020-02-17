using System;

namespace WebApp.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string GetUniqueKey(this string s)
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }

            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
    }
}