﻿using System.Threading.Tasks;
using SkyChain.Web;

namespace Coverse
{
    public abstract class ClearVarWork : WebWork
    {
    }

    public class AdmlyClearVarWork : ClearVarWork
    {
    }

    public class OrglyClearVarWork : ClearVarWork
    {
        [Ui("￥", "微信领款"), Tool(Modal.ButtonShow)]
        public async Task rcv(WebContext wc, int dt)
        {
            int orderid = wc[0];
            if (wc.IsGet)
            {
            }
            else // POST
            {
                wc.GivePane(200); // close
            }
        }
    }
}