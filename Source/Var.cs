using System;
using Urbrural.Mv;

namespace Urbrural
{
    public class Var : IEquatable<Var>, IComparable<Var>
    {
        readonly MvVar _ref;

        public Var(MvVar @ref)
        {
            _ref = @ref;
        }

        internal MvVar Ref => _ref;

        public short State { get; internal set; }

        public decimal Value { get; set; }

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

            return State == other.State && Value == other.Value;
        }

        public int CompareTo(Var other)
        {
            var s = State - other.State;
            if (s > 0) return 1;
            if (s < 0) return -1;

            var v = Value - other.Value;
            if (v > 0) return 1;
            if (v < 0) return -1;
            return 0;
        }
    }
}