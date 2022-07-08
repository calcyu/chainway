using System;

namespace Urbrural.Core
{
    public abstract class MvObj : IEquatable<MvObj>, IComparable<MvObj>
    {
        // last modified
        DateTime modified;


        public short State { get; internal set; }

        public decimal Value { get; set; }

        /// <summary>
        /// Functional wellness of the target, valued from 0.01 through 1.00
        /// </summary>
        public decimal Cent { get; set; }

        public decimal Qty { get; set; }

        public bool Equals(MvObj other)
        {
            if (other == null)
            {
                return false;
            }

            return State == other.State && Value == other.Value;
        }

        public int CompareTo(MvObj other)
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