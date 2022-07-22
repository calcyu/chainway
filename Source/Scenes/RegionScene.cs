using CoChain;
using Urbrural.Core;
using Urbrural.Objs;

namespace Urbrural.Scenes
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