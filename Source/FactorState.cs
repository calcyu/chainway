using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Urbrural
{
    public class FactorState
    {
        // one, many, 
        short cardinality;

        // average, mean, max, min
        short valuing;


        byte[] webp;

        byte[] audio;

        private Script<bool> predicate;

        FactorState()
        {
            var d = CSharpScript.Create<bool>("");
        }
    }
}