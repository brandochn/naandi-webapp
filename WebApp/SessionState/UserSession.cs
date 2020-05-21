using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Naandi.Shared.DataBase;
using Naandi.Shared.Services;
using WebApp.Data;
using WebApp.ExtensionMethods;
using WebApp.Services;

namespace WebApp.SessionState
{
    public static class UserSession
    {
        private static IHttpContextAccessor _httpAccessor;
        private static IConfiguration _configuration;

        private static HttpContext HttpContext => _httpAccessor.HttpContext;

        public static void Configure(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpAccessor = httpContextAccessor;
            _configuration = configuration;
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

        public static void SetToken(string token)
        {
            HttpContext.Session.Set("jwt_token", token);
        }

        public static string GetToken()
        {
            return HttpContext.Session.Get<string>("jwt_token");
        }

        public static void ReConnectToWebApi()
        {
            bool userIsSignedIn = HttpContext.User.Identity.IsAuthenticated;
            if (userIsSignedIn)
            {
                string userName = HttpContext.User.Identity.Name;
                ApplicationDbContext applicationDbContext = new ApplicationDbContext(_configuration["ConnectionString"]);
                ApplicationRestClient applicationRestClient = new ApplicationRestClient(_configuration["AppServiceUri"]);
                IUser userResearchRepository = new UserRepository(applicationDbContext, applicationRestClient);
                var user = userResearchRepository.GetUserByName(userName);
                var token = userResearchRepository.CreateToken(user.UserName, user.Password);

                token = token.Replace("\"", "");

                SetToken(token);
            }
        }
    }
}