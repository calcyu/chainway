using System.Threading.Tasks;
using ChainFx.Web;

namespace ChainVerse.Core
{
    public class MvWork : WebWork
    {
        protected override async Task HandleAsync(string rsc, WebContext wc)
        {

            int dealid = wc[-1];

            var deal = MvDealFolder.GetDeal(dealid);
            
            // prepare



            return ;
        }
    }
}