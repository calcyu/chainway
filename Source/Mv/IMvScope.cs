namespace Urbrural.Mv
{
    public interface IMvScope<out P>
    {
        P ParentScope { get; }
    }
}