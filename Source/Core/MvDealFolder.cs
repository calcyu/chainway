using System;
using System.Reflection;
using ChainFx;

namespace ChainVerse.Core
{
    public class MvDealFolder : IFolder<MvDeal>
    {
        static readonly Map<short, MvDealInfo> All = new Map<short, MvDealInfo>();

        static MvDealFolder()
        {
            var typs = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var typ in typs)
            {
                if (typ.Namespace == nameof(Deals) && typ.Name.EndsWith("Proj"))
                {
                    if (Activator.CreateInstance(typ) is MvDeal sec)
                    {
                        // init

                        //add
                        // All.Add(sec);
                    }
                }
            }
        }

        public static MvDeal GetDeal(int dealid)
        {
            return null;
        }

        public MvDeal GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Create(MvDeal ent)
        {
            throw new NotImplementedException();
        }
    }
}