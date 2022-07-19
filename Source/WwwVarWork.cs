using System.Threading.Tasks;
using CoChain.Web;
using Urbrural.Core;
using static CoChain.Nodal.Store;

namespace Urbrural
{
    /// <summary>
    /// The home page for markets and businesses therein..
    /// </summary>
    public class WwwVarWork : WebWork
    {
        public async Task @default(WebContext wc, int sect)
        {
            int orgid = wc[0];
            var org = GrabObject<int, Org>(orgid);
            var regs = Grab<short, MvScope>();
        }

        public async Task search(WebContext wc, int sect)
        {
            bool inner = wc.Query[nameof(inner)];
            int orgid = wc[0];
            var regs = Grab<short, MvScope>();
            if (inner)
            {
                using var dc = NewDbContext();
            }
            else
            {
                wc.GivePage(200, h =>
                {
                    h.TOPBAR_().SUBNAV(regs, string.Empty, sect, filter: (k, v) => v.typ == MvScope.TYP_SECT);
                    h.T("");
                }, true, 60);
            }
        }
    }
}