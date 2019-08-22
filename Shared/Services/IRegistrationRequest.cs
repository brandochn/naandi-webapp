using Naandi.Shared.Models;
using System.Collections.Generic;

namespace Naandi.Shared.Services
{
    public interface IRegistrationRequest
    {
        void Add(RegistrationRequest registrationRequest);

        RegistrationRequest GetRegistrationRequestById(int id);
        IEnumerable<MaritalStatus> GetMaritalStatuses();
        IEnumerable<Relationship> GetRelationships();
        IEnumerable<StatesOfMexico> GetStatesOfMexico();
        IEnumerable<MunicipalitiesOfMexico> GetMunicipalitiesOfMexicoByStateOfMexicoName(string nameOfState);
        IEnumerable<RegistrationRequest> GetRegistrationRequests(int limitRequest);
        IEnumerable<RegistrationRequest> GetRegistrationRequestsByMinorName(string minorName);
        void DeleteById(int Id);
        void Update(RegistrationRequest registrationRequest);
        IEnumerable<RegistrationRequestStatus> RegistrationRequestStatuses();
    }
}