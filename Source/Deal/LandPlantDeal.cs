using Urbrural.Core;
using Urbrural.Objs;
using Urbrural.Tasks;

namespace Urbrural.Deal
{
    public class LandPlantDeal : MvDeal
    {
        //
        // declare mv objects

        public readonly MvObj celes = new CelestialObj()
        {
        };

        public readonly MvObj tropo = new TropoObj()
        {
        };

        //
        // tasks

        public static readonly MvJob pay = new WechatPayTask()
        {
        };

        public static readonly MvJob pay1 = new WechatPayTask()
        {
            // Parent = pay,
            // OnEnter = () => celes.Qty == 0
        };
    }
}