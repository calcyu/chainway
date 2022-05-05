using System;
using System.Collections.Concurrent;
using Chainly;
using Urbrural.Mv;

namespace Urbrural
{
    public class Project : Info, IMvView, IMvScope
    {
        public static readonly Project Empty = new Project();

        string unit;

        short min;

        short max;

        short step;

        // model definition
        JObj mpml;


        public override void Read(ISource s, short proj = 255)
        {
            base.Read(s, proj);
        }

        public override void Write(ISink s, short proj = 255)
        {
            base.Write(s, proj);
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

        public Site SiteAt(int p)
        {
            throw new NotImplementedException();
        }

        public void AddSite(Site v)
        {
            if (sites == null)
            {
                sites = new ConcurrentDictionary<int, Site>();
            }
            sites.TryAdd(v.Key, v);
        }
    }
}