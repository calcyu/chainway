using System;
using CoChain;

namespace Urbrural.Core
{
    /// <summary>
    /// The descriptor for a class of metaverse deals.
    /// </summary>
    public class MvDealInfo : MvEntityInfo<MvDeal>
    {
        Type dealTyp;


        Map<string, MvConf> confs;

        //
        Map<string, MvObjInfo> objs;

        //
        Map<string, MvTaskInfo> tasks;


        public MvDealInfo(Type dealTyp)
        {
            this.dealTyp = dealTyp;
        }
    }
}