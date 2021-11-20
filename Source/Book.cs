using System;
using SkyChain;

namespace Revital
{
    public class Book : _Doc, IKeyable<int>
    {
        public static readonly Book Empty = new Book();

        public const short
            TYP_PRODUCT = 1,
            TYP_SERVICE = 2,
            TYP_EVENT = 3;

        public static readonly Map<short, string> Typs = new Map<short, string>
        {
            {1, "现货"},
            {2, "预订"},
        };


        internal int id;
        internal short planid;
        internal short itemid;
        internal decimal price;
        internal decimal off;
        internal int qty;
        internal decimal pay;
        internal decimal refund;

        public override void Read(ISource s, byte proj = 15)
        {
            base.Read(s, proj);

            if ((proj & ID) == ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(planid), ref planid);
            s.Get(nameof(itemid), ref itemid);
            s.Get(nameof(price), ref price);
            s.Get(nameof(off), ref off);
            s.Get(nameof(qty), ref qty);
            s.Get(nameof(pay), ref pay);
            s.Get(nameof(refund), ref refund);
        }

        public override void Write(ISink s, byte proj = 15)
        {
            base.Write(s, proj);

            if ((proj & ID) == ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(planid), planid);
            s.Put(nameof(itemid), itemid);
            s.Put(nameof(price), price);
            s.Put(nameof(off), off);
            s.Put(nameof(qty), qty);
            s.Put(nameof(pay), pay);
            s.Put(nameof(refund), refund);
        }

        public int Key => id;

        public bool IsOver(DateTime now) => false;
    }
}