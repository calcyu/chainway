namespace Urbrural.Core
{
    /// <summary>
    /// The scripting global states.
    /// </summary>
    public abstract class MvContext
    {
        Cat _class;
        
        Reg _reg;

        protected MvDeal project;


        IUser user;

        public IUser User => user;
    }
}