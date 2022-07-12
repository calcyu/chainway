using System;

namespace Urbrural.Core
{
    /// <summary>
    /// The abstract root class of a metaverse job or task.
    /// </summary>
    public abstract class MvTask
    {
        string id;

        public string Key => id;

        public MvTask Parent { get; set; }


        public Func<bool> OnEnter { get; set; }
    }
}