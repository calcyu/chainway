namespace Urbrural.Mv
{
    public interface IMvScope<out P>
    {
        Var GetVar(string varName);
    }
}