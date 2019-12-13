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
    }
}