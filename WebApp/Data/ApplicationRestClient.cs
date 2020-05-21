using RestSharp;
using RestSharp.Authenticators;
using WebApp.SessionState;

namespace WebApp.Data
{
    public class ApplicationRestClient
    {
        private string AppServiceUri { get; set; }

        public ApplicationRestClient(string _appServiceUri)
        {
            AppServiceUri = _appServiceUri;
        }

        public IRestClient CreateRestClient(bool includeToken = true)
        {
            RestClient client = new RestClient(AppServiceUri);
            client.UseSerializer(() => new JsonNetSerializer());
            if (includeToken == true)
            {
                if (UserSession.GetToken() == null)
                {
                    UserSession.ReConnectToWebApi();
                }
                client.Authenticator = new JwtAuthenticator(UserSession.GetToken());
            }

            return client;
        }
    }
}