using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Services;

namespace WebApp.Areas.SocialWork.Controllers
{
    [Area("SocialWork")]
    public class FamilyResearchController : Controller
    {
        private readonly IFamilyResearch familyResearchRepository;
        public FamilyResearchController(IFamilyResearch _familyResearchRepository)
        {
            familyResearchRepository = _familyResearchRepository;
        }
    }
}