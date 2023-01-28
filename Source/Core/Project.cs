using ChainFx;

namespace ChainPort.Core
{
    /// <summary>
    /// A project object model.
    /// </summary>
    public class Project : Entity, IKeyable<int>
    {
        public static readonly Project Empty = new Project();

        internal int id;
        internal int orgid;
        internal short scopeid;
        double x;
        double y;
        
        string unit;
        short min;
        short max;
        short step;
        internal decimal price;
        internal decimal qty;
        internal decimal fee;
        internal decimal pay;


        // counterpart to the conf in deal
        JObj conf;


        public override void Read(ISource s, short msk = 0xff)
        {
            base.Read(s, msk);

            if ((msk & MSK_ID) == MSK_ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(orgid), ref orgid);
            s.Get(nameof(scopeid), ref scopeid);
            s.Get(nameof(x), ref x);
            s.Get(nameof(y), ref y);
            
            s.Get(nameof(unit), ref unit);
            s.Get(nameof(min), ref min);
            s.Get(nameof(max), ref max);
            s.Get(nameof(step), ref step);
            s.Get(nameof(price), ref price);
            s.Get(nameof(qty), ref qty);
            s.Get(nameof(fee), ref fee);
            s.Get(nameof(pay), ref pay);

            s.Get(nameof(conf), ref conf);

        }

        public override void Write(ISink s, short msk = 0xff)
        {
            base.Write(s, msk);

            if ((msk & MSK_ID) == MSK_ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(orgid), orgid);
            s.Put(nameof(scopeid), scopeid);
            s.Put(nameof(x), x);
            s.Put(nameof(y), y);
            
            s.Put(nameof(unit), unit);
            s.Put(nameof(min), min);
            s.Put(nameof(max), max);
            s.Put(nameof(step), step);
            s.Put(nameof(price), price);
            s.Put(nameof(qty), qty);
            s.Put(nameof(fee), fee);
            s.Put(nameof(pay), pay);

            s.Put(nameof(conf), conf);

        }

        public int Key => id;

        public override string ToString() => name;
    }
}