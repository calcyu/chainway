using System;

namespace ChainVerse.Core
{
    /// <summary>
    /// The abstract root class for metaverse tasks.
    /// </summary>
    public class MvTask : MvTuple, IVisual
    {
        string name;

        internal short state;

        internal decimal value;

        internal short cent;

        internal decimal ext;

        public string Key => name;

        public MvTask Parent { get; set; }


        public Func<bool> OnEnter { get; set; }

        public bool Equals(MvTask other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(MvTask other)
        {
            throw new NotImplementedException();
        }

        public void Render(MvContext mc)
        {
            throw new NotImplementedException();
        }
    }
}