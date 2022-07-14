using System;
using CoChain;

namespace Urbrural.Core
{
    public class Plan : Entity
    {
        int projectid;

        short idx;

        // counterpart to the conf in deal
        JObj confs;

        public override void Read(ISource s, short msk = 255)
        {
            throw new NotImplementedException();
        }

        public override void Write(ISink s, short msk = 255)
        {
            throw new NotImplementedException();
        }
    }
}