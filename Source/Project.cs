using System;
using System.Collections.Concurrent;
using Chainly;
using Urbrural.Mv;

namespace Urbrural
{
    public class Project : Info, IKeyable<int>, IMvScope<Reg>
    {
        #region IDATA

        public static readonly Project Empty = new Project();

        int id;
        double x;
        double y;
        string unit;
        short min;
        short max;
        short step;
        JObj mpml;

        public override void Read(ISource s, short proj = 255)
        {
            base.Read(s, proj);

            if ((proj & ID) == ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(x), ref x);
            s.Get(nameof(y), ref y);
            s.Get(nameof(unit), ref unit);
            s.Get(nameof(min), ref min);
            s.Get(nameof(max), ref max);
            s.Get(nameof(step), ref step);
            s.Get(nameof(mpml), ref mpml);
        }

        public override void Write(ISink s, short proj = 255)
        {
            base.Write(s, proj);

            if ((proj & ID) == ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(x), x);
            s.Put(nameof(y), y);
            s.Put(nameof(unit), unit);
            s.Put(nameof(min), min);
            s.Put(nameof(max), max);
            s.Put(nameof(step), step);
            s.Put(nameof(mpml), mpml);
        }

        public int Key => id;

        #endregion


        #region MPML

        // view
        MvView view;

        // stages
        MvStage[] stages;


        Map<string, Var> vars = new Map<string, Var>();

        public Var GetVar(string varName) => vars[varName];

        #endregion
    }
}