using ChainFx;
using ChainVerse.Core;

namespace ChainVerse.Objs
{
    public class CelestialObj : MvObj
    {
        
        static readonly MvObjInfo Info = new MvObjInfo(); 
        
        public const short 
            STA_A = 1;
        
        static Map<short, string> States = new Map<short,string>()
        {
            
        };
    }
}