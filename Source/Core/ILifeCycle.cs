namespace ChainVerse.Core
{
    public interface ILifeCycle
    {
        void OnLoad();

        void OnFullLoad();

        void OnSave();
    }
}