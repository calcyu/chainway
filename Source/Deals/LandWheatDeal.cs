using ChainVerse.Core;
using ChainVerse.Library;
using ChainVerse.Tasks;

namespace ChainVerse.Deals
{
    // [Note]
    public class LandWheatDeal : MvDeal
    {
        //
        // mv objects
        //
        MvObj wheat = new GrainObj()
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
                Before = (x) => wheat.Ext > 0
            };
        }
    }
}