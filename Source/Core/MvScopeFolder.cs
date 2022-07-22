using CoChain;

namespace Urbrural.Core
{
    public class MvScopeFolder : IFolder<MvScene>
    {
        public Map<string, MvScene> All;


        public MvScene GetScope(string name)
        {
            return All[name];
        }

        public MvScene GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Create(MvScene ent)
        {
            throw new System.NotImplementedException();
        }
    }
}