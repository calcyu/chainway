using System.Reflection;
using CoChain;

namespace Urbrural.Core
{
    /// <summary>
    /// Represents a metaverse entity, that has a set of metaverse objects
    /// </summary>
    public class MvScope : Entity
    {
        readonly Map<string, MvObj> objmap = new Map<string, MvObj>(32);

        public MvObj GetVar(string varName) => objmap[varName];


        internal void Init()
        {
            var type = GetType();

            // gather actions
            foreach (var fi in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (typeof(MvObj).IsAssignableFrom(fi.FieldType)) // MvObj
                {
                    if (fi.GetValue(this) is MvObj o)
                    {
                        objmap.Add(o.key, o);
                    }
                }
            }
        }
    }
}