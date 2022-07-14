using CoChain;

namespace Urbrural.Core
{
    public class MvDeal : MvScope, IKeyable<int>
    {
        #region IDATA

        public static readonly MvDeal Empty = new MvDeal();

        int id;

        int projid;

        //
        // duplicate plan detail here

        private string planname;


        internal int orgid;
        internal short regid;
        double x;
        double y;
        internal int uid;
        internal string uname;
        internal string utel;
        internal string uim;
        internal string unit;
        short min;
        short max;
        short step;
        internal decimal price;
        internal decimal qty;
        internal decimal fee;
        internal decimal pay;

        internal JObj conf;

        public override void Read(ISource s, short msk = 255)
        {
            base.Read(s, msk);

            if ((msk & ID) == ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(orgid), ref orgid);
            s.Get(nameof(x), ref x);
            s.Get(nameof(y), ref y);

            s.Get(nameof(uid), ref uid);
            s.Get(nameof(uname), ref uname);
            s.Get(nameof(utel), ref utel);
            s.Get(nameof(uim), ref uim);
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

        public override void Write(ISink s, short msk = 255)
        {
            base.Write(s, msk);

            if ((msk & ID) == ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(orgid), orgid);

            s.Put(nameof(x), x);
            s.Put(nameof(y), y);

            s.Put(nameof(uid), uid);
            s.Put(nameof(uname), uname);
            s.Put(nameof(utel), utel);
            s.Put(nameof(uim), uim);
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

        #endregion


        #region MPML

        Cat _class;

        decimal ver;

        void Resolve()
        {
            string sch = null;
            conf.Get(nameof(_class), ref sch);
            conf.Get(nameof(ver), ref ver);
            _class = CatUtility.GetSchema(sch);
        }

        #endregion
    }
}