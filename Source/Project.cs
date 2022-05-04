using System;
using System.Collections.Concurrent;
using Chainly;
using Urbrural.Core;

namespace Urbrural
{
    public class Project : Info, IMvView, IMvProcess
    {
        public static readonly Project Empty = new Project();

        // scheme id
        private short typ;


        string unit;

        short min;

        short step;

        short max;


        public void StartAllDeals()
        {
        }

        public Site SiteAt(int p)
        {
            throw new NotImplementedException();
        }

        //
        // MPML

        // view
        MvView view;

        // stages
        MvStage[] stages;


        //
        // sites belong to this project
        ConcurrentDictionary<int, Site> sites;
    }
}