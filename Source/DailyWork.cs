using ChainFx.Web;

namespace ChainVerse
{
    public abstract class DailyWork : WebWork
    {
    }

    [UserAuthorize(admly: User.ADMLY_)]
    [Ui("平台业务日报")]
    public class AdmlyDailyWork : DailyWork
    {
        public void @default(WebContext wc, int page)
        {
            wc.GivePage(200, h => { h.TOOLBAR(); });
        }
    }
}