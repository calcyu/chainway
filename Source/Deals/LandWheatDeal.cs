using Urbrural.Core;
using Urbrural.Objs;
using Urbrural.Tasks;

namespace Urbrural.Deals
{
    // [Note]
    public class LandWheatDeal : MvDeal
    {
        //
        // mv objects
        //
        MvObj celes = new CelestialObj()
        {
        };

        [Sub]
        MvObj tropo = new TropoObj()
        {
        };

        //
        // tasks 
        //

        MvTask A, A1, B;

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="price"></param>
        /// <param name="final"></param>
        public LandWheatDeal(
            [Info("")] decimal price,
            decimal final
        )
        {
            A = new WechatPayTask()
            {
                Before = (x) => celes.Ext > 0
            };
        }
    }
}