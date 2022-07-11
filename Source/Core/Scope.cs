using CoChain;

namespace Urbrural.Core
{
    /// <summary>
    /// An object scope.
    /// </summary>
    public class Scope : Entity
    {
        readonly Map<string, MvObj> objmap = new Map<string, MvObj>(32);

        public MvObj GetVar(string varName) => objmap[varName];
    }
}