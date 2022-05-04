using Microsoft.CodeAnalysis.Scripting;

namespace Urbrural.Core
{
    public abstract class MvActivityTag : MvTag
    {
        Script<decimal> OnStart;
        Script<decimal> OnEnd;
        Script<decimal> OnE3nd;
        Script<decimal> On2End;
    }
}