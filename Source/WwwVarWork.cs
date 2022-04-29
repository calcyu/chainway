using System.Threading.Tasks;
using Chainly;
using Chainly.Web;
using Urbrural;
using static Chainly.Nodal.Store;

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
            var regs = Grab<short, Reg>();

            if (org.IsMrt)
            {
                using var dc = NewDbContext();
                dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE sprid = @1 AND regid = @2 AND status > 0 ORDER BY addr");
                var bizs = await dc.QueryAsync<Org>(p => p.Set(orgid).Set(sect));
                if (sect == 0) // when default sect
                {
                    wc.Subscript = sect = 99;
                    bizs = bizs.AddOf(org); // append the supervising market
                }
                wc.GivePage(200, h =>
                {
                    h.TOPBAR_().SUBNAV(regs, string.Empty, sect, filter: (k, v) => v.typ == Reg.TYP_SECT);
                    h.ADIALOG_(nameof(search), "-", sect, 8, false, Appear.Full).ICON("search", "uk-icon-button")._A();
                    h._TOPBAR();
                    h.GRID(bizs, o =>
                    {
                        h.SECTION_("uk-flex uk-card-body");
                        h.SPAN(o.ShopLabel, "uk-circle-label").SP();
                        h.ADIALOG_("/", o.id, "/", 8, false, Appear.Large, css: "uk-button-link").T(o.Shop)._A();
                        h._SECTION();
                    }, width: 2);
                }, title: org.name);
            }
            else if (org.IsBiz)
            {
                using var dc = NewDbContext();
            }
        }

        public async Task search(WebContext wc, int sect)
        {
            bool inner = wc.Query[nameof(inner)];
            int orgid = wc[0];
            var regs = Grab<short, Reg>();
            if (inner)
            {
                using var dc = NewDbContext();
            }
            else
            {
                wc.GivePage(200, h =>
                {
                    h.TOPBAR_().SUBNAV(regs, string.Empty, sect, filter: (k, v) => v.typ == Reg.TYP_SECT);
                    h.T("");
                }, true, 60);
            }
        }
    }
}