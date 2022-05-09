using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Urbrural
{
    public class ThingState
    {
        // one, many, 
        short cardinality;

        // average, mean, max, min
        short valuing;


        byte[] webp;

        byte[] audio;

        private Script<bool> predicate;

        ThingState()
        {
            var d = CSharpScript.Create<bool>("");
        }
    }
}