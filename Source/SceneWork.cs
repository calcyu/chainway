using System;
using System.Threading.Tasks;
using ChainFx;
using ChainFx.Web;
using ChainPort.Core;
using static ChainPort.User;
using static ChainFx.Nodal.Nodality;
using static ChainFx.Web.Modal;

namespace ChainPort
{
    public abstract class SceneWork : WebWork
    {
    }

    [UserAuthorize(admly: ADMLY_MGT)]
    [Ui("平台地域设置", "world")]
    public class AdmlySceneWork : SceneWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<AdmlySceneVarWork>();
        }

        public void @default(WebContext wc)
        {
            wc.GivePage(200, h =>
            {
                h.TOOLBAR(subscript: MvScene.TYP_PROV);
                using var dc = NewDbContext();
                dc.Sql("SELECT ").collst(MvScene.Empty).T(" FROM regs WHERE typ = ").T(MvScene.TYP_PROV).T(" ORDER BY id, state DESC");
                var arr = dc.Query<MvScene>();
                h.TABLE(arr, o =>
                    {
                        h.TDCHECK(o.Key);
                        h.TDAVAR(o.Key, o.name);
                    }
                );
            });
        }

        [Ui("✚", "新建区域", group: 7), Tool(ButtonOpen)]
        public async Task @new(WebContext wc, int typ)
        {
            var prin = (User) wc.Principal;
            var o = new MvScene
            {
                typ = (short) typ,
                created = DateTime.Now,
                creator = prin.name,
            };
            if (wc.IsGet)
            {
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("区域属性");
                    h.LI_().NUMBER("区域编号", nameof(o.id), o.id, min: 1, max: 99, required: true)._LI();
                    h.LI_().SELECT("类型", nameof(o.typ), o.typ, MvScene.Typs, filter: (k, v) => k == typ, required: true)._LI();
                    h.LI_().TEXT("名称", nameof(o.name), o.name, min: 2, max: 10, required: true)._LI();
                    h.LI_().NUMBER("排序", nameof(o.idx), o.idx, min: 1, max: 99)._LI();
                    h.LI_().SELECT("状态", nameof(o.status), o.status, Entity.Statuses)._LI();
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