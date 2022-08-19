using ChainFx;

namespace ChainVerse.Core
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