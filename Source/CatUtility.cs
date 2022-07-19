using CoChain;
using Urbrural.Core;

namespace Urbrural
{
    public class CatUtility
    {
        public static Map<string, Reg> All = new Map<string, Reg>();


        public static Reg GetSchema(string name)
        {
            return All[name];
        }
    }
}