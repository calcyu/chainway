using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoChain;
using CoChain.Web;
using Urbrural.Core;
using static System.Data.IsolationLevel;

namespace Urbrural
{
    public class UrbruralApplication : Application
    {
        // contextual objects
        //

        static readonly ConcurrentDictionary<int, MvProj> projects = new ConcurrentDictionary<int, MvProj>();

        static readonly ConcurrentDictionary<int, Deal> deals = new ConcurrentDictionary<int, Deal>();


        public static MvContext NewOpContext(Deal v)
        {
            var p = projects[v.projectid];
            return new MvContext000001();
        }

        // periodic polling and concluding ended lots 
        static readonly Thread cycler = new Thread(Cycle);

        /// <summary>
        /// The entry point of the application.
        /// </summary>
        public static async Task Main(string[] args)
        {
            // start the concluder thead
            // cycler.Start();

            // ScriptTest.Test();

            if (args.Length == 0 || args.Contains("main"))
            {
                CacheUp();

                CreateService<WwwService>("www");

                CreateService<MgtService>("mgt");

                CreateService<FedService>("fed");
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
                    dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE state > 0");
                    return dc.Query<int, Org>();
                }, 60 * 15
            );
        }

        static async void LoadProjects(object state)
        {
            var lst = new List<int>(64);
            try
            {
                using (var dc = NewDbContext())
                {
                    // dc.Sql("SELECT id FROM lots WHERE status = ").T(Flow_.STATUS_CREATED).T(" AND ended < @1 AND qtys >= min");
                    // await dc.QueryAsync(p => p.Set(today));
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

        static async void Cycle(object state)
        {
            var lst = new List<int>(64);
            while (true)
            {
                Thread.Sleep(60 * 1000);

                var today = DateTime.Today;

                foreach (var pair in deals)
                {
                    var play = pair.Value;
                }
                // WAR("cycle: " + today);

                // to succeed
            }
        }
    }
}