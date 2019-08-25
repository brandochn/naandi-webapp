using System;
using RestSharp;
using Naandi.Shared.Models;

namespace WebApp.ExtensionMethods
{
    public static class RestSharpExtensions
    {
        public static T GetCall<T>(this IRestClient client, RestRequest request) where T : new()
        {
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = Constants.UNHANDLED_EXCEPTION_MESSAGE;
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            return response.Data;
        }
    }
}