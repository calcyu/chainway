using System.Threading.Tasks;
using ChainFx.Web;

namespace ChainPort.Core
{
    public class MvWork : WebWork
    {
        protected async Task HandleAsync(string rsc, WebContext wc)
        {
            int dealid = wc[-1];

            var deal = MvDealFolder.GetDeal(dealid);

            // prepare


            return;
        }
    }
}