using CoChain.Nodal;
using CoChain.Web;

namespace Urbrural.Core
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