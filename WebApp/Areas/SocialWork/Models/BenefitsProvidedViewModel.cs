using Naandi.Shared.Models;

namespace WebApp.Areas.SocialWork.Models
{
    public class BenefitsProvidedViewModel : BenefitsProvidedDetails
    {
        private string key;

        public string Key { get => key; set => key = value; }
    }
}