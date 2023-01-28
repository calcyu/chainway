using System.Threading.Tasks;
using ChainFx.Web;
using ChainPort.Core;
using static ChainFx.Fabric.Nodality;

namespace ChainPort
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
            var regs = Grab<short, MvScene>();
        }

        public async Task search(WebContext wc, int sect)
        {
            bool inner = wc.Query[nameof(inner)];
            int orgid = wc[0];
            var regs = Grab<short, MvScene>();
            if (inner)
            {
                using var dc = NewDbContext();
            }
            else
            {
                wc.GivePage(200, h =>
                {
                    // h.TOPBAR_().SUBNAV(regs, string.Empty, sect,  (k, v) => v.typ == MvScene.TYP_SECT);
                    h.T("");
                }, true, 60);
            }
        }
    }
}