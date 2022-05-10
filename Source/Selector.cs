using System;
using Chainly;

namespace Urbrural
{
    public class Selector : IKeyable<string>
    {
        private Func<float, OpContext, short> func;

        public string Key { get; }

        public static Map<string, Selector> All = new Map<string, Selector>
        {
            new Selector()
            {
                func = (v, ctx) => { return 0; }
            }
        };
    }
}