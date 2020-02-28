namespace WebApp.Areas.SocialWork.Models
{
    public class PatrimonyViewModelCollection
    {
        private PatrimonyViewModel[] collection = new PatrimonyViewModel[100];

        public PatrimonyViewModel this[int index]
        {
            get
            {
                return collection[index];
            }

            set
            {
                collection[index] = value;
            }
        }

        public PatrimonyViewModel this[string name]
        {
            get
            {
                foreach (PatrimonyViewModel iter in collection)
                {
                    if (string.Equals(iter.Name, name, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return iter;
                    }
                }

                return null;
            }
        }
    }
}