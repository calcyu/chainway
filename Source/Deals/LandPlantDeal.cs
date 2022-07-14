using Urbrural.Core;
using Urbrural.Obj;
using Urbrural.Tasks;

namespace Urbrural.Deals
{
    // [Note]
    public class LandPlantDeal : MvDeal
    {
        //
        // mv objects
        //

        readonly MvObj celes = new CelestialObj()
        {
        };

        [Note(nameof(celes))] MvObj tropo = new TropoObj()
        {
        };

        //
        // confs
        //

        public decimal Pay => conf[nameof(Pay)];

        public short Level => conf[nameof(Level)];


        //
        // tasks 
        //

        [Note(nameof(celes))] MvTask A,
            A1,
            B;

        public LandPlantDeal()
        {
            A = new WechatPayTask()
            {
                Before = (x) => celes.Ext > 0
            };
        }
    }
}