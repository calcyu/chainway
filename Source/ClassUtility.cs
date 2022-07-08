using CoChain;
using Urbrural.Core;

namespace Urbrural
{
    public class ClassUtility
    {
        public static Map<string, MvScene> All = new Map<string, MvScene>();


        public static MvScene GetSchema(string name)
        {
            return All[name];
        }
    }
}