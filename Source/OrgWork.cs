using System;
using System.Threading.Tasks;
using ChainFx;
using ChainFx.Web;
using ChainPort.Core;
using static ChainFx.Nodal.Nodality;
using static ChainFx.Web.Modal;

namespace ChainPort
{
    public abstract class OrgWork : WebWork
    {
    }


    [Ui("平台入驻机构设置", "album")]
    public class AdmlyOrgWork : OrgWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<AdmlyOrgVarWork>();
        }

        [Ui("市场", group: 1), Tool(Anchor)]
        public async Task @default(WebContext wc, int page)
        {
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw ORDER BY regid, status DESC");
            var arr = await dc.QueryAsync<Org>();
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                h.TABLE(arr, o =>
                {
                    h.TDCHECK(o.id);
                    h.TD_().AVAR(o.Key, o.name)._TD();
                    h.TD_("uk-visible@s").T(o.addr)._TD();
                    h.TD(Entity.Statuses[o.status]);
                });
            });
        }

        [Ui("✚", "新建入驻机构", group: 7), Tool(ButtonOpen)]
        public async Task @new(WebContext wc, int typ)
        {
            var prin = (User) wc.Principal;
            var regs = Grab<short, MvScene>();
            var orgs = Grab<int, Org>();

            if (wc.IsGet)
            {
                var m = new Org
                {
                    typ = (short) typ,
                    created = DateTime.Now,
                    creator = prin.name,
                };
                m.Read(wc.Query, 0);
                wc.GivePane(200, h =>
                {
                    var typname = Org.Typs[m.typ];
                    h.FORM_().FIELDSUL_(typname + "机构信息");
                    h.LI_().TEXT(typname + "名称", nameof(m.name), m.name, min: 2, max: 12, required: true)._LI();
                    h.LI_().TEXTAREA("简介", nameof(m.tip), m.tip, max: 30)._LI();
                    h.LI_().TEXT("地址", nameof(m.addr), m.addr, max: 20)._LI();
                    h.LI_().NUMBER("经度", nameof(m.x), m.x, min: 0.000, max: 180.000).NUMBER("纬度", nameof(m.y), m.y, min: -90.000, max: 90.000)._LI();
                    h.LI_().SELECT("状态", nameof(m.status), m.status, Entity.Statuses, filter: (k, v) => k > 0)._LI();
                    h._FIELDSUL()._FORM();
                });
            }
            else // POST
            {
                var o = await wc.ReadObjectAsync(0, new Org
                {
                    typ = (short) typ,
                    created = DateTime.Now,
                    creator = prin.name,
                });
                using var dc = NewDbContext();
                dc.Sql("INSERT INTO orgs ").colset(Org.Empty, 0)._VALUES_(Org.Empty, 0);
                await dc.ExecuteAsync(p => o.Write(p, 0));
                wc.GivePane(201); // created
            }
        }
    }
}