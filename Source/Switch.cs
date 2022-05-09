using System;
using Chainly;

namespace Urbrural
{
    public class Switch : IKeyable<string>
    {
        private Func<float, OpContext, short> func;

        public string Key { get; }

        public static Map<string, Switch> All = new Map<string, Switch>
        {
            new Switch()
            {
                func = (v, ctx) => { return 0; }
            }
        };
    }
}