namespace Urbrural
{
    public abstract class OpContext
    {
        Reg reg;

        protected Project project;

        protected Deal deal;


        public string Stage => deal.Stage;

        public string Job => deal.job;
    }
}