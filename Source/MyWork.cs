using SkyChain.Web;

namespace Zhnt.Supply
{
    public class MyWork : WebWork
    {
        protected override void OnMake()
        {
            MakeVarWork<MyVarWork>(x => ((User) x).id);
        }
    }
}