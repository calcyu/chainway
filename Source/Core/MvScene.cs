using ChainFx;

namespace ChainPort.Core
{
    /// <summary>
    /// A predefined shared scope.
    /// </summary>
    public class MvScene : MvScope
    {
        public static readonly MvScene Empty = new MvScene();

        public const short
            TYP_PROV = 1,
            TYP_DIST = 2,
            TYP_SECT = 3;

        public static readonly Map<short, string> Typs = new Map<short, string>
        {
            {TYP_PROV, "省份"},
            {TYP_DIST, "地区"},
            {TYP_SECT, "场地"},
        };

        internal short id;
        internal short idx;


        public short Background { get; set; }

        public override void Read(ISource s, short msk = 0xff)
        {
            base.Read(s, msk);

            if ((msk & MSK_ID) == MSK_ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(idx), ref idx);
        }

        public override void Write(ISink s, short msk = 0xff)
        {
            base.Write(s, msk);

            if ((msk & MSK_ID) == MSK_ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(idx), idx);
        }

        public short Key => id;

        public bool IsProv => typ == TYP_PROV;

        public bool IsDist => typ == TYP_DIST;

        public bool IsSect => typ == TYP_SECT;


        public override string ToString() => name;


        public void Render(MvContext mc)
        {
        }
    }
}