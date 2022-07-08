namespace Urbrural.Core
{
    public class MvContext000001 : MvContext
    {

        public MvObj Weather
        {
            get
            {
                return project.GetVar(nameof(Weather));
            }
        }

        public MvObj Water
        {
            get
            {
                return deal.GetVar(nameof(Water));
            }
        }
    }
}