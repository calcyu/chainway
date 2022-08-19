using ChainFx;
using ChainVerse.Core;
using ChainVerse.Library;
using ChainVerse.Objs;

namespace ChainVerse.Scenes
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