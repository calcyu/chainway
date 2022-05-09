using Microsoft.CodeAnalysis.Scripting;

namespace Urbrural.Mv
{
    public abstract class MvActivityDef : MvDef
    {
        
        
        Script<decimal> OnStart;
        
        Script<decimal> OnEnd;
        
        Script<decimal> OnE3nd;
        
        Script<decimal> On2End;
    }
}