﻿using System.Threading.Tasks;
using SkyChain.Web;

namespace Coverse
{
    public abstract class ShopWork : WebWork
    {
    }

    [UserAuthorize(Org.TYP_BIZ, User.ORGLY_OPN)]
    [Ui("商户线下零售", "cloud-download")]
    public class BizlyShopWork : ShopWork
    {
        public async Task @default(WebContext wc)
        {
        }
    }
}