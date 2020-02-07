using Microsoft.AspNetCore.Http;

namespace WebApp.SessionState
{
    public static class UserSession
    {
        private static IHttpContextAccessor _httpAccessor;

        public static HttpContext HttpContext => _httpAccessor.HttpContext;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpAccessor = httpContextAccessor;
        }
    }
}