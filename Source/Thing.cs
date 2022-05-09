using Chainly;

namespace Urbrural
{
    public class Thing : Info, IKeyable<string>
    {
        string id;

        string sel;

        Switch _switch;

        ThingState[] states;


        public string Key => id;
    }
}