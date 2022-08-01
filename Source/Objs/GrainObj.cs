using Urbrural.Core;

namespace Urbrural.Library
{
    [Info("谷物")]
    public class GrainObj : MvObj
    {
    }

    [Info("小麦")]
    public class WheatObj : GrainObj
    {
    }

    [Info("大米")]
    public class RiceObj : GrainObj
    {
    }

    [Info("小米")]
    public class MilletObj : GrainObj
    {
    }
}