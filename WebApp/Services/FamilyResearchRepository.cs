using System.Collections.Generic;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using RestSharp;
using WebApp.Data;
using WebApp.ExtensionMethods;

namespace WebApp.Services
{
    public class FamilyResearchRepository : IFamilyResearch
    {
        private readonly ApplicationRestClient applicationRestClient;

        public FamilyResearchRepository(ApplicationRestClient _applicationRestClient)
        {
            applicationRestClient = _applicationRestClient;
        }
        public void Add(FamilyResearch familyResearch)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<HomeAcquisition> GetHomeAcquisitions()
        {
             IList<HomeAcquisition> acquisitions;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetHomeAcquisitions", Method.GET);
            acquisitions = client.GetCall<List<HomeAcquisition>>(request);

            return acquisitions;
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

        public IEnumerable<TypesOfHouses> GetTypesOfHouses()
        {
            IList<TypesOfHouses> typeOfHouses;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetTypesOfHouses", Method.GET);
            typeOfHouses = client.GetCall<List<TypesOfHouses>>(request);

            return typeOfHouses;
        }
    }
}