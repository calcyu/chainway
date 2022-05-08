namespace Urbrural.Mv
{
    public interface IMvScope<out P>
    {
        Variable GetVariable(string varName);
    }
}