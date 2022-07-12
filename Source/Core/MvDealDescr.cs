using System;
using CoChain;

namespace Urbrural.Core
{
    /// <summary>
    /// The descriptor for a class of metaverse deals.
    /// </summary>
    public class MvDealDescr
    {
        Type dealTyp;


        Map<string, MvConf> confs;

        //
        Map<string, MvObjDescr> objs;

        //
        Map<string, MvTaskDescr> tasks;


        public MvDealDescr(Type dealTyp)
        {
            this.dealTyp = dealTyp;
        }
    }
}