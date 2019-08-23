using RestSharp;

namespace WebApp.Data
{
    public class ApplicationRestClient
    {
        private string AppServiceUri { get; set; }

        public ApplicationRestClient(string _appServiceUri)
        {
            AppServiceUri = _appServiceUri;
        }

        public IRestClient CreateRestClient()
        {
            return new RestClient(AppServiceUri).UseSerializer(() => new JsonNetSerializer());
        }
    }
}