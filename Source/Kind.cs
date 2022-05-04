using Chainly;

namespace Urbrural
{
    public class Kind : IKeyable<short>
    {
        readonly short key;

        readonly string name;

        Kind(short key, string name)
        {
            this.key = key;
            this.name = name;
        }

        public short Key { get; }

        public static Map<short, Kind> All = new Map<short, Kind>()
        {
            new Kind(1, "庄稼"),
            new Kind(1, "作物"),
            new Kind(1, "药材"),
            new Kind(1, "环境"),
        };
    }
}