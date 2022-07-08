using System;
using CoChain.Web;

namespace Urbrural
{
    public class FedService : CoChain.Nodal.FedService
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