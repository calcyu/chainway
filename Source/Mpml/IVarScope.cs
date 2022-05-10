namespace Urbrural.Mpml
{
    public interface IVarScope<out P>
    {
        Var GetVar(string varName);
    }
}