using CoChain;
using CoChain.Nodal;

namespace Urbrural.Core
{
    public class RegUtility
    {
        static readonly Map<short, Reg> All;

        static RegUtility()
        {
            using var dc = Store.NewDbContext();

            dc.Sql("SELECT ").collst(Reg.Empty).T(" FROM regs WHERE state > 0");
            All = dc.Query<short, Reg>();
        }
    }
}