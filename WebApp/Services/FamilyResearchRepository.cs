using Naandi.Shared.Models;
using Naandi.Shared.Services;
using WebApp.Data;

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
    }
}