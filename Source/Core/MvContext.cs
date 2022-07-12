using CoChain.Nodal;
using CoChain.Web;

namespace Urbrural.Core
{
    /// <summary>
    /// The scripting global states.
    /// </summary>
    public class MvContext
    {
        Cat _class;

        Reg _reg;

        WebContext webctx;

        DbContext dbctx;

        IUser user;

        public IUser User => user;
    }
}