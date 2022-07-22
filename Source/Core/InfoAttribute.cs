using System;

namespace Urbrural.Core
{
    /// <summary>
    /// To determine principal identity based on current web context. The interaction with user, however, is not included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter, Inherited = false)]
    public class InfoAttribute : Attribute
    {
        readonly string name;

        public InfoAttribute(string name)
        {
            this.name = name;
        }

        public string IsName => name;
    }
}