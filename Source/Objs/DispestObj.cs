using Urbrural.Core;

namespace Urbrural.Objs
{
    [Info("病虫害")]
    public class DispestObj : MvObj
    {
    }

    [Info("条锈病")]
    public class DispestStripeRustObj : DispestObj
    {
    }

    [Info("蚜虫")]
    public class DispestAphidObj : DispestObj
    {
    }

    [Info("红蜘蛛")]
    public class DispestSpiderObj : DispestObj
    {
    }
}