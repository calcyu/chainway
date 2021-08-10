using System;
using System.Threading.Tasks;
using SkyChain;
using SkyChain.Web;
using static System.Data.IsolationLevel;
using static SkyChain.Web.Modal;

namespace Zhnt.Supply
{
    public class CtrlyDoVarWork : WebWork
    {
        [Ui(group: 1), Tool(ButtonOpen)]
        public  async Task act(WebContext wc, int cmd)
        {
            int lotid = wc[0];
            var prin = (User) wc.Principal;

            if (wc.IsGet)
            {
                using var dc = NewDbContext();

                dc.Sql("SELECT ").collst(Uo.Empty).T(" FROM lots_vw WHERE id = @1");
                var m = await dc.QueryTopAsync<Uo>(p => p.Set(lotid));
            }
            else // POST
            {
                if (cmd == 1)
                {
                    using var dc = NewDbContext(ReadCommitted);
                    try
                    {
                        var err = await dc.RemoveLotJnAsync(lotid, prin.id, "主动撤销");
                        if (err != null)
                        {
                            dc.Rollback();
                        }
                    }
                    catch
                    {
                        dc.Rollback();
                    }
                }
                wc.GivePane(200);
            }
        }
    }


    public class BizlyDoVarWork : WebWork
    {
        [Ui, Tool(ButtonOpen)]
        public  async Task act(WebContext wc, int cmd)
        {
            int lotid = wc[0];
            short orgid = wc[-2];
            if (wc.IsGet)
            {
                var orgs = Fetch<Map<short, Org>>();

                using var dc = NewDbContext();
                dc.Sql("SELECT ").collst(Uo.Empty).T(" FROM lots_vw WHERE id = @1");
                var m = await dc.QueryTopAsync<Uo>(p => p.Set(lotid));
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                int[] key = f[nameof(key)];
                if (cmd == 1)
                {
                    using var dc = NewDbContext(ReadCommitted);
                    try
                    {
                        foreach (int uid in key)
                        {
                            await dc.RemoveLotJnAsync(lotid, uid, "商家撤销");
                        }
                    }
                    catch (Exception e)
                    {
                        ERR(e.Message);
                        dc.Rollback();
                    }
                }
                else
                {
                }

                wc.GiveRedirect(nameof(act));
            }
        }

        [Ui("修改", group: 1), Tool(ButtonOpen)]
        public async Task upd(WebContext wc)
        {
            var prin = (User) wc.Principal;
            short orgid = wc[-2];
            var org = Fetch<Map<short, Org>>()[orgid];
            int id = wc[0];
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
                dc.Sql("SELECT ").collst(Uo.Empty).T(" FROM lots_vw WHERE id = @1");
                var m = await dc.QueryTopAsync<Uo>(p => p.Set(id));
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                short typ = f[nameof(typ)];
                var m = new Uo
                {
                    orgid = orgid,
                    typ = typ,
                    status = _Doc.STATUS_DRAFT,
                    author = prin.name
                };
                m.Read(f);
                using var dc = NewDbContext();
                dc.Sql("UPDATE lots ")._SET_(m, 0).T(" WHERE id = @1");
                await dc.ExecuteAsync(p =>
                {
                    m.Write(p, 0);
                    p.Set(id);
                });

                wc.GivePane(201);
            }
        }

        [Ui("图标", group: 1), Tool(ButtonCrop, Appear.Small)]
        public async Task icon(WebContext wc)
        {
            int id = wc[0];
            if (wc.IsGet)
            {
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                ArraySegment<byte> img = f[nameof(img)];
                using var dc = NewDbContext();
                if (await dc.ExecuteAsync("UPDATE lots SET icon = @1 WHERE id = @2", p => p.Set(img).Set(id)) > 0)
                {
                    wc.Give(200); // ok
                }
                else wc.Give(500); // internal server error
            }
        }

        [Ui("图片", group: 1), Tool(ButtonCrop, Appear.Small)]
        public async Task img(WebContext wc)
        {
            int id = wc[0];
            if (wc.IsGet)
            {
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                ArraySegment<byte> img = f[nameof(img)];
                using var dc = NewDbContext();
                if (await dc.ExecuteAsync("UPDATE lots SET img = @1 WHERE id = @2", p => p.Set(img).Set(id)) > 0)
                {
                    wc.Give(200); // ok
                }
                else wc.Give(500); // internal server error
            }
        }

        // [Ui("核实"), Tool(Modal.ButtonShow)]
        public async Task apprv(WebContext wc)
        {
            short orgid = wc[-2];
            var orgs = Fetch<Map<short, Org>>();
            var org = orgs[orgid];
            long job = wc[0];
            bool ok;
            if (wc.IsGet)
            {
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("核实申请");
                    h.LI_().CHECKBOX(null, nameof(ok), true, tip: "我确定此项申请情况属实，同意奖励数字珍珠", required: true)._LI();
                    h._FORM()._FIELDSUL();
                });
            }
            else
            {
                ok = (await wc.ReadAsync<Form>())[nameof(ok)];
                if (ok)
                {
                    using var dc = NewDbContext(ReadCommitted);

                    // var tx = new ChainTransaction(1)
                    // .Row()

                    // await dc.ExecuteAsync("", org.Acct);
                }
                wc.GivePane(200);
            }
        }
    }
}