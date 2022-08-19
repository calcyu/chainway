using System;
using ChainFx.Web;

namespace ChainVerse
{
    public class FedService : ChainFx.Nodal.FedService
    {
        public override void dir(WebContext wc)
        {
            throw new NotImplementedException();
        }

        public override void rsc(WebContext wc, int rscid)
        {
            throw new NotImplementedException();
        }
    }
}