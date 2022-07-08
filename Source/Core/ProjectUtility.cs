using System;
using System.Reflection;
using CoChain;

namespace Urbrural.Core
{
    public class ProjectUtility
    {
        static readonly Map<short, MvProjDescr> All = new Map<short, MvProjDescr>();

        static ProjectUtility()
        {
            var typs = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var typ in typs)
            {
                if (typ.Namespace == nameof(Projects) && typ.Name.EndsWith("Proj"))
                {
                    if (Activator.CreateInstance(typ) is MvProj sec)
                    {
                        // init

                        //add
                        All.Add(sec);
                    }
                }
            }
        }
    }
}