using System;
using CoChain.Web;

namespace Urbrural.Core
{
    /// <summary>
    /// A meta prototype for certain kind of jobs.
    /// </summary>
    public class MvJob
    {
        string id;

        public string Key => id;

        public MvJob Parent { get; set; }


        public Func<bool> OnEnter { get; set; }
    }
}