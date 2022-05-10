using Chainly;
using Urbrural.Mpml;

namespace Urbrural
{
    /// <summary>
    /// A variable prototype model.
    /// </summary>
    public class Varp : Info, IKeyable<string>, IProto
    {
        string id;

        string selector;

        Selector selectr;

        VarpState[] states;

        public override void Read(ISource s, short msk = 255)
        {
            base.Read(s, msk);

            s.Get(nameof(id), ref id);
            s.Get(nameof(id), ref id);
        }

        public override void Write(ISink s, short msk = 255)
        {
            base.Write(s, msk);

            s.Put(nameof(id), id);
            s.Put(nameof(id), id);
        }


        public string Key => id;
    }
}