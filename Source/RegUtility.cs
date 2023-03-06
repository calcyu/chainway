using ChainFx;
using ChainFx.Nodal;

namespace ChainPort.Core
{
    public class RegUtility
    {
        static readonly Map<short, MvScene> All;

        static RegUtility()
        {
            using var dc = Nodality.NewDbContext();

            dc.Sql("SELECT ").collst(MvScene.Empty).T(" FROM regs WHERE state > 0");
            All = dc.Query<short, MvScene>();
        }
    }
}