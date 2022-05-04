using System.Threading.Tasks;
using Chainly.Web;
using static Chainly.Nodal.Store;

namespace Urbrural
{
    public class ProjectVarWork : WebWork
    {
    }

    /// 
    /// post
    /// 
    public class PublyProjectVarWork : ProjectVarWork
    {
        public async Task @default(WebContext wc)
        {
            int id = wc[0];
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Project.Empty).T(" FROM posts WHERE id = @1");
            var o = await dc.QueryTopAsync<Project>(p => p.Set(id));
            wc.GivePage(200, h =>
            {
                // org

                // item

                // buy
            });
        }

        public async Task buy(WebContext wc)
        {
        }
    }

    public class OrglyProjectVarWork : ProjectVarWork
    {
        public async Task @default(WebContext wc)
        {
            int id = wc[0];
            var org = wc[-2].As<Org>();
            var prin = (User) wc.Principal;
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
            }
            else // POST
            {
                wc.GivePane(200); // close dialog
            }
        }
    }
}