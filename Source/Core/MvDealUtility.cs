using System;
using System.Reflection;
using CoChain;

namespace Urbrural.Core
{
    public class MvDealUtility
    {
        static readonly Map<short, MvDealDescr> All = new Map<short, MvDealDescr>();

        static MvDealUtility()
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
    }
}