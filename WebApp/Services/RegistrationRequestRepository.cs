using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Collections.Generic;
using WebApp.Data;
using RestSharp;
using Newtonsoft.Json;
using WebApp.ExtensionMethods;

namespace WebApp.Services
{
    public class RegistrationRequestRepository : IRegistrationRequest
    {
        private readonly ApplicationRestClient applicationRestClient;

        public RegistrationRequestRepository(ApplicationRestClient _applicationRestClient)
        {
            applicationRestClient = _applicationRestClient;
        }

        public void Add(RegistrationRequest registrationRequest)
        {
            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/RegistrationRequest/AddRegistrationRequest", Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(registrationRequest), ParameterType.RequestBody);

            var response = client.Post(request);

            if (response.ErrorException != null)
            {
                string message = Constants.UNHANDLED_EXCEPTION_MESSAGE;
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }
        }

        public void DeleteById(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MaritalStatus> GetMaritalStatuses()
        {
            IList<MaritalStatus> maritalStatuses;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/RegistrationRequest/GetMaritalStatuses", Method.GET);
            maritalStatuses = client.GetCall<List<MaritalStatus>>(request);

            return maritalStatuses;
        }

        public IEnumerable<MunicipalitiesOfMexico> GetMunicipalitiesOfMexicoByStateOfMexicoName(string nameOfState)
        {
            IList<MunicipalitiesOfMexico> municipalities;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/RegistrationRequest/GetMunicipalitiesOfMexicoByStateOfMexicoName/{nameOfState}", Method.GET);

            request.AddUrlSegment("nameOfState", nameOfState);
            municipalities = client.GetCall<List<MunicipalitiesOfMexico>>(request);

            return municipalities;
        }

        public RegistrationRequest GetRegistrationRequestById(int id)
        {
            RegistrationRequest registrationRequest;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/RegistrationRequest/GetRegistrationRequestById/{Id}", Method.GET);
            request.AddUrlSegment("Id", id);

            registrationRequest = client.GetCall<RegistrationRequest>(request);

            return registrationRequest;
        }

        public IEnumerable<RegistrationRequest> GetRegistrationRequestsByMinorName(string minorName)
        {
            IList<RegistrationRequest> registrationRequests;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/RegistrationRequest/GetRegistrationRequestsByMinorName/{name}", Method.GET);

            request.AddUrlSegment("name", minorName);
            var response = client.Execute<List<RegistrationRequest>>(request);

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<RegistrationRequest>();
            }

            if (response.ErrorException != null)
            {
                const string message = Constants.UNHANDLED_EXCEPTION_MESSAGE;
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            registrationRequests = response.Data;

            return registrationRequests;
        }

        public IEnumerable<RegistrationRequest> GetRegistrationRequests()
        {
            IList<RegistrationRequest> registrationRequests = new List<RegistrationRequest>();
            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/RegistrationRequest/GetRegistrationRequests", Method.GET);
            registrationRequests = client.GetCall<List<RegistrationRequest>>(request);

            return registrationRequests;
        }

        public IEnumerable<Relationship> GetRelationships()
        {
            IList<Relationship> Relationship;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/RegistrationRequest/GetRelationships", Method.GET);
            Relationship = client.GetCall<List<Relationship>>(request);

            return Relationship;
        }

        public IEnumerable<StatesOfMexico> GetStatesOfMexico()
        {
            IList<StatesOfMexico> statesOfMexicoList;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/RegistrationRequest/GetStatesOfMexico", Method.GET);
            statesOfMexicoList = client.GetCall<List<StatesOfMexico>>(request);

            return statesOfMexicoList;
        }

        public IEnumerable<RegistrationRequestStatus> RegistrationRequestStatuses()
        {
            IList<RegistrationRequestStatus> registrationRequestStatuses;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/RegistrationRequest/RegistrationRequestStatuses", Method.GET);
            registrationRequestStatuses = client.GetCall<List<RegistrationRequestStatus>>(request);

            return registrationRequestStatuses;
        }

        public void Update(RegistrationRequest registrationRequest)
        {
            Add(registrationRequest);
        }
    }
}
