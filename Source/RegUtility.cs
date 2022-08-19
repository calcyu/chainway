using ChainFx;
using ChainFx.Nodal;

namespace ChainVerse.Core
{
    public class RegUtility
    {
        static readonly Map<short, MvScene> All;

        static RegUtility()
        {
            using var dc = Store.NewDbContext();

            dc.Sql("SELECT ").collst(MvScene.Empty).T(" FROM regs WHERE state > 0");
            All = dc.Query<short, MvScene>();
        }
    }
}