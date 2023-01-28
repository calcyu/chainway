using ChainFx.Web;
using ChainPort.Core;

namespace ChainPort
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