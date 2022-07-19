using System.Reflection;
using CoChain;

namespace Urbrural.Core
{
    /// <summary>
    /// An object scope.
    /// </summary>
    public class MvEntity : Entity
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