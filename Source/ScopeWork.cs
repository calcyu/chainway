using CoChain.Web;

namespace Urbrural
{
    public class ScopeWork : WebWork
    {
    }

    [UserAuthorize(admly: User.ADMLY_MGT)]
    [Ui("平台体系设置", "world")]
    public class AdmlyScopeWork : ScopeWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<AdmlyScopeVarWork>();
        }

        public void @default(WebContext wc)
        {
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                // h.GRID(CatUtility.All, o =>
                //     {
                //         h.DIV_("uk-card-default");
                //         // h.TOOLGROUPVAR(o.Key)._NAV();
                //         h._DIV();
                //     }
                // );
            });
        }
    }
}