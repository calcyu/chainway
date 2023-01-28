using ChainFx;

namespace ChainPort
{
    /// <summary>
    /// The data model for an organizational unit.
    /// </summary>
    public class Org : Entity, IKeyable<int>
    {
        public static readonly Org Empty = new Org();

        public const short
            TYP_COL = 1, // collective
            TYP_PRI = 2, // private
            TYP_STA = 3; // state-owned

        public static readonly Map<short, string> Typs = new Map<short, string>
        {
            {TYP_COL, "集体"},
            {TYP_PRI, "民营"},
            {TYP_STA, "国有"},
        };

        public static readonly Map<short, string> Forks = new Map<short, string>
        {
        };

        // id
        internal int id;

        internal short fork;
        internal string license;
        internal short regid;
        internal string addr;
        internal double x;
        internal double y;

        internal string tel;

        internal int mgrid;
        internal string mgrname;
        internal string mgrtel;
        internal string mgrim;
        internal bool img;

        public override void Read(ISource s, short msk = 0xff)
        {
            base.Read(s, msk);

            if ((msk & MSK_ID) == MSK_ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(fork), ref fork);
            s.Get(nameof(license), ref license);
            s.Get(nameof(regid), ref regid);
            s.Get(nameof(addr), ref addr);
            s.Get(nameof(x), ref x);
            s.Get(nameof(y), ref y);
            s.Get(nameof(tel), ref tel);
            if ((msk & MSK_LATER) == MSK_LATER)
            {
                s.Get(nameof(mgrid), ref mgrid);
                s.Get(nameof(mgrname), ref mgrname);
                s.Get(nameof(mgrtel), ref mgrtel);
                s.Get(nameof(mgrim), ref mgrim);
                s.Get(nameof(img), ref img);
            }
        }

        public override void Write(ISink s, short msk = 0xff)
        {
            base.Write(s, msk);

            if ((msk & MSK_ID) == MSK_ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(fork), fork);
            s.Put(nameof(license), license);
            if (regid > 0) s.Put(nameof(regid), regid);
            else s.PutNull(nameof(regid));
            s.Put(nameof(addr), addr);
            s.Put(nameof(x), x);
            s.Put(nameof(y), y);
            s.Put(nameof(tel), tel);
            if ((msk & MSK_LATER) == MSK_LATER)
            {
                s.Put(nameof(mgrid), mgrid);
                s.Put(nameof(mgrname), mgrname);
                s.Put(nameof(mgrtel), mgrtel);
                s.Put(nameof(mgrim), mgrim);
                s.Put(nameof(img), img);
            }
        }

        #region Properties

        public int Key => id;

        public short Fork => fork;

        public string Tel => tel;

        public string Im => mgrim;

        public override string ToString() => name;

        #endregion
    }
}