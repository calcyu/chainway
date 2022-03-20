﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coverse.Meta;
using SkyChain;
using SkyChain.Nodal;
using SkyChain.Web;
using static System.Data.IsolationLevel;

namespace Coverse
{
    public class RuralApp : Application
    {
        Map<short, Schema> schemas = new Map<short, Schema>();

        ConcurrentDictionary<int, Plan> plans = new ConcurrentDictionary<int, Plan>();

        public static S CreateSchema<S>(string name) where S : Schema, new()
        {
            if (name == null)
            {
                // throw new ApplicationException("missing app.json");
            }
            return null;
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

            if (args.Length == 0 || args.Contains("main"))
            {
                CacheUp();

                CreateService<WwwService>("www");

                CreateService<MgtService>("mgt");
            }
            else
            {
                if (args.Contains("pub-p"))
                {
                    CreateService<WebProxy>("pub");
                }

                if (args.Contains("mgt-p"))
                {
                    CreateService<WebProxy>("mgt");
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

            Cache(dc =>
                {
                    dc.Sql("SELECT ").collst(Item.Empty).T(" FROM items ORDER BY typ, id");
                    return dc.Query<short, Item>();
                }, 60 * 15
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

            CacheMap((DbContext dc, int orgid) =>
                {
                    dc.Sql("SELECT ").collst(Product.Empty).T(" FROM products WHERE orgid = @1 AND status > 0 ORDER BY status DESC");
                    return dc.Query<int, Product>(p => p.Set(orgid));
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
                            ERR(e.Message);
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
                            ERR(e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    ERR(nameof(Cycle) + ": " + e.Message);
                }
            }
        }
    }
}