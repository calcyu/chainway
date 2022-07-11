using System.Threading.Tasks;
using CoChain.Web;
using Urbrural.Core;
using static Urbrural.User;
using static CoChain.Nodal.Store;

namespace Urbrural
{
    public abstract class CreditWork : WebWork
    {
    }


    public class MyCreditWork : CreditWork
    {
        protected override void OnCreate()
        {
        }

        public async Task @default(WebContext wc, int page)
        {
            var prin = (User) wc.Principal;
            using var dc = NewDbContext();
        }
    }

    [Ui("平台零售电商报告", "table")]
    public class AdmlyCreditWork : CreditWork
    {
        public async Task @default(WebContext wc)
        {
            wc.GivePage(200, h => { h.TOOLBAR(); });
        }
    }

    [UserAuthorize(orgly: ORGLY_OPN)]
    [Ui("商户线上零售", "cloud-upload")]
    public class OrglyCreditWork : CreditWork
    {
        protected override void OnCreate()
        {
        }

        public async Task @default(WebContext wc)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
        }

        public async Task closed(WebContext wc)
        {
            short orgid = wc[-1];
            using var dc = NewDbContext();
        }
    }
}