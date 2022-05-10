using Chainly;

namespace Urbrural.Mpml
{
    public class Rule : IKeyable<string>
    {
        public string Key { get; }


        Map<string, Rule> All = new Map<string, Rule>()
        {
            new Rule()
        };
    }
}