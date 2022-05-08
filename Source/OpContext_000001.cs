namespace Urbrural
{
    public class OpContext_000001 : OpContext
    {

        public Variable Weather
        {
            get
            {
                return project.GetVariable(nameof(Weather));
            }
        }

        public Variable Water
        {
            get
            {
                return deal.GetVariable(nameof(Water));
            }
        }
    }
}