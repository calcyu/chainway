using System.Threading.Tasks;
using ChainFx;
using ChainFx.Web;
using ChainPort.Core;
using static ChainFx.Nodal.Nodality;

namespace ChainPort
{
    /// <summary>
    /// The home page for regionnal view of supply
    /// </summary>
    public class MgtVarWork : WebWork
    {
        // display all related sources
        public async Task @default(WebContext wc, int sect)
        {
            int ctrid = wc[0];
            var org = GrabObject<int, Org>(ctrid);
            var regs = Grab<short, MvScene>();

            // get list of assocated sources
            var orgs = Grab<int, Org>();
            var ids = new ValueList<int>();
            // orgs.ForEach((k, v) => v.HasCtr(ctrid), (k, v) => ids.Add(k));

            using var dc = NewDbContext();
        }

        public async Task prod(WebContext wc, int sect)
        {
            int ctrid = wc[0];
            var org = GrabObject<int, Org>(ctrid);
            var regs = Grab<short, MvScene>();

            // get list of assocated sources
            var orgs = Grab<int, Org>();
            var ids = new ValueList<int>();
            // orgs.ForEach((k, v) => v.HasCtr(ctrid), (k, v) => ids.Add(k));
        }

        public async Task search(WebContext wc, int cur)
        {
        }
    }
}