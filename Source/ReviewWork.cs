﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using CoChain;
using CoChain.Web;

namespace Urbrural
{
    public abstract class ReviewWork : WebWork
    {
    }

    public class OrglyReviewWork : ReviewWork
    {
        static readonly ConcurrentDictionary<int, Queue<Review>> queues = new ConcurrentDictionary<int, Queue<Review>>();

        public void @default(WebContext wc, int page)
        {
            var org = wc[-1].As<Org>();
            var q = queues[org.id];
            if (q != null)
            {
                var jc = new JsonContent(true, 16);
                lock (q)
                {
                    Review o;
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

        public static void Append(int orgid, Review review)
        {
            var q = queues[orgid];
            if (q == null)
            {
                q = new Queue<Review>();
                q.Enqueue(review);
                queues.TryAdd(orgid, q);
            }
            else
            {
                lock (q)
                {
                    q.Enqueue(review);
                }
            }
        }
    }
}