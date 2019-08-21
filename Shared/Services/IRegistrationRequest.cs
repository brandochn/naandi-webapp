using Naandi.Shared.Models;
using System.Collections.Generic;

namespace Naandi.Shared.Services
{
    public interface IRegistrationRequest
    {
        void Add(RegistrationRequest registrationRequest);

        RegistrationRequest GetRegistrationRequestById(int id);
        IList<MaritalStatus> GetMaritalStatuses();
        IList<Relationship> GetRelationships();
        IList<StatesOfMexico> GetStatesOfMexico();
        IList<MunicipalitiesOfMexico> GetMunicipalitiesOfMexicoByStateOfMexicoName(string nameOfState);
        IList<RegistrationRequest> GetRegistrationRequests(int limitRequest);
        IList<RegistrationRequest> GetRegistrationRequestsByMinorName(string minorName);
        void DeleteById(int Id);
        void Update(RegistrationRequest registrationRequest);
        IList<RegistrationRequestStatus> RegistrationRequestStatuses();
    }
}