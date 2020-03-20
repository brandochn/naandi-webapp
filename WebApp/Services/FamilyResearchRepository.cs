using System;
using System.Collections.Generic;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using Newtonsoft.Json;
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
            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/AddFamilyResearch", Method.POST);
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(familyResearch), ParameterType.RequestBody);

            var response = client.Post(request);

            if (response.ErrorException != null || response.IsSuccessful == false)
            {
                string message = Constants.UNHANDLED_EXCEPTION_MESSAGE;
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }
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

        public IEnumerable<TipoDeMobiliario> GetTipoDeMobiliarios()
        {
            IList<TipoDeMobiliario> tipoDeMobiliarios;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetTipoDeMobiliarios", Method.GET);
            tipoDeMobiliarios = client.GetCall<List<TipoDeMobiliario>>(request);

            return tipoDeMobiliarios;
        }

        public IEnumerable<TypeOfDistrict> GetTypeOfDistricts()
        {
            IList<TypeOfDistrict> typeOfDistricts;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetTypeOfDistricts", Method.GET);
            typeOfDistricts = client.GetCall<List<TypeOfDistrict>>(request);

            return typeOfDistricts;
        }

        public IEnumerable<TypesOfHouses> GetTypesOfHouses()
        {
            IList<TypesOfHouses> typeOfHouses;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetTypesOfHouses", Method.GET);
            typeOfHouses = client.GetCall<List<TypesOfHouses>>(request);

            return typeOfHouses;
        }

        public IEnumerable<Patrimony> GetPatrimonies()
        {
            IList<Patrimony> patrimonies;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetPatrimonies", Method.GET);
            patrimonies = client.GetCall<List<Patrimony>>(request);

            return patrimonies;
        }

        public IEnumerable<Food> GetFoods()
        {
            IList<Food> foods;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetFoods", Method.GET);
            foods = client.GetCall<List<Food>>(request);

            return foods;
        }

        public IEnumerable<Frequency> GetFrequencies()
        {
            IList<Frequency> frequencies;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetFrequencies", Method.GET);
            frequencies = client.GetCall<List<Frequency>>(request);

            return frequencies;
        }

        public FamilyResearch GetFamilyResearchById(int id)
        {
            FamilyResearch familyResearch;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetFamilyResearchById/{Id}", Method.GET);
            request.AddUrlSegment("Id", id);

            familyResearch = client.GetCall<FamilyResearch>(request);

            return familyResearch;
        }

        public IEnumerable<FamilyResearch> GetFamilyResearches()
        {
            IList<FamilyResearch> familyResearches = new List<FamilyResearch>();
            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetFamilyResearches", Method.GET);
            familyResearches = client.GetCall<List<FamilyResearch>>(request);

            return familyResearches;
        }

        public void Update(FamilyResearch familyResearch)
        {
            Add(familyResearch);
        }

        public IEnumerable<FamilyResearch> GetFamilyResearchByMinorName(string minorName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Movimiento> GetMovimientosByTipoMovimiento(string tipoMovimiento)
        {
            IList<Movimiento> items;

            var client = applicationRestClient.CreateRestClient();
            var request = new RestRequest("/api/FamilyResearch/GetMovimientosByTipoMovimiento/{tipoMovimiento}", Method.GET);
            request.AddUrlSegment("tipoMovimiento", tipoMovimiento);

            items = client.GetCall<List<Movimiento>>(request);

            return items;
        }
    }
}