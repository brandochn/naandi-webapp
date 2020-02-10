using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using WebApp.ExtensionMethods;

namespace WebApp.SessionState
{
    public static class UserSession
    {
        private static IHttpContextAccessor _httpAccessor;

        private static HttpContext HttpContext => _httpAccessor.HttpContext;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpAccessor = httpContextAccessor;
        }

        public static void AddItemInDataCollection<T>(string sessionKeyName, T value)
        {
            if (string.IsNullOrEmpty(sessionKeyName) == true)
            {
                throw new ArgumentNullException("sessionKeyName cannot be null or empty");
            }

            IList<T> collection = GetDataCollection<List<T>>(sessionKeyName);

            if (collection == null)
            {
                collection = new List<T>();
            }

            collection.Add(value);

            HttpContext.Session.Set(sessionKeyName, collection);
        }

        public static T GetDataCollection<T>(string sessionKeyName)
        {
            return HttpContext.Session.Get<T>(sessionKeyName);
        }

        public static void RemoveItemInDataCollection<T>(string sessionKeyName, int index)
        {
            if (string.IsNullOrEmpty(sessionKeyName) == true)
            {
                throw new ArgumentNullException("sessionKeyName cannot be null or empty");
            }

            IList<T> collection = GetDataCollection<List<T>>(sessionKeyName);

            if (collection == null)
            {
               return;
            }

            collection.RemoveAt(index);

            HttpContext.Session.Set(sessionKeyName, collection);
        }
    }
}