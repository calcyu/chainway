using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SkyChain;
using SkyChain.Web;
using static SkyChain.CryptoUtility;
using static SkyChain.Application;
using WebUtility = System.Net.WebUtility;

namespace Revital
{
    /// <summary>
    /// A hub of operation that has its own weixin official acount.
    /// </summary>
    public static class WeChatUtility
    {
        static readonly WebClient Api = new WebClient("https://api.weixin.qq.com");

        static readonly WebClient PayApi;

        public static readonly string
            url,
            appid,
            appsecret;

        public static readonly string
            mchid,
            noncestr,
            spbillcreateip;

        public static readonly string key;
        public static readonly string watchurl;


        static WeChatUtility()
        {
            var s = extcfg;
            s.Get(nameof(url), ref url);
            s.Get(nameof(appid), ref appid);
            s.Get(nameof(appsecret), ref appsecret);
            s.Get(nameof(mchid), ref mchid);
            s.Get(nameof(noncestr), ref noncestr);
            s.Get(nameof(spbillcreateip), ref spbillcreateip);
            s.Get(nameof(key), ref key);
            s.Get(nameof(watchurl), ref watchurl);

            try
            {
                // load and init client certificate
                var handler = new WebClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual
                };
                var cert = new X509Certificate2("apiclient_cert.p12", mchid, X509KeyStorageFlags.MachineKeySet);
                handler.ClientCertificates.Add(cert);
                PayApi = new WebClient("https://api.mch.weixin.qq.com", handler);
            }
            catch (Exception e)
            {
                WAR("failed to load certificate: " + e.Message);
            }
        }


        static string accessToken;

        static int tick;

        public static string GetAccessToken()
        {
            int now = Environment.TickCount;
            if (accessToken == null || now < tick || now - tick > 3600000)
            {
                var (_, jo) = Api.GetAsync<JObj>("/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret, null).Result;
                string access_token = jo?[nameof(access_token)];
                accessToken = access_token;
                tick = now;
            }

            return accessToken;
        }

        public static void GiveRedirectWeiXinAuthorize(WebContext wc, string listenAddr, bool userinfo = false)
        {
            string redirect_url = WebUtility.UrlEncode(listenAddr + wc.Uri);
            wc.SetHeader(
                "Location",
                "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + redirect_url + "&response_type=code&scope=" + (userinfo ? "snsapi_userinfo" : "snsapi_base") + "&state=wxauth#wechat_redirect"
            );
            wc.Give(303);
        }

        public static async Task<(string access_token, string openid)> GetAccessorAsync(string code)
        {
            string url = "/sns/oauth2/access_token?appid=" + appid + "&secret=" + appsecret + "&code=" + code + "&grant_type=authorization_code";
            var (_, jo) = await Api.GetAsync<JObj>(url, null);
            if (jo == null)
            {
                return default((string, string));
            }

            string access_token = jo[nameof(access_token)];
            if (access_token == null)
            {
                return default((string, string));
            }

            string openid = jo[nameof(openid)];
            return (access_token, openid);
        }

        public static async Task<User> GetUserInfoAsync(string access_token, string openid)
        {
            var (_, jo) = await Api.GetAsync<JObj>("/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN", null);
            string nickname = jo[nameof(nickname)];
            return new User {im = openid, name = nickname};
        }


        static readonly DateTime EPOCH = new DateTime(1970, 1, 1);

        public static long NowMillis => (long) (DateTime.Now - EPOCH).TotalMilliseconds;

        public static IContent BuildPrepayContent(string prepay_id)
        {
            string package = "prepay_id=" + prepay_id;
            string timeStamp = ((int) (DateTime.Now - EPOCH).TotalSeconds).ToString();
            var jo = new JObj
            {
                {"appId", appid},
                {"nonceStr", noncestr},
                {"package", package},
                {"signType", "MD5"},
                {"timeStamp", timeStamp}
            };
            jo.Add("paySign", Sign(jo, "paySign"));
            return jo.Dump();
        }

        public static async Task<(string ticket, string url)> PostQrSceneAsync(int uid)
        {
            var j = new JsonContent(true, 1024);
            try
            {
                j.OBJ_();
                j.Put("expire_seconds", 604800);
                j.Put("action_name", "QR_SCENE");
                j.OBJ_("action_info");
                j.OBJ_("scene");
                j.Put("scene_id", uid);
                j._OBJ();
                j._OBJ();
                j._OBJ();
                var (_, jo) =
                    await Api.PostAsync<JObj>("/cgi-bin/qrcode/create?access_token=" + GetAccessToken(), j);
                return (jo["ticket"], jo["url"]);
            }
            finally
            {
                j.Clear();
            }
        }

        public static async Task PostSendAsync(string openid, string text)
        {
            var j = new JsonContent(true, 1024);
            try
            {
                j.OBJ_();
                j.Put("touser", openid);
                j.Put("msgtype", "text");
                j.OBJ_("text");
                j.Put("content", text);
                j._OBJ();
                j._OBJ();
                await Api.PostAsync<JObj>("/cgi-bin/message/custom/send?access_token=" + GetAccessToken(), j);
            }
            finally
            {
                j.Clear();
            }
        }

        public static async Task PostSendAsync(string openid, string title, string descr, string url, string picurl = null)
        {
            var j = new JsonContent(true, 1024);
            try
            {
                j.OBJ_();
                j.Put("touser", openid);
                j.Put("msgtype", "news");
                j.OBJ_("news").ARR_("articles").OBJ_();
                j.Put("title", title);
                j.Put("description", descr);
                j.Put("url", url);
                j.Put("picurl", picurl);
                j._OBJ()._ARR()._OBJ();
                j._OBJ();
                await Api.PostAsync<JObj>("/cgi-bin/message/custom/send?access_token=" + GetAccessToken(), j);
            }
            finally
            {
                j.Clear();
            }
        }

        public static async Task<string> PostTransferAsync(int id, string openid, string username, decimal cash, string desc)
        {
            var x = new XElem("xml")
            {
                {"amount", ((int) (cash * 100)).ToString()},
                {"check_name", "FORCE_CHECK"},
                {"desc", desc},
                {"mch_appid", appid},
                {"mchid", mchid},
                {"nonce_str", noncestr},
                {"openid", openid},
                {"partner_trade_no", id.ToString()},
                {"re_user_name", username},
                {"spbill_create_ip", spbillcreateip}
            };
            string sign = Sign(x);
            x.Add("sign", sign);

            var (_, xe) = (await PayApi.PostAsync<XElem>("/mmpaymkttransfers/promotion/transfers", x.Dump()));
            string return_code = xe.Child(nameof(return_code));
            if ("SUCCESS" == return_code)
            {
                string result_code = xe.Child(nameof(result_code));
                if ("SUCCESS" != result_code)
                {
                    string err_code_des = xe.Child(nameof(err_code_des));
                    return err_code_des;
                }
            }
            else
            {
                string return_msg = xe.Child(nameof(return_msg));
                return return_msg;
            }

            return null;
        }

        public static async Task<(string, string)> PostUnifiedOrderAsync(string trade_no, decimal amount, string openid, string ip, string notifyurl, string descr)
        {
            var x = new XElem("xml")
            {
                {"appid", appid},
                {"body", descr},
                {"detail", descr},
                {"mch_id", mchid},
                {"nonce_str", noncestr},
                {"notify_url", notifyurl},
                {"openid", openid},
                {"out_trade_no", trade_no},
                {"spbill_create_ip", ip},
                {"total_fee", ((int) (amount * 100)).ToString()},
                {"trade_type", "JSAPI"}
            };
            string sign = Sign(x);
            x.Add("sign", sign);

            var (_, xe) = (await PayApi.PostAsync<XElem>("/pay/unifiedorder", x.Dump()));
            string prepay_id = xe.Child(nameof(prepay_id));
            string err_code = null;
            if (prepay_id == null)
            {
                err_code = xe.Child("err_code");
            }

            return (prepay_id, err_code);
        }

        public static bool OnNotified(XElem xe, out string out_trade_no, out decimal total)
        {
            total = 0;
            out_trade_no = null;
            string appid = xe.Child(nameof(appid));
            string mch_id = xe.Child(nameof(mch_id));
            string nonce_str = xe.Child(nameof(nonce_str));

            if (appid != WeChatUtility.appid || mch_id != mchid || nonce_str != noncestr) return false;

            string result_code = xe.Child(nameof(result_code));

            if (result_code != "SUCCESS") return false;

            string sign = xe.Child(nameof(sign));
            xe.Sort();
            if (sign != Sign(xe, "sign")) return false;

            int total_fee = xe.Child(nameof(total_fee)); // in cent
            total = ((decimal) total_fee) / 100;
            out_trade_no = xe.Child(nameof(out_trade_no)); // 商户订单号
            return true;
        }

        public static async Task<decimal> PostOrderQueryAsync(string orderno)
        {
            var x = new XElem("xml")
            {
                {"appid", appid},
                {"mch_id", mchid},
                {"nonce_str", noncestr},
                {"out_trade_no", orderno}
            };
            string sign = Sign(x);
            x.Add("sign", sign);
            var (_, xe) = (await PayApi.PostAsync<XElem>("/pay/orderquery", x.Dump()));
            sign = xe.Child(nameof(sign));
            xe.Sort();
            if (sign != Sign(xe, "sign")) return 0;

            string return_code = xe.Child(nameof(return_code));
            if (return_code != "SUCCESS") return 0;

            decimal cash_fee = xe.Child(nameof(cash_fee));

            return cash_fee;
        }

        public static async Task<string> PostRefundAsync(string orderno, decimal total, decimal refund, string refoundno, string descr = null)
        {
            var x = new XElem("xml")
            {
                {"appid", appid},
                {"mch_id", mchid},
                {"nonce_str", noncestr},
                {"op_user_id", mchid},
                {"out_refund_no", refoundno},
                {"out_trade_no", orderno},
                {"refund_desc", descr},
                {"refund_fee", ((int) (refund * 100)).ToString()},
                {"total_fee", ((int) (total * 100)).ToString()}
            };
            string sign = Sign(x);
            x.Add("sign", sign);

            var (_, xe) = (await PayApi.PostAsync<XElem>("/secapi/pay/refund", x.Dump()));
            if (xe == null)
            {
                return "TIMEOUT";
            }
            string return_code = xe.Child(nameof(return_code));
            if (return_code != "SUCCESS")
            {
                string return_msg = xe.Child(nameof(return_msg));
                return return_msg;
            }

            string result_code = xe.Child(nameof(result_code));
            if (result_code != "SUCCESS")
            {
                string err_code_des = xe.Child(nameof(err_code_des));
                return err_code_des;
            }

            return null;
        }

        public static async Task<string> PostRefundQueryAsync(long orderid)
        {
            var x = new XElem("xml")
            {
                {"appid", appid},
                {"mch_id", mchid},
                {"nonce_str", noncestr},
                {"out_trade_no", orderid.ToString()}
            };
            string sign = Sign(x);
            x.Add("sign", sign);
            var (_, xe) = (await PayApi.PostAsync<XElem>("/pay/refundquery", x.Dump()));

            sign = xe.Child(nameof(sign));
            xe.Sort();
            if (sign != Sign(xe, "sign")) return "返回结果签名错误";

            string return_code = xe.Child(nameof(return_code));
            if (return_code != "SUCCESS")
            {
                string return_msg = xe.Child(nameof(return_msg));
                return return_msg;
            }

            string result_code = xe.Child(nameof(result_code));
            if (result_code != "SUCCESS")
            {
                return "退款订单查询失败";
            }

            string refund_status_0 = xe.Child(nameof(refund_status_0));
            if (refund_status_0 != "SUCCESS")
            {
                return refund_status_0 == "PROCESSING" ? "退款处理中" :
                    refund_status_0 == "REFUNDCLOSE" ? "退款关闭" : "退款异常";
            }

            return null;
        }

        static string Sign(XElem xe, string exclude = null)
        {
            var sb = new StringBuilder(1024);
            for (int i = 0; i < xe.Count; i++)
            {
                var child = xe[i];
                // not include the sign field
                if (exclude != null && child.Tag == exclude) continue;
                if (sb.Length > 0)
                {
                    sb.Append('&');
                }

                sb.Append(child.Tag).Append('=').Append(child.Text);
            }

            sb.Append("&key=").Append(key);
            return MD5(sb.ToString(), true);
        }

        static string Sign(JObj jo, string exclude = null)
        {
            var sb = new StringBuilder(1024);
            for (int i = 0; i < jo.Count; i++)
            {
                var mbr = jo.ValueAt(i);
                // not include the sign field
                if (exclude != null && mbr.Key == exclude) continue;
                if (sb.Length > 0)
                {
                    sb.Append('&');
                }

                sb.Append(mbr.Key).Append('=').Append((string) mbr);
            }

            sb.Append("&key=").Append(key);
            return MD5(sb.ToString(), true);
        }
    }
}