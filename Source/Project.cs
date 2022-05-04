using System;
using Chainly;
using Urbrural.Core;

namespace Urbrural
{
    public class Project : Info, IMvView, IMvProcess
    {
        public static readonly Project Empty = new Project();

        private Scene _scene;

        // scheme id
        private short typ;


        string unit;

        short min;

        short step;

        short max;


        public void StartAllDeals()
        {
        }

        public ProjSite SiteAt(int p)
        {
            throw new NotImplementedException();
        }

        //
        // view

        // layout

        MvBox[] panels;

        //
        // process

        // stages
        MvStage[] stages;

        // sites
        ProjSite[] sites;
    }
}