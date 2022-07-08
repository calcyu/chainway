using CoChain;

namespace Urbrural.Core
{
    public class Scope : Info
    {
        readonly Map<string, MvObj> objmap = new Map<string, MvObj>(32);


        public MvObj GetVar(string varName) => objmap[varName];
    }
}