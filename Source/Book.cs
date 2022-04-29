using System;
using Chainly;

namespace Urbrural
{
    public class Book : Info, IKeyable<long>
    {
        public static readonly Book Empty = new Book();

        public const short
            TYP_SPOT = 1,
            TYP_PRESALE = 2;


        public static readonly Map<short, string> Typs = new Map<short, string>
        {
            {TYP_SPOT, "现货"},
            {TYP_PRESALE, "预售"},
        };


        internal long id;
        internal int bizid;
        internal string bizname;
        internal int mrtid;
        internal string mrtname;
        internal int ctrid;
        internal string ctrname;
        internal int prvid;
        internal string prvname;
        internal int srcid;
        internal string srcname;

        internal int wareid;
        internal string warename;
        internal short itemid;
        internal decimal price;
        internal short qty;
        internal decimal pay;

        internal Act[] trace;


        public override void Read(ISource s, short proj = 0xff)
        {
            base.Read(s, proj);

            if ((proj & ID) == ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(bizid), ref bizid);
            s.Get(nameof(bizname), ref bizname);
            s.Get(nameof(mrtid), ref mrtid);
            s.Get(nameof(mrtname), ref mrtname);
            s.Get(nameof(ctrid), ref ctrid);
            s.Get(nameof(ctrname), ref ctrname);
            s.Get(nameof(prvid), ref prvid);
            s.Get(nameof(prvname), ref prvname);
            s.Get(nameof(srcid), ref srcid);
            s.Get(nameof(srcname), ref srcname);

            s.Get(nameof(wareid), ref wareid);
            s.Get(nameof(warename), ref warename);
            s.Get(nameof(itemid), ref itemid);
            s.Get(nameof(price), ref price);
            s.Get(nameof(qty), ref qty);
            s.Get(nameof(pay), ref pay);

            s.Get(nameof(trace), ref trace);
        }

        public override void Write(ISink s, short proj = 0xff)
        {
            base.Write(s, proj);

            if ((proj & ID) == ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(bizid), bizid);
            s.Put(nameof(bizname), bizname);
            s.Put(nameof(mrtid), mrtid);
            s.Put(nameof(mrtname), mrtname);
            s.Put(nameof(ctrid), ctrid);
            s.Put(nameof(ctrname), ctrname);
            s.Put(nameof(prvid), prvid);
            s.Put(nameof(prvname), prvname);
            s.Put(nameof(srcid), srcid);
            s.Put(nameof(srcname), srcname);

            s.Put(nameof(wareid), wareid);
            s.Put(nameof(warename), warename);
            s.Put(nameof(itemid), itemid);
            s.Put(nameof(price), price);
            s.Put(nameof(qty), qty);
        }

        public long Key => id;

        public bool IsOver(DateTime now) => false;
    }
}