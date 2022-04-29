using Chainly;
using Coverse.Metaverse;

namespace Urbrural
{
    public class Proj : Info, IContainer
    {
        public static readonly Proj Empty = new Proj();

        private Scheme scheme;

        // scheme id
        private short typ;

        // stages
        Stage[] stages;

        // sites
        ProjSite[] sites;


        string unit;

        short min;

        short step;

        short max;


        public void StartAllDeals()
        {
        }

        public ProjSite SiteAt(int p)
        {
            throw new System.NotImplementedException();
        }
    }
}