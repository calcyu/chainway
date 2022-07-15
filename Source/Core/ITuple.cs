using CoChain;

namespace Urbrural.Core
{
    public interface ITuple : IKeyable<string>
    {
        MvDeal Parent { get; }

        short State { get; }

        decimal Value { get; }

        short Cent { get; }

        decimal Ext { get; }
    }
}