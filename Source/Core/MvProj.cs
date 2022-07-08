using CoChain;

namespace Urbrural.Core
{
    public class MvProj : Scope, IKeyable<int>
    {
        #region IDATA

        public static readonly MvProj Empty = new MvProj();

        int id;
        internal short regid;
        double x;
        double y;
        string unit;
        short min;
        short max;
        short step;

        JObj config;

        public override void Read(ISource s, short msk = 255)
        {
            base.Read(s, msk);

            if ((msk & ID) == ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(x), ref x);
            s.Get(nameof(y), ref y);
            s.Get(nameof(unit), ref unit);
            s.Get(nameof(min), ref min);
            s.Get(nameof(max), ref max);
            s.Get(nameof(step), ref step);
            s.Get(nameof(config), ref config);
        }

        public override void Write(ISink s, short msk = 255)
        {
            base.Write(s, msk);

            if ((msk & ID) == ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(x), x);
            s.Put(nameof(y), y);
            s.Put(nameof(unit), unit);
            s.Put(nameof(min), min);
            s.Put(nameof(max), max);
            s.Put(nameof(step), step);
            s.Put(nameof(config), config);
        }

        public int Key => id;

        #endregion


        #region MPML

        MvScene _class;

        decimal ver;

        void Resolve()
        {
            string sch = null;
            config.Get(nameof(_class), ref sch);
            config.Get(nameof(ver), ref ver);
            _class = ClassUtility.GetSchema(sch);
        }

        public Deal NewDeal()
        {
            var o = new Deal();

            // deal-level vars

            return null;
        }

        #endregion
    }
}