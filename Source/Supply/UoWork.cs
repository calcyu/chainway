using System;
using System.Threading.Tasks;
using SkyChain;
using SkyChain.Db;
using SkyChain.Web;
using static SkyChain.Web.Modal;
using static Zhnt._Doc;
using static Zhnt.User;

namespace Zhnt.Supply
{
    
    [UserAuthorize(admly: 1)]
    [Ui("采购")]
    public class AdmlyUoWork : WebWork
    {
        protected override void OnMake()
        {
            MakeVarWork<CtrlyDoVarWork>();
        }

        [Ui("当前", group: 1), Tool(Anchor)]
        public async Task @default(WebContext wc, int page)
        {
        }
    }

    [UserAuthorize(orgly: ORGLY_OP)]
    [Ui("拼团")]
    public class CtrlyUoWork : WebWork
    {
        protected override void OnMake()
        {
            MakeVarWork<CtrlyUoVarWork>();
        }

        [Ui("当前", group: 1), Tool(Anchor)]
        public async Task @default(WebContext wc, int page)
        {
            short orgid = wc[-1];
            var orgs = Fetch<Map<short, Org>>();

            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Uo.Empty).T(" FROM lots_vw WHERE orgid = @1 AND status < ").T(STATUS_ISSUED).T(" ORDER BY id DESC LIMIT 10 OFFSET @2 * 10");
            var arr = await dc.QueryAsync<Uo>(p => p.Set(orgid).Set(page), 0xff);

            wc.GivePage(200, h =>
            {
                h.TOOLBAR(caption: "当前活跃拼团");
                if (arr != null)
                {
                    h.ViewLotList(arr, orgs, DateTime.Today);
                }
                h.PAGINATION(arr?.Length == 10);
            }, false, 3);
        }

        [Ui("停止", group: 2), Tool(Anchor)]
        public async Task closed(WebContext wc, int page)
        {
            short orgid = wc[-1];
            var orgs = Fetch<Map<short, Org>>();

            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Uo.Empty).T(" FROM lots_vw WHERE orgid = @1 AND status >= ").T(STATUS_ISSUED).T(" ORDER BY status, id DESC LIMIT 10 OFFSET @2 * 10");
            var arr = await dc.QueryAsync<Uo>(p => p.Set(orgid).Set(page), 0xff);

            wc.GivePage(200, h =>
            {
                h.TOOLBAR(caption: "已成和已结拼团");
                if (arr != null)
                {
                    h.ViewLotList(arr, orgs, DateTime.Today);
                }
                h.PAGINATION(arr?.Length == 10);
            }, false, 3);
        }

        [Ui("发布", group: 1), Tool(ButtonOpen)]
        public async Task @new(WebContext wc, int typ)
        {
            var prin = (User) wc.Principal;
            short orgid = wc[-1];
            var orgs = Fetch<Map<short, Org>>();
            var org = orgs[orgid];
            if (wc.IsGet)
            {
                if (typ == 0) // display type selection
                {
                    wc.GivePane(200, h =>
                    {
                        h.FORM_().FIELDSUL_("请选择推广类型");
                      
                    });
                }
                else // typ specified
                {
                  
                }
            }
            else // POST
            {
                var today = DateTime.Today;
                var o = await wc.ReadObjectAsync(inst: new Uo
                {
                    status = STATUS_DRAFT,
                    orgid = orgid,
                    issued = today,
                    author = prin.name,
                    @extern = org.@extern,
                });
                // database op
                using var dc = NewDbContext();
                dc.Sql("INSERT INTO lots ").colset(o, 0)._VALUES_(o, 0);
                await dc.QueryTopAsync(p => o.Write(p, 0));

                wc.GivePane(201);
            }
        }

        [Ui("复制", group: 2), Tool(ButtonPickOpen)]
        public async Task copy(WebContext wc)
        {
            short orgid = wc[-1];
            var prin = (User) wc.Principal;
            var ended = DateTime.Today.AddDays(3);
            int[] key;
            if (wc.IsGet)
            {
                key = wc.Query[nameof(key)];
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("目标截止日期");
                    h.LI_().DATE("截止", nameof(ended), ended)._LI();
                    h._FIELDSUL();
                    h.HIDDENS(nameof(key), key);
                    h.BOTTOM_BUTTON("确认", nameof(copy));
                    h._FORM();
                });
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                ended = f[nameof(ended)];
                key = f[nameof(key)];
                using var dc = NewDbContext();
                dc.Sql("INSERT INTO lots (typ, status, orgid, issued, ended, span, name, tag, tip, unit, unitip, price, min, max, least, step, extern, addr, start, author, icon, img) SELECT typ, 0, orgid, issued, @1, span, name, tag, tip, unit, unitip, price, min, max, least, step, extern, addr, start, @2, icon, img FROM lots WHERE orgid = @3 AND id")._IN_(key);
                await dc.ExecuteAsync(p => p.Set(ended).Set(prin.name).Set(orgid).SetForIn(key));

                wc.GivePane(201);
            }
        }

        public async Task tbank(WebContext wc, int page)
        {
            short orgid = wc[-1];
            var orgs = Fetch<Map<short, Org>>();
            var org = orgs[orgid];
            var prin = (User) wc.Principal;
            using var dc = NewDbContext();
            var todo = await dc.SeekQueueAsync(org.Acct);
            wc.GivePage(200, h =>
            {
                h.TOOLBAR(caption: "核实公益价值（区块链）");

                h.LI_().LABEL("代办事项")._LI();
                foreach (var o in todo)
                {
                    h.LI_("uk-flex");
                    h.SPAN_("uk-width-1-2").T(o.Name).T(" ➜")._SPAN();
                    h.SPAN(o.Remark, "uk-width-expand");
                    // h.SPAN_("uk-width-micro uk-text-right").VARTOOLS(o.Job)._SPAN();
                    h._LI();
                }
            });
        }
    }


    [UserAuthorize(orgly: ORGLY_OP)]
    [Ui("运送")]
    public class SprlyUoWork : WebWork
    {
        protected override void OnMake()
        {
            MakeVarWork<SprlyUoVarWork>();
        }

        public async Task @default(WebContext wc, int page)
        {
            short orgid = wc[-1];
            using var dc = NewDbContext();
            dc.Sql("SELECT m.id, m.name, m.price, m.unit, d.uid, d.uname, d.utel, d.uim, d.qty, d.pay FROM lots m, lotjns d WHERE m.id = d.lotid AND m.status = ").T(STATUS_ISSUED).T(" AND d.ptid = @1 ORDER BY m.id");
            await dc.QueryAsync(p => p.Set(orgid));

            var orgs = Fetch<Map<short, Org>>();
            wc.GivePage(200, h =>
            {
                h.TOOLBAR(caption: "拼团递货管理");

                int last = 0;
                while (dc.Next())
                {
                    dc.Let(out int id);
                    dc.Let(out string name);
                    dc.Let(out decimal price);
                    dc.Let(out string unit);
                    dc.Let(out int uid);
                    dc.Let(out string uname);
                    dc.Let(out string utel);
                    dc.Let(out string uim);
                    dc.Let(out short qty);
                    dc.Let(out decimal pay);
                    if (id != last)
                    {
                        h._LI();
                        if (last != 0)
                        {
                            h._UL();
                            h._ARTICLE();
                        }
                        h.ARTICLE_("uk-card uk-card-default");
                        h.HEADER_("uk-card-header").T(name).SPAN_("uk-badge").CNY(price).T('／').T(unit)._SPAN()._HEADER();
                        h.UL_("uk-card-body");
                    }
                    h.LI_("uk-flex uk-width-1-1");
                    h.P(uname, "uk-width-1-3");
                    h.P(qty, "uk-text-right uk-width-1-6");
                    h.P(pay, "uk-text-right uk-width-1-6");
                    h._LI();

                    last = id;
                }
                h._UL();
                h._ARTICLE();
            });
        }
    }
}