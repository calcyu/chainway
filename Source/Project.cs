using System;
using System.Collections.Concurrent;
using Chainly;
using Urbrural.Mv;

namespace Urbrural
{
    public class Project : Info, IMvScope<Reg>
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
        ConcurrentDictionary<int, Deal> sites;

        public Deal SiteAt(int p)
        {
            throw new NotImplementedException();
        }

        public void AddSite(Deal v)
        {
            if (sites == null)
            {
                sites = new ConcurrentDictionary<int, Deal>();
            }
            sites.TryAdd(v.Key, v);
        }


        public Reg ParentScope { get; }
        
        Map<string, Variable> variables = new Map<string, Variable>();

        public Variable GetVariable(string varName) => variables[varName];


    }
}