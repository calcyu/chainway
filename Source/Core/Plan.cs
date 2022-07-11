using System;
using CoChain;

namespace Urbrural.Core
{
    public class Plan : IData
    {
        int projid;

        short idx;
        
        JObj config;
        
        public void Read(ISource s, short msk = 255)
        {
            throw new NotImplementedException();
        }

        public void Write(ISink s, short msk = 255)
        {
            throw new NotImplementedException();
        }
    }
}