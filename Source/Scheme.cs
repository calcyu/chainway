namespace Urbrural
{
    public abstract class Scheme
    {
        private string id;

        private string name;
        
        // pertained factors
        Fact[] factors;

        // allowed or potential jobs
        Job[] jobcaps;


        //
        // processing cycles and logics
        
        
        //
        // visual layout

        public abstract void Layout();
    }
}