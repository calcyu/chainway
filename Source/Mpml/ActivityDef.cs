using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;

namespace Urbrural.Mpml
{
    public abstract class ActivityDef : Def
    {
        /// <summary>
        /// Tests if this activity is available 
        /// </summary>
        Script<bool> _if;

        Script<decimal> OnStart;

        Script<decimal> OnEnd;


        public async Task<bool> If(OpContext ctx)
        {
            if (_if != null)
            {
                var sta = await _if.RunAsync(globals: ctx);
                return sta.ReturnValue;
            }
            return true;
        }
    }
}