﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SkyChain;
using static System.Data.IsolationLevel;

namespace Revital
{
    public class RevitalApp : Application
    {
        // periodic polling and concluding ended lots 
        static readonly Thread cycler = new Thread(Cycle);

        /// <summary>
        /// The entry point of the application.
        /// </summary>
        public static async Task Main(string[] args)
        {
            CacheUp();

            Drive = new RevitalDrive();

            // start the concluder thead
            // cycler.Start();

            // web
            MakeService<RevitalService>("main");
            await StartAsync();
        }


        public static void CacheUp()
        {
            CacheMap(dc =>
                {
                    dc.Sql("SELECT ").collst(Reg.Empty).T(" FROM regs ORDER BY typ, idx");
                    return dc.Query<short, Reg>();
                }, 3600 * 24
            );

            CacheMap(dc =>
                {
                    dc.Sql("SELECT ").collst(Item.Empty).T(" FROM items ORDER BY cat, id");
                    return dc.Query<short, Item>();
                }, 60 * 15
            );

            Cache<int, Org>((dc, id) =>
                {
                    dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE id = @1");
                    return dc.QueryTop<Org>(p => p.Set(id));
                }, 60 * 15
            );

            CacheMap(dc =>
                {
                    dc.Sql("SELECT ").collst(Plan.Empty).T(" FROM plans ORDER BY typ, status DESC");
                    return dc.Query<int, Plan>();
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