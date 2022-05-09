namespace Urbrural
{
    public class OpContext_000001 : OpContext
    {

        public Var Weather
        {
            get
            {
                return project.GetVar(nameof(Weather));
            }
        }

        public Var Water
        {
            get
            {
                return deal.GetVar(nameof(Water));
            }
        }
    }
}