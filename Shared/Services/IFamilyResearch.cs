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
        IEnumerable<FamilyResearch> GetFamilyResearches();
        void Update(FamilyResearch familyResearch);
        IEnumerable<FamilyResearch> GetFamilyResearchByMinorName(string minorName);
        IEnumerable<Movimiento> GetMovimientosByTipoMovimiento(string tipoMovimiento);
        FamilyMembers GetFamilyFamilyMembersById(int? familyMembersId);
        EconomicSituation GetEconomicSituationById(int? economicSituationId);
        FamilyNutrition GetFamilyNutritionById(int? familyNutritionId);
        BenefitsProvided GetBenefitsProvidedById(int? benefitsProvidedId);
        IngresosEgresosMensuales GetIngresosEgresosMensualesById(int? Id);
    }
}