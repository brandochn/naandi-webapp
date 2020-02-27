using System.Collections.Generic;

namespace WebApp.Areas.SocialWork.Models
{
    public class PatrimonyViewModelCollection
    {
        private Dictionary<string, PatrimonyViewModel> entries = new Dictionary<string, PatrimonyViewModel>();

        public PatrimonyViewModel this[string key]
        {
            get
            {
                // Return the value for this key or the default value.
                if (entries.ContainsKey(key))
                {
                    return entries[key];
                }

                return null;
            }
            set
            {
                // Set the property's value for the key.
                entries.Add(key, value);
            }
        }
    }
}