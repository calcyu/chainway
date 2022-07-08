using CoChain.Web;

namespace Urbrural
{
    public class ClassWork : WebWork
    {
    }

    [UserAuthorize(admly: User.ADMLY_MGT)]
    [Ui("平台体系设置", "world")]
    public class AdmlyClassWork : ClassWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<AdmlyClassVarWork>();
        }

        public void @default(WebContext wc)
        {
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                h.GRID(ClassUtility.All, o =>
                    {
                        h.DIV_("uk-card-default");
                        h.TOOLGROUPVAR(o.Key)._NAV();
                        h._DIV();
                    }
                );
            });
        }
    }

    public class OrglyClassWork : ClassWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<OrglyClassVarWork>();
        }


        public void @default(WebContext wc)
        {
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                h.GRID(ClassUtility.All, o =>
                    {
                        h.DIV_("uk-card-default");
                        h.TOOLGROUPVAR(o.Key)._NAV();
                        h._DIV();
                    }
                );
            });
        }
    }
}