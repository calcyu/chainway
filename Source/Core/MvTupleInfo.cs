using System;

namespace ChainPort.Core
{
    public class MvTupleInfo<T> where T : MvTuple, new()
    {
        private Type type;

        public T CreateInstance() => Activator.CreateInstance(type) as T;
    }
}