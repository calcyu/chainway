using System.Collections.Concurrent;
using System.Collections.Generic;
using Chainly;
using Chainly.Web;

namespace Urbrural
{
    public abstract class PostWork : WebWork
    {
    }

    public class OrglyPostWork : PostWork
    {
        static readonly ConcurrentDictionary<int, Queue<Post>> queues = new ConcurrentDictionary<int, Queue<Post>>();

        public void @default(WebContext wc, int page)
        {
            var org = wc[-1].As<Org>();
            var q = queues[org.id];
            if (q != null)
            {
                var jc = new JsonContent(true, 16);
                lock (q)
                {
                    Post o;
                    jc.ARR_();
                    while ((o = q.Dequeue()) != null)
                    {
                        jc.OBJ(j => { j.Put(nameof(o.typ), o.typ); });
                    }
                    jc._ARR();
                }
                wc.Give(200, jc);
            }
            else
            {
                wc.Give(204); // no content
            }
        }

        public static void Append(int orgid, Post post)
        {
            var q = queues[orgid];
            if (q == null)
            {
                q = new Queue<Post>();
                q.Enqueue(post);
                queues.TryAdd(orgid, q);
            }
            else
            {
                lock (q)
                {
                    q.Enqueue(post);
                }
            }
        }
    }
}