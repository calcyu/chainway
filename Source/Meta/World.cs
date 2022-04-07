using System.Collections.Concurrent;

namespace Coverse.Meta
{
    public class World
    {
        ConcurrentDictionary<string, Scheme> types = new ConcurrentDictionary<string, Scheme>();


        // loaded projects
        ConcurrentDictionary<int, Project> projects = new ConcurrentDictionary<int, Project>();
    }
}