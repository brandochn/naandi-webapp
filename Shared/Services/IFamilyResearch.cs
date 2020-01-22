using System.Collections.Generic;
using Naandi.Shared.Models;

namespace Naandi.Shared.Services
{
    public interface IFamilyResearch
    {
        void Add(FamilyResearch familyResearch);
        IEnumerable<MaritalStatus> GetMaritalStatuses();
        IEnumerable<Relationship> GetRelationships();
        IEnumerable<StatesOfMexico> GetStatesOfMexico();
        IEnumerable<MunicipalitiesOfMexico> GetMunicipalitiesOfMexicoByStateOfMexicoName(string nameOfState);
        IEnumerable<HomeAcquisition> GetHomeAcquisitions();
        IEnumerable<TypesOfHouses> GetTypesOfHouses();
        IEnumerable<TipoDeMobiliario> GetTipoDeMobiliarios();
        IEnumerable<TypeOfDistrict> GetTypeOfDistricts();
        IEnumerable<Patrimony> GetPatrimonies();
        IEnumerable<Food> GetFoods();
        IEnumerable<Frequency> GetFrequencies();
        FamilyResearch GetFamilyResearchById(int id);
    }
}