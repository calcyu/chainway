using System;
using Urbrural.Mv;

namespace Urbrural
{
    public class Variable : IEquatable<Variable>, IComparable<Variable>
    {
        readonly MvVariable _ref;

        public Variable(MvVariable @ref)
        {
            _ref = @ref;
        }

        internal MvVariable Ref => _ref;

        public short State { get; internal set; }

        public decimal Value { get; set; }


        public bool Equals(Variable other)
        {
            if (other == null)
            {
                return false;
            }

            return State == other.State && Value == other.Value;
        }

        public int CompareTo(Variable other)
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