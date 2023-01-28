namespace ChainPort.Core
{
    public interface ILifeCycle
    {
        void OnLoad();

        void OnFullLoad();

        void OnSave();
    }
}