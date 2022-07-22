using System;

namespace Urbrural.Core
{
    public class MvObj : MvTuple, IEquatable<MvObj>, IComparable<MvObj>
    {
        MvDeal parent;


        public virtual void Render(MvContext mc)
        {
            var web = mc.Web;
        }

        public virtual void Control(MvContext mc)
        {
        }
    }
}