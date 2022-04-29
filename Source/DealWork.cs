using System.Threading.Tasks;
using Chainly.Web;
using static Chainly.Nodal.Store;
using static Chainly.Web.Modal;

namespace Urbrural
{
    public class DealWork : WebWork
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
            dc.Sql("SELECT ").collst(Book.Empty).T(" FROM books WHERE id = @1 LIMIT 1");
            var o = await dc.QueryTopAsync<Book>(p => p.Set(code));
            wc.GivePage(200, h =>
            {
                if (o == null || o.srcid - o.srcid >= code)
                {
                    h.ALERT("编码没有找到");
                }
                else
                {
                    var frm = GrabObject<int, Org>(o.wareid);
                    var ctr = GrabObject<int, Org>(o.wareid);

                    h.FORM_();
                    h.FIELDSUL_("溯源信息");
                    h.LI_().FIELD("生产户", frm.name);
                    h.LI_().FIELD("分拣中心", ctr.name);
                    h._FIELDSUL();
                    h._FORM();
                }
            }, title: "中惠农通溯源系统");
        }
    }

    [UserAuthorize(Org.TYP_BIZ, 1)]
    [Ui("商户线上订货", "file-text")]
    public class OrglyDealWork : DealWork
    {
        [Ui("当前", group: 1), Tool(Anchor)]
        public async Task @default(WebContext wc, int page)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Book.Empty).T(" FROM books WHERE bizid = @1 AND status = 0 ORDER BY id");
            var arr = await dc.QueryAsync<Book>(p => p.Set(org.id));

            wc.GivePage(200, h =>
            {
                h.TOOLBAR(tip: "当前订货");
                h.TABLE(arr, o =>
                {
                    // h.TD(items[o.itemid].name);
                    h.TD(o.ctrid);
                    h.TDFORM(() => { });
                });
            });
        }

        [Ui("历史", group: 2), Tool(Anchor)]
        public async Task past(WebContext wc, int page)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Book.Empty).T(" FROM books WHERE bizid = @1 AND status >= 1 ORDER BY id");
            var arr = await dc.QueryAsync<Book>(p => p.Set(org.id));

            wc.GivePage(200, h =>
            {
                h.TOOLBAR(tip: "历史订货");
                h.TABLE(arr, o =>
                {
                    // h.TD(items[o.itemid].name);
                    h.TD(o.ctrid);
                    h.TDFORM(() => { });
                });
            });
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
                var o = await wc.ReadObjectAsync<Book>(0);
                using var dc = NewDbContext();
                dc.Sql("INSERT INTO buys ").colset(Book.Empty, 0)._VALUES_(Book.Empty, 0);
                await dc.ExecuteAsync(p => o.Write(p, 0));

                wc.GivePane(200); // close dialog
            }
        }
    }
}