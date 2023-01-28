using System;

namespace ChainPort.Core
{
    /// <summary>
    /// To determine principal identity based on current web context. The interaction with user, however, is not included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, Inherited = false)]
    public class SubAttribute : Attribute
    {
        readonly string name;

        public SubAttribute()
        {
        }

        public string IsName => name;
    }
}