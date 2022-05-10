using Chainly;
using Urbrural.Mpml;

namespace Urbrural
{
    /// <summary>
    /// A job prototype model.
    /// </summary>
    public class Jobp : Info, IKeyable<string>, IProto
    {
        string id;

        JobpState[] states;

        public string Key => id;
    }
}