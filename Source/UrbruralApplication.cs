using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chainly;
using Chainly.Web;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using static System.Data.IsolationLevel;

namespace Urbrural
{
    public class Point
    {
        public int X { get; set; }
        public int Y;
    }

    public class UrbruralApplication : Application
    {
        static readonly Map<short, Scheme> schemas = new Map<short, Scheme>();

        static readonly Map<short, Reg> regions = new Map<short, Reg>();

        static readonly ConcurrentDictionary<int, Proj> projects = new ConcurrentDictionary<int, Proj>();

        static readonly ConcurrentDictionary<int, Deal> deals = new ConcurrentDictionary<int, Deal>();

        // periodic polling and concluding ended lots 
        static readonly Thread cycler = new Thread(Cycle);

        /// <summary>
        /// The entry point of the application.
        /// </summary>
        public static async Task Main(string[] args)
        {
            var result = await CSharpScript.EvaluateAsync<int>("2 + 3");
            Console.WriteLine(result);


            var now = await CSharpScript.EvaluateAsync("System.DateTime.Now");
            Console.WriteLine(now);

            var point = new Point {X = 3, Y = 5};
            var ps = await CSharpScript.EvaluateAsync<int>("X*2 + Y*2", globals: point);
            Console.WriteLine(ps);

            // start the concluder thead
            // cycler.Start();

            if (args.Length == 0 || args.Contains("main"))
            {
                CacheUp();

                CreateService<WwwService>("www");

                CreateService<MgtService>("mgt");
            }
            else
            {
                if (args.Contains("www-p"))
                {
                    CreateService<ProxyService>("www");
                }

                if (args.Contains("mgt-p"))
                {
                    CreateService<ProxyService>("mgt");
                }
            }

            await StartAsync();
        }


        public static void CacheUp()
        {
            Cache(dc =>
                {
                    dc.Sql("SELECT ").collst(Reg.Empty).T(" FROM regs ORDER BY typ, id");
                    return dc.Query<short, Reg>();
                }, 3600 * 24
            );

            CacheObject<int, Org>((dc, id) =>
                {
                    dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE id = @1");
                    return dc.QueryTop<Org>(p => p.Set(id));
                }, 60 * 15
            );

            Cache(dc =>
                {
                    dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE typ >= ").T(Org.TYP_CTR);
                    return dc.Query<int, Org>();
                }, 60 * 15
            );
        }

        static async void Cycle(object state)
        {
            var lst = new List<int>(64);
            while (true)
            {
                Thread.Sleep(60 * 1000);

                var today = DateTime.Today;
                // WAR("cycle: " + today);

                // to succeed
                lst.Clear();
                try
                {
                    using (var dc = NewDbContext())
                    {
                        // dc.Sql("SELECT id FROM lots WHERE status = ").T(Flow_.STATUS_CREATED).T(" AND ended < @1 AND qtys >= min");
                        await dc.QueryAsync(p => p.Set(today));
                        while (dc.Next())
                        {
                            dc.Let(out int id);
                            lst.Add(id);
                        }
                    }
                    foreach (var lotid in lst)
                    {
                        using var dc = NewDbContext(ReadCommitted);
                        try
                        {
                        }
                        catch (Exception e)
                        {
                            dc.Rollback();
                            Err(e.Message);
                        }
                    }

                    // to abort
                    lst.Clear();
                    using (var dc = NewDbContext())
                    {
                        // dc.Sql("SELECT id FROM lots WHERE status = ").T(Flow_.STATUS_CREATED).T(" AND ended < @1 AND qtys < min");
                        await dc.QueryAsync(p => p.Set(today));
                        while (dc.Next())
                        {
                            dc.Let(out int id);
                            lst.Add(id);
                        }
                    }
                    foreach (var lotid in lst)
                    {
                        using var dc = NewDbContext(ReadCommitted);
                        try
                        {
                        }
                        catch (Exception e)
                        {
                            dc.Rollback();
                            Err(e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Err(nameof(Cycle) + ": " + e.Message);
                }
            }
        }
    }
}