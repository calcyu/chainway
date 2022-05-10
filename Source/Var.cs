using System;
using Urbrural.Mpml;

namespace Urbrural
{
    public class Var : IEquatable<Var>, IComparable<Var>
    {
        readonly VarDef _def;
        
        public Var(VarDef def)
        {
            _def = def;
        }

        internal VarDef Def => _def;

        public short Mode { get; internal set; }

        public decimal Scale { get; set; }

        /// <summary>
        /// Functional wellness of the target, valued from 0.01 through 1.00
        /// </summary>
        public decimal Well { get; set; }

        public bool Equals(Var other)
        {
            if (other == null)
            {
                return false;
            }

            return Mode == other.Mode && Scale == other.Scale;
        }

        public int CompareTo(Var other)
        {
            var s = Mode - other.Mode;
            if (s > 0) return 1;
            if (s < 0) return -1;

            var v = Scale - other.Scale;
            if (v > 0) return 1;
            if (v < 0) return -1;
            return 0;
        }
    }
}