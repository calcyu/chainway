﻿using System;

namespace ChainPort.Core
{
    public class MvObj : MvTuple, IEquatable<MvObj>, IComparable<MvObj>, IVisual
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