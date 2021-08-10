using System;
using System.Data;
using System.Threading.Tasks;
using SkyChain;
using SkyChain.Web;
using static SkyChain.Web.Appear;
using static SkyChain.Web.Modal;
using static Zhnt.Market.Ro;
using static Zhnt._Doc;
using static Zhnt.WeChatUtility;

namespace Zhnt.Market
{
    public class MyRoVarWork : WebWork
    {
        [Ui("✎", "✎ 填写日志"), Tool(ButtonShow)]
        public async Task log(WebContext wc, int dt)
        {
            int orderid = wc[0];
            if (wc.IsGet)
            {
            }
            else // POST
            {
                wc.GivePane(200); // close
            }
        }

        [Ui("标签", group: 1), Tool(ButtonShow)]
        public async Task tag(WebContext wc)
        {
            int orderid = wc[0];
            var prin = (User) wc.Principal;
            if (wc.IsGet)
            {
                // list
                using var dc = NewDbContext();
                dc.Sql("SELECT tag FROM orders WHERE id = @1 AND uid = @2");
                var o = (string) await dc.ScalarAsync(p => p.Set(orderid).Set(prin.id));
                wc.GivePane(200, h =>
                {
                    h.FORM_();
                    h.FIELDSUL_("相较调养之前");
                    h.LI_().TEXT("肠胃排泄", nameof(o), o)._LI();
                    h._FIELDSUL();
                    h._FORM();
                });
            }
            else // POST
            {
                string tag = (await wc.ReadAsync<Form>())[nameof(tag)];
                using var dc = NewDbContext();
                dc.Sql("UPDATE orders SET tag = @1 WHERE id = @1 AND uid = @2");
                dc.Execute(p => p.Set(tag).Set(orderid).Set(prin.id));

                wc.GivePane(200); // close
            }
        }

        [Ui("协议", group: 2), Tool(ButtonOpen)]
        public void agrmt(WebContext wc, int dietid)
        {
            var diets = Fetch<Map<short, Org>>();
            var diet = diets[(short) dietid];
            wc.GivePane(200, h =>
            {
                h.FORM_();
                h._FORM();
            });
        }

        [Ui("申诉", group: 2), Tool(ButtonShow)]
        public async Task compl(WebContext wc)
        {
            int orderid = wc[0];
            var prin = (User) wc.Principal;
            short appeal;
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
                await dc.QueryTopAsync("SELECT compl FROM orders WHERE id = @1 AND uid = @2", p => p.Set(orderid).Set(prin.id));
                dc.Let(out appeal);
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("申诉类型");
                    for (int i = 0; i < Compls.Count; i++)
                    {
                        var e = Compls.EntryAt(i);
                        h.LI_().RADIO(nameof(appeal), e.Key, e.Value, @checked: e.Key == appeal)._LI();
                    }
                    h._FIELDSUL()._FORM();
                });
            }
            else
            {
                appeal = (await wc.ReadAsync<Form>())[nameof(appeal)];
                using var dc = NewDbContext();
                dc.Sql("UPDATE orders SET appeal = @1 WHERE id = @2 AND uid = @3");
                dc.Execute(p => p.Set(appeal).Set(orderid).Set(prin.id));
                wc.GivePane(200); // close
            }
        }
    }

    public class BizlyRoVarWork : WebWork
    {
        [Ui("☰", "☰ 明细"), Tool(ButtonOpen, Half)]
        public async Task dtl(WebContext wc)
        {
            short orgid = wc[-2];
            int orderid = wc[0];
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Empty).T(" FROM orderlgs WHERE orderid = @1 ORDER BY dt");
            var arr = await dc.QueryAsync<Ro>(p => p.Set(orderid));
            wc.GivePane(200, h =>
            {
                var today = DateTime.Today;
                h.TABLE(arr, o => { h.TD_("uk-width-micro").T("&#10004;&#65039;", o.status > 0)._TD(); });
                h.BOTTOM_BUTTON("退款", nameof(refund));
            });
        }

        [Ui("⥻", "⥻ 退款"), Tool(ButtonOpen, Small)]
        public async Task refund(WebContext wc)
        {
            short orgid = wc[-2];
            int orderid = wc[0];
            short level = 0;
            if (wc.IsGet)
            {
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("请选择退款比例");
                    h.LI_().SELECT("退款比例", nameof(level), level, Levels);
                    h._FIELDSUL();
                    h.BOTTOM_BUTTON("退款", nameof(refund));
                    h._FORM();
                });
            }
            else // POST
            {
                level = (await wc.ReadAsync<Form>())[nameof(level)];

                using var dc = NewDbContext(IsolationLevel.ReadCommitted);
                try
                {
                    var percent = level * 0.10M;
                    dc.Sql("UPDATE orders SET refund = pay * @1, status = CASE WHEN @1 = 1 THEN ").T(STATUS_ABORTED).T(" ELSE status END WHERE id = @2 AND orgid = @3 AND status IN (").T(STATUS_ISSUED).T(", ").T(STATUS_ARGUED).T(") RETURNING refund");
                    var refund = (decimal) await dc.ScalarAsync(p => p.Set(percent).Set(orderid).Set(orgid));
                    if (refund <= 0)
                    {
                        wc.Give(403); // forbidden
                        return;
                    }

                    // remote call weixin
                    string orderno = orderid.ToString();
                    string err = await PostRefundAsync(orderno, refund, refund, orderno);
                    if (err != null) // not success
                    {
                        dc.Rollback();
                        ERR(err);
                    }
                }
                catch (Exception)
                {
                    dc.Rollback();
                    ERR("退款失败: orderid = " + orderid);
                    return;
                }
                wc.GivePane(200);
            }
        }
    }
}