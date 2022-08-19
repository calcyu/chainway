using ChainFx;
using ChainFx.Web;
using static ChainFx.Nodal.Store;

namespace ChainVerse
{
    [Ui("机构主体操作")]
    public class OrglyWork : WebWork
    {
        protected override void OnCreate()
        {
            // id of either current user or the specified
            CreateVarWork<OrglyVarWork>((prin, key) =>
                {
                    var orgid = key?.ToInt() ?? ((User) prin).orgid;
                    return GrabObject<int, Org>(orgid);
                }
            );
        }
    }
}