namespace ChainPort.Core
{
    public interface IFolder<E> where E : MvScope
    {
        E GetById(int id);

        void Create(E ent);
    }
}