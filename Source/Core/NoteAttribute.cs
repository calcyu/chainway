using System;
using System.Threading.Tasks;
using static CoChain.CryptoUtility;

namespace Urbrural.Core
{
    /// <summary>
    /// To determine principal identity based on current web context. The interaction with user, however, is not included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public abstract class NoteAttribute : Attribute
    {
        readonly bool async;

        protected NoteAttribute(bool async)
        {
            this.async = async;
        }

        public bool IsAsync => async;
    }
}