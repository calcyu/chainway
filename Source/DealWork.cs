using CoChain.Web;
using Urbrural.Core;

namespace Urbrural
{
    public class DealWork : WebWork
    {
    }

    public class OrglyDealWork : DealWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<MvWork>();
        }

        public void @default(WebContext wc)
        {
        }
    }
}