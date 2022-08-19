using ChainFx.Web;
using ChainVerse.Core;

namespace ChainVerse
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