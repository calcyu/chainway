using ChainFx;
using ChainPort.Core;
using ChainPort.Library;
using ChainPort.Objs;

namespace ChainPort.Scenes
{
    /// <summary>
    /// A geographic region that includes projects.
    /// </summary>
    public class RegionScene : MvScene, IKeyable<short>
    {
        MvObj celes = new CelestialObj()
        {
        };

        [Sub]
        MvObj tropo = new TropoObj()
        {
        };
    }
}