using CoChain.Web;

namespace Urbrural.Core
{
    /// <summary>
    /// A meta prototype for certain kind of jobs.
    /// </summary>
    public class MvTask : WebWork
    {
        string id;

        public string Key => id;
    }
}