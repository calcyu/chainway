using CoChain;
using Urbrural.Core;

namespace Urbrural
{
    public class CatUtility
    {
        public static Map<string, Cat> All = new Map<string, Cat>();


        public static Cat GetSchema(string name)
        {
            return All[name];
        }
    }
}