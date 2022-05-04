using System.Threading.Tasks;
using Chainly.Web;
using static Chainly.Nodal.Store;
using static Chainly.Web.Modal;

namespace Urbrural
{
    public class PlayWork : WebWork
    {
    }

    [Ui("平台供应链电商报告", "table")]
    public class AdmlyDealWork : OrgWork
    {
        public async Task @default(WebContext wc)
        {
            wc.GivePage(200, h => { h.TOOLBAR(); });
        }
    }

    public class PublyDealWork : OrgWork
    {
        public async Task @default(WebContext wc, int code)
        {
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Play.Empty).T(" FROM books WHERE id = @1 LIMIT 1");
            var o = await dc.QueryTopAsync<Play>(p => p.Set(code));
            wc.GivePage(200, h => { }, title: "中惠农通溯源系统");
        }
    }

    [UserAuthorize(Org.TYP_PRI, 1)]
    [Ui("商户线上订货", "file-text")]
    public class OrglyPlayWork : PlayWork
    {
        [Ui("当前", group: 1), Tool(Anchor)]
        public async Task @default(WebContext wc, int page)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
        }

        [Ui("历史", group: 2), Tool(Anchor)]
        public async Task past(WebContext wc, int page)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
        }


        [Ui("✚", "新增进货", group: 1), Tool(ButtonOpen)]
        public async Task @new(WebContext wc)
        {
            var org = wc[-1].As<Org>();

            var ctr = GrabObject<int, Org>(2);
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
            }
            else // POST
            {
                wc.GivePane(200); // close dialog
            }
        }
    }
}