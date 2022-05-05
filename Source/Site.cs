using Chainly;
using Urbrural.Mv;

namespace Urbrural
{
    public class Site : Info, IKeyable<int>, IMvScope
    {
        int id;

        // lattitude and longitude
        double x, y;


        public override void Read(ISource s, short proj = 255)
        {
            base.Read(s, proj);
        }

        public override void Write(ISink s, short proj = 255)
        {
            base.Write(s, proj);
        }

        public int Key => id;
    }
}