using Chainly;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Urbrural
{
    public class VarpState : IData
    {
        short mode;

        decimal scale;

        string label;

        // average, mean, max, min
        short valuing;


        public void Read(ISource s, short msk = 255)
        {
        }

        public void Write(ISink s, short msk = 255)
        {
        }

        private Script<bool> predicate;

        VarpState()
        {
            var d = CSharpScript.Create<bool>("");
        }
    }
}