using System;
using System.Threading.Tasks;
using System.Web;
using ChainFx;
using ChainFx.Web;
using ChainPort.Core;
using static ChainPort.WeChatUtility;
using static ChainFx.Nodal.Nodality;

namespace ChainPort
{
    [UserAuthenticate]
    public class WwwService : WebService
    {
        protected override void OnCreate()
        {
            CreateVarWork<WwwVarWork>(); // market home page

            CreateWork<PublyProjectWork>("proj");

            CreateWork<MyWork>("my");
        }

        public void @default(WebContext wc, int cmd)
        {
            var orgs = Grab<int, Org>();
            var regs = Grab<short, MvScene>();

            short regid = wc.Query[nameof(regid)];
            int mktid = wc.Query[nameof(mktid)];

            if (cmd == 0)
            {
                if (mktid == 0)
                {
                    mktid = wc.Cookies[nameof(mktid)].ToInt();
                }

            }
        }

        public void @catch(WebContext wc)
        {
            var e = wc.Error;
        }

        public async Task signin(WebContext wc)
        {
            string tel = null;
            string password = null;
            string url;
            if (wc.IsGet)
            {
                wc.GivePage(200, h =>
                {
                    url = wc.Query[nameof(url)];
                    url = HttpUtility.UrlDecode(url);

                    h.FORM_().FIELDSUL_("登录信息");
                    h.LI_().TEXT("手机号码", nameof(tel), tel, pattern: "[0-9]+", max: 11, min: 11, required: true);
                    h.LI_().PASSWORD("密码", nameof(password), password, max: 12, min: 3)._LI();
                    h._FIELDSUL();
                    h.HIDDEN(nameof(url), url);
                    h.BOTTOMBAR_().BUTTON("登录", nameof(signin))._BOTTOMBAR();
                    h._FORM();
                }, title: "用户登录");
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                tel = f[nameof(tel)];
                password = f[nameof(password)];
                url = f[nameof(url)];

                using var dc = NewDbContext();
                var credential = UrbruralUtility.ComputeCredential(tel, password);
                dc.Sql("SELECT ").collst(User.Empty).T(" FROM users WHERE tel = @1");
                var prin = dc.QueryTop<User>(p => p.Set(tel));
                if (prin == null || !credential.Equals(prin.credential))
                {
                    wc.GiveRedirect(nameof(signin));
                    return;
                }

                // successfully signed in
                wc.Principal = prin;
                wc.SetTokenCookie(prin);
                wc.GiveRedirect(url ?? "/");
            }
        }

        public async Task signup(WebContext wc)
        {
            // must have openid
            string openid = wc.Cookies[nameof(openid)];
            if (openid == null)
            {
                wc.Give(400); // bad request
                return;
            }

            string name = null;
            string tel = null;
            string url;
            if (wc.IsGet) // GET
            {
                wc.GivePage(200, h =>
                {
                    url = wc.Query[nameof(url)];
                    url = HttpUtility.UrlDecode(url);

                    h.FORM_().FIELDSUL_("注册信息");
                    h.LI_().TEXT("姓名", nameof(name), name, max: 10, min: 2, required: true);
                    h.LI_().TEXT("手机号码", nameof(tel), tel, pattern: "[0-9]+", max: 11, min: 11, required: true);
                    h._FIELDSUL();
                    h.HIDDEN(nameof(url), url);

                    h.SECTION_("uk-section uk-col uk-flex-middle");
                    h.P("如需接受消息通知清关注");
                    h.PIC("/qrcode.jpg", css: "uk-width-medium");
                    h._SECTION();

                    h.BOTTOMBAR_().BUTTON("注册", nameof(signup))._BOTTOMBAR();
                    h._FORM();
                }, title: "用户注册");
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                url = f[nameof(url)];
                var o = new User
                {
                    // status = Entity.STA_PRE,
                    name = f[nameof(name)],
                    tel = f[nameof(tel)],
                    im = openid,
                    created = DateTime.Now,
                };
                using var dc = NewDbContext();
                dc.Sql("INSERT INTO users ").colset(o, 0)._VALUES_(o, 0).T(" RETURNING ").collst(User.Empty);
                o = await dc.QueryTopAsync<User>(p => o.Write(p));

                // refresh cookie
                wc.Principal = o;
                wc.SetTokenCookie(o);
                wc.GiveRedirect(url);
            }
        }


        /// <summary>
        /// A booking payment notification.
        /// </summary>
        public async Task onbook(WebContext wc)
        {
            var xe = await wc.ReadAsync<XElem>();
            if (!OnNotified(xe, out var trade_no, out var cash))
            {
                wc.Give(400);
                return;
            }
            int pos = 0;
            var orderid = trade_no.ParseInt(ref pos);
            try
            {
                // NOTE: WCPay may send notification more than once
                using var dc = NewDbContext();
                // verify that the ammount is correct
                var today = DateTime.Today;
                dc.Sql("SELECT price FROM orders WHERE id = @1 AND status = 4");
                var price = (decimal) dc.Scalar(p => p.Set(orderid));
                if (price == cash) // update order status and line states
                {
                    dc.Sql("UPDATE orders SET status = 1, pay = @1, issued = @2 WHERE id = @3 AND status = 0");
                    await dc.ExecuteAsync(p => p.Set(cash).Set(today).Set(orderid));
                }
                else // try to refund this payment
                {
                    await PostRefundAsync(trade_no, cash, cash, trade_no);
                }
            }
            finally
            {
                // return xml to WCPay server
                var x = new XmlBuilder(true, 1024);
                x.ELEM("xml", null, () =>
                {
                    x.ELEM("return_code", "SUCCESS");
                    x.ELEM("return_msg", "OK");
                });
                wc.Give(200, x);
            }
        }

        /// <summary>
        /// A buying payment notification.
        /// </summary>
        public async Task onbuy(WebContext wc)
        {
            var xe = await wc.ReadAsync<XElem>();
            if (!OnNotified(xe, out var trade_no, out var cash))
            {
                wc.Give(400);
                return;
            }
            int pos = 0;
            var orderid = trade_no.ParseInt(ref pos);
            try
            {
                // NOTE: WCPay may send notification more than once
                using var dc = NewDbContext();
                // verify that the ammount is correct
                var today = DateTime.Today;
                dc.Sql("SELECT price FROM orders WHERE id = @1 AND status = 4");
                var price = (decimal) dc.Scalar(p => p.Set(orderid));
                if (price == cash) // update order status and line states
                {
                    dc.Sql("UPDATE orders SET status = 1, pay = @1, issued = @2 WHERE id = @3 AND status = 0");
                    await dc.ExecuteAsync(p => p.Set(cash).Set(today).Set(orderid));
                }
                else // try to refund this payment
                {
                    await PostRefundAsync(trade_no, cash, cash, trade_no);
                }
            }
            finally
            {
                // return xml to WCPay server
                var x = new XmlBuilder(true, 1024);
                x.ELEM("xml", null, () =>
                {
                    x.ELEM("return_code", "SUCCESS");
                    x.ELEM("return_msg", "OK");
                });
                wc.Give(200, x);
            }
        }
    }
}