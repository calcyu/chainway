﻿using ChainFx.Fabric;
using ChainFx.Web;

namespace ChainPort.Core
{
    /// <summary>
    /// The scripting global states.
    /// </summary>
    public class MvContext
    {
        MvScene _class;

        MvScene _reg;

        WebContext webctx;

        DbContext dbctx;

        IUser user;

        public IUser User => user;

        public WebContext Web => webctx;
    }
}