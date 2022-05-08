using Chainly;

namespace Urbrural
{
    public class Factor : Info, IKeyable<string>
    {
        string id;

        string sel;

        Selector selector;

        FactorState[] states;


        public string Key => id;
    }
}