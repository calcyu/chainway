using Chainly;
using Urbrural.Mv;

namespace Urbrural
{
    /// <summary>
    /// An actual contract or order execution.
    /// </summary>
    public class Deal : Info, IKeyable<int>, IMvScope<Project>
    {
        public static readonly Deal Empty = new Deal();

        public const short
            TYP_ONLINE = 1,
            TYP_OFFLINE = 2;


        public static readonly Map<short, string> Typs = new Map<short, string>
        {
            {TYP_ONLINE, "网上"},
            {TYP_OFFLINE, "线下"},
        };


        internal int id;
        internal int orgid;
        internal int uid;
        internal string uname;
        internal string utel;
        internal string uim;
        internal string unit;
        internal decimal price;
        internal decimal qty;
        internal decimal fee;
        internal decimal pay;
        internal JObj log;


        public override void Read(ISource s, short proj = 0xff)
        {
            if ((proj & ID) == ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(orgid), ref orgid);
            s.Get(nameof(uid), ref uid);
            s.Get(nameof(uname), ref uname);
            s.Get(nameof(utel), ref utel);
            s.Get(nameof(uim), ref uim);
            s.Get(nameof(unit), ref unit);
            s.Get(nameof(price), ref price);
            s.Get(nameof(qty), ref qty);
            s.Get(nameof(fee), ref fee);
            s.Get(nameof(pay), ref pay);
        }

        public override void Write(ISink s, short proj = 0xff)
        {
            if ((proj & ID) == ID)
            {
                s.Put(nameof(id), id);
            }

            s.Put(nameof(orgid), orgid);
            s.Put(nameof(uid), uid);
            s.Put(nameof(uname), uname);
            s.Put(nameof(utel), utel);
            s.Put(nameof(uim), uim);
            s.Put(nameof(unit), unit);
            s.Put(nameof(price), price);
            s.Put(nameof(qty), qty);
            s.Put(nameof(fee), fee);
            s.Put(nameof(pay), pay);
        }

        public int Key => id;


        Map<string, Var> variables = new Map<string, Var>();


        public Var GetVar(string varName) => variables[varName];
    }
}