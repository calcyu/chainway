using System;
using System.Threading.Tasks;
using CoChain;
using CoChain.Web;
using Urbrural.Core;
using static Urbrural.User;
using static CoChain.Nodal.Store;
using static CoChain.Web.Modal;

namespace Urbrural
{
    public abstract class RegWork : WebWork
    {
    }

    [UserAuthorize(admly: ADMLY_MGT)]
    [Ui("平台地域设置", "world")]
    public class AdmlyRegWork : RegWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<AdmlyRegVarWork>();
        }

        public void @default(WebContext wc)
        {
            wc.GivePage(200, h =>
            {
                h.TOOLBAR(subscript: Reg.TYP_PROV);
                using var dc = NewDbContext();
                dc.Sql("SELECT ").collst(Reg.Empty).T(" FROM regs WHERE typ = ").T(Reg.TYP_PROV).T(" ORDER BY id, state DESC");
                var arr = dc.Query<Reg>();
                h.TABLE(arr, o =>
                    {
                        h.TDCHECK(o.Key);
                        h.TDAVAR(o.Key, o.name);
                        h.TDFORM(() => h.TOOLGROUPVAR(o.Key, subscript: Reg.TYP_PROV));
                    }
                );
            });
        }

        [Ui("✚", "新建区域", group: 7), Tool(ButtonShow)]
        public async Task @new(WebContext wc, int typ)
        {
            var prin = (User) wc.Principal;
            var o = new Reg
            {
                typ = (short) typ,
                state = Entity.STA_ENABLED,
                created = DateTime.Now,
                creator = prin.name,
            };
            if (wc.IsGet)
            {
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("区域属性");
                    h.LI_().NUMBER("区域编号", nameof(o.id), o.id, min: 1, max: 99, required: true)._LI();
                    h.LI_().SELECT("类型", nameof(o.typ), o.typ, Reg.Typs, filter: (k, v) => k == typ, required: true)._LI();
                    h.LI_().TEXT("名称", nameof(o.name), o.name, min: 2, max: 10, required: true)._LI();
                    h.LI_().NUMBER("排序", nameof(o.idx), o.idx, min: 1, max: 99)._LI();
                    h.LI_().SELECT("状态", nameof(o.state), o.state, Entity.States)._LI();
                    h._FIELDSUL()._FORM();
                });
            }
            else // POST
            {
                o = await wc.ReadObjectAsync(instance: o);
                using var dc = NewDbContext();
                // dc.Sql("INSERT INTO regs ").colset(Reg.Empty)._VALUES_(Item.Empty);
                await dc.ExecuteAsync(p => o.Write(p));

                wc.GivePane(200); // close dialog
            }
        }
    }
}