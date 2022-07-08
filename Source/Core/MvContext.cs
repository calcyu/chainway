namespace Urbrural.Core
{
    /// <summary>
    /// The scripting global states.
    /// </summary>
    public abstract class MvContext
    {
        MvScene _class;
        
        Reg _reg;

        protected MvProj project;

        protected Deal deal;


        public string CurStage => deal.Stage;

        public string CurJob => deal.job;


        IUser user;

        public IUser User => user;
    }
}