using System.Threading.Tasks;
using ChainFx.Web;
using ChainPort.Core;
using static ChainFx.Nodal.Nodality;
using static ChainFx.Web.Modal;

namespace ChainPort
{
    public abstract class ProjectWork : WebWork
    {
    }

    public class PublyProjectWork : ProjectWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<PublyProjectVarWork>();
        }

        public void @default(WebContext wc, int page)
        {
        }
    }

    [Ui("商户线上货架设置", "thumbnails")]
    public class OrglyProjectWork : ProjectWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<OrglyProjectVarWork>();
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