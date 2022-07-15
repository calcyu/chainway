using System.Threading.Tasks;
using CoChain.Web;

namespace Urbrural.Core
{
    public class MvWork : WebWork
    {
        protected override async Task HandleAsync(string rsc, WebContext wc)
        {

            int dealid = wc[-1];

            var deal = MvDealUtility.GetDeal(dealid);
            
            // prepare



            return ;
        }
    }
}