using System;
using System.Threading.Tasks;
using Chainly;
using Chainly.Web;
using Urbrural;
using static Chainly.Nodal.Store;
using static Chainly.Web.Modal;

namespace Urbrural
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
            dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE typ = ").T(Org.TYP_MRT).T(" ORDER BY regid, status DESC");
            var arr = await dc.QueryAsync<Org>();
            wc.GivePage(200, h =>
            {
                h.TOOLBAR(subscript: Org.TYP_MRT);
                h.TABLE(arr, o =>
                {
                    h.TDCHECK(o.id);
                    h.TD_().AVAR(o.Key, o.name)._TD();
                    h.TD_("uk-visible@s").T(o.addr)._TD();
                    h.TD_().A_TEL(o.mgrname, o.Tel)._TD();
                    h.TD(Info.Symbols[o.status]);
                    h.TDFORM(() => h.TOOLGROUPVAR(o.Key));
                });
            });
        }

        [Ui("中枢", group: 2), Tool(Anchor)]
        public async Task ctr(WebContext wc, int page)
        {
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE typ = ").T(Org.TYP_CTR).T(" ORDER BY regid, status DESC");
            var arr = await dc.QueryAsync<Org>();
            wc.GivePage(200, h =>
            {
                h.TOOLBAR(subscript: Org.TYP_CTR);
                h.TABLE(arr, o =>
                {
                    h.TDCHECK(o.id);
                    h.TD_().AVAR(o.Key, o.name)._TD();
                    h.TD_("uk-visible@s").T(o.addr)._TD();
                    h.TD_().A_TEL(o.mgrname, o.Tel)._TD();
                    h.TD(Info.Symbols[o.status]);
                    h.TDFORM(() => h.TOOLGROUPVAR(o.Key));
                });
            });
        }

        [Ui("产源", group: 4), Tool(Anchor)]
        public async Task prv(WebContext wc, int page)
        {
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE typ = ").T(Org.TYP_SRC).T(" ORDER BY status DESC");
            var arr = await dc.QueryAsync<Org>();
            wc.GivePage(200, h =>
            {
                h.TOOLBAR(subscript: Org.TYP_SRC);
                h.TABLE(arr, o =>
                {
                    h.TDCHECK(o.id);
                    h.TD_().AVAR(o.Key, o.name)._TD();
                    h.TD_("uk-visible@s").T(o.addr)._TD();
                    h.TD_().A_TEL(o.mgrname, o.Tel)._TD();
                    h.TD(Info.Symbols[o.status]);
                    h.TDFORM(() => h.TOOLGROUPVAR(o.Key));
                });
            });
        }

        [Ui("✚", "新建入驻机构", group: 7), Tool(ButtonShow)]
        public async Task @new(WebContext wc, int typ)
        {
            var prin = (User) wc.Principal;
            var regs = Grab<short, Reg>();
            var orgs = Grab<int, Org>();

            if (wc.IsGet)
            {
                var m = new Org
                {
                    typ = (short) typ,
                    created = DateTime.Now,
                    creator = prin.name,
                    status = Info.STA_ENABLED
                };
                m.Read(wc.Query, 0);
                wc.GivePane(200, h =>
                {
                    var typname = Org.Typs[m.typ];
                    h.FORM_().FIELDSUL_(typname + "机构信息");
                    h.LI_().TEXT(typname + "名称", nameof(m.name), m.name, min: 2, max: 12, required: true)._LI();
                    h.LI_().TEXTAREA("简介", nameof(m.tip), m.tip, max: 30)._LI();
                    if (m.IsSrc)
                    {
                        h.LI_().SELECT("物流投放", nameof(m.fork), m.fork, Org.Forks, required: true).SELECT("业务版块", nameof(m.zone), m.zone, Org.Zones, required: true)._LI();
                    }
                    h.LI_().SELECT(m.HasLocality ? "所在地市" : "所在省份", nameof(m.regid), m.regid, regs, filter: (k, v) => m.HasLocality ? v.IsDist : v.IsProv, required: !m.IsSrc)._LI();
                    h.LI_().TEXT("地址", nameof(m.addr), m.addr, max: 20)._LI();
                    if (m.HasXy)
                    {
                        h.LI_().NUMBER("经度", nameof(m.x), m.x, min: 0.000, max: 180.000).NUMBER("纬度", nameof(m.y), m.y, min: -90.000, max: 90.000)._LI();
                    }
                    if (m.IsSpr)
                    {
                        h.LI_().SELECT("关联中枢", nameof(m.ctras), m.ctras, orgs, filter: (k, v) => v.IsCtr, multiple: m.IsSrc, required: true)._LI();
                    }
                    h.LI_().SELECT("状态", nameof(m.status), m.status, Info.Statuses, filter: (k, v) => k > 0)._LI();
                    h._FIELDSUL()._FORM();
                });
            }
            else // POST
            {
                var o = await wc.ReadObjectAsync(Info.BORN, new Org
                {
                    typ = (short) typ,
                    created = DateTime.Now,
                    creator = prin.name,
                });
                using var dc = NewDbContext();
                dc.Sql("INSERT INTO orgs ").colset(Org.Empty, Info.BORN)._VALUES_(Org.Empty, Info.BORN);
                await dc.ExecuteAsync(p => o.Write(p, Info.BORN));
                wc.GivePane(201); // created
            }
        }
    }
}