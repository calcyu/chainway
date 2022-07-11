using CoChain;

namespace Urbrural.Core
{
    /// <summary>
    /// A project object model.
    /// </summary>
    public class Project : Entity, IKeyable<int>
    {
        public static readonly Project Empty = new Project();

        internal int id;
        internal int orgid;
        internal short regid;
        double x;
        double y;

        public override void Read(ISource s, short msk = 0xff)
        {
            base.Read(s, msk);

            if ((msk & ID) == ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(orgid), ref orgid);
            s.Get(nameof(regid), ref regid);
            s.Get(nameof(x), ref x);
            s.Get(nameof(y), ref y);
        }

        public override void Write(ISink s, short msk = 0xff)
        {
            base.Write(s, msk);

            if ((msk & ID) == ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(orgid),  orgid);
            s.Put(nameof(regid),  regid);
            s.Put(nameof(x),  x);
            s.Put(nameof(y),  y);
        }

        public int Key => id;

        public override string ToString() => name;
    }
}