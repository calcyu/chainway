using System.Reflection;
using ChainFx;

namespace ChainPort.Core
{
    /// <summary>
    /// A metaverse scope of states, that copmrises of an object set.
    /// </summary>
    public abstract class MvScope : Entity, IVisual
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

        public void Render(MvContext mc)
        {
        }
    }
}