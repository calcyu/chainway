using System;

namespace ChainPort.Core
{
    public class MvInfo<T> where T : class, new()
    {
        private Type type;

        public T CreateInstance() => Activator.CreateInstance(type) as T;
    }
}