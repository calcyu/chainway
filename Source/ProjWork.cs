using System.Threading.Tasks;
using Chainly.Web;
using Urbrural;
using static Chainly.Nodal.Store;
using static Chainly.Web.Modal;

namespace Urbrural
{
    public abstract class ProjWork : WebWork
    {
    }

    public class PublyProjWork : ProjWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<PublyProjVarWork>();
        }

        public void @default(WebContext wc, int page)
        {
        }
    }

    [Ui("商户线上货架设置", "thumbnails")]
    public class OrglyProjWork : ProjWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<OrglyProjVarWork>();
        }

        [Ui("上架", group: 1), Tool(Anchor)]
        public async Task @default(WebContext wc)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Project.Empty).T(" FROM pieces WHERE orgid = @1 AND status >= 2 ORDER BY status DESC");
            var arr = await dc.QueryAsync<Project>(p => p.Set(org.id));
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                h.TABLE(arr, o =>
                {
                    h.TD_().A_TEL(o.name, o.tip)._TD();
                    // h.TD(o.price, true);
                    // h.TD(Statuses[o.status]);
                });
            });
        }

        [Ui("下架", group: 2), Tool(Anchor)]
        public async Task off(WebContext wc)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Project.Empty).T(" FROM peices WHERE orgid = @1 AND status <= 1 ORDER BY status DESC");
            var arr = await dc.QueryAsync<Project>(p => p.Set(org.id));
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                h.TABLE(arr, o =>
                {
                    // h.TD_().TOOLVAR(o.Key, nameof(BizlyPieceVarWork.upd), caption: o.name).SP()._TD();
                    // h.TD(o.price, true);
                    // h.TD(Statuses[o.status]);
                });
            });
        }
    }
}