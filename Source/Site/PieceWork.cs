using System;
using System.Threading.Tasks;
using SkyChain.Web;
using static Revital.Item;
using static SkyChain.Web.Modal;

namespace Revital
{
    public class PublyPieceWork : PieceWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<PublyPieceVarWork>();
        }

        public void @default(WebContext wc, int page)
        {
        }
    }

    [Ui("［商户］线上货架")]
    public class BizlyPieceWork : PieceWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<BizlyPieceVarWork>();
        }

        [Ui("上架", group: 1), Tool(Anchor)]
        public async Task @default(WebContext wc)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Piece.Empty).T(" FROM peices WHERE orgid = @1 AND status >= 2 ORDER BY status DESC");
            var arr = await dc.QueryAsync<Piece>(p => p.Set(org.id));
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                h.TABLE(arr, o =>
                {
                    h.TD_().A_TEL(o.name, o.tip)._TD();
                    h.TD(o.price, true);
                    // h.TD(Statuses[o.status]);
                });
            });
        }

        [Ui("下架", group: 2), Tool(Anchor)]
        public async Task off(WebContext wc)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Piece.Empty).T(" FROM peices WHERE orgid = @1 AND status <= 1 ORDER BY status DESC");
            var arr = await dc.QueryAsync<Piece>(p => p.Set(org.id));
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                h.TABLE(arr, o =>
                {
                    // h.TD_().TOOLVAR(o.Key, nameof(BizlyPieceVarWork.upd), caption: o.name).SP()._TD();
                    h.TD(o.price, true);
                    // h.TD(Statuses[o.status]);
                });
            });
        }

        [Ui("✚", "添加平台溯源商品", group: 3), Tool(ButtonOpen)]
        public async Task add(WebContext wc)
        {
            var org = wc[-1].As<Org>();
            var prin = (User) wc.Principal;
            var items = Grab<short, Item>();
            if (wc.IsGet)
            {
                // get booking orders
                using var dc = NewDbContext();
                dc.Sql("SELECT DISTINCT productid FROM books_ WHERE ");


                var o = new Piece
                {
                    created = DateTime.Now,
                    creator = prin.name,
                    status = _Info.STA_DISABLED,
                    unitx = 1,
                    min = 1,
                    max = 100,
                    step = 1,
                    cap = 100
                };
                wc.GivePane(200, h =>
                {
                    // var ctr = Obtain<int, Org>(org.ctrid);
                    // var plans = ObtainSub<int, int, Plan>(org.ctrid);
                    //
                    // h.FORM_().FIELDSUL_(ctr.name);
                    //
                    // h.LI_().SELECT_PLAN("供应项目", nameof(o.planid), o.planid, plans, Cats, required: true)._LI();
                    // h.LI_().TEXTAREA("特色描述", nameof(o.tip), o.tip, max: 40)._LI();
                    // h.LI_().SELECT("状态", nameof(o.status), o.status, _Article.Statuses, required: true)._LI();
                    //
                    // h._FIELDSUL().FIELDSUL_("规格参数");
                    //
                    // h.LI_().TEXT("单位", nameof(o.unit), o.unit, min: 1, max: 4, required: true).NUMBER("标准比", nameof(o.unitx), o.unitx, min: 1, max: 1000, required: true)._LI();
                    // h.LI_().NUMBER("单价", nameof(o.price), o.price, min: 0.00M, max: 99999.99M)._LI();
                    // h.LI_().NUMBER("起订量", nameof(o.min), o.min).NUMBER("限订量", nameof(o.max), o.max, min: 1, max: 1000)._LI();
                    // h.LI_().NUMBER("递增量", nameof(o.step), o.step).NUMBER("最大容量", nameof(o.cap), o.cap)._LI();
                    //
                    // h._FIELDSUL();
                    //
                    // h.BOTTOM_BUTTON("确定");
                    //
                    // h._FORM();
                });
            }
            else // POST
            {
                // populate 
                var o = await wc.ReadObjectAsync(0, new Piece
                {
                    created = DateTime.Now,
                    creator = prin.name,
                    orgid = org.id,
                });
                var item = items[o.itemid];
                o.typ = item.typ;
                o.name = item.name + '（' + o.ext + '）';

                // insert
                using var dc = NewDbContext();
                dc.Sql("INSERT INTO plans ").colset(Product.Empty, 0)._VALUES_(Product.Empty, 0);
                await dc.ExecuteAsync(p => o.Write(p, 0));

                wc.GivePane(200); // close dialog
            }
        }

        [Ui("✜", "添加无溯源商品", group: 3), Tool(ButtonOpen)]
        public async Task @new(WebContext wc)
        {
            var org = wc[-1].As<Org>();
            var prin = (User) wc.Principal;
            var items = Grab<short, Item>();
            if (wc.IsGet)
            {
                var o = new Piece
                {
                    created = DateTime.Now,
                    creator = prin.name,
                    status = _Info.STA_DISABLED,
                    unitx = 1,
                    min = 1,
                    max = 10,
                    step = 1,
                    cap = 100
                };
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("基本信息");

                    h.LI_().SELECT_ITEM("品目名", nameof(o.itemid), o.itemid, items, Typs, filter: x => x.typ == org.fork, required: true).TEXT("附加名", nameof(o.ext), o.ext, max: 10)._LI();
                    h.LI_().TEXTAREA("特色描述", nameof(o.tip), o.tip, max: 40)._LI();
                    h.LI_().SELECT("状态", nameof(o.status), o.status, _Info.Statuses, required: true)._LI();

                    h._FIELDSUL().FIELDSUL_("规格参数");

                    h.LI_().TEXT("单位", nameof(o.unit), o.unit, min: 1, max: 4, required: true).NUMBER("标准比", nameof(o.unitx), o.unitx, min: 1, max: 1000, required: true)._LI();
                    h.LI_().NUMBER("单价", nameof(o.price), o.price, min: 0.00M, max: 99999.99M)._LI();
                    h.LI_().NUMBER("起订量", nameof(o.min), o.min).NUMBER("限订量", nameof(o.max), o.max, min: 1, max: 1000)._LI();
                    h.LI_().NUMBER("递增量", nameof(o.step), o.step).NUMBER("现存量", nameof(o.cap), o.cap)._LI();

                    h._FIELDSUL();

                    h.BOTTOM_BUTTON("确定");

                    h._FORM();
                });
            }
            else // POST
            {
                // populate 
                var o = await wc.ReadObjectAsync(0, new Piece
                {
                    created = DateTime.Now,
                    creator = prin.name,
                    orgid = org.id,
                });
                var item = items[o.itemid];
                o.typ = item.typ;
                o.name = item.name + '（' + o.ext + '）';

                // insert
                using var dc = NewDbContext();
                dc.Sql("INSERT INTO posts ").colset(Piece.Empty, 0)._VALUES_(Piece.Empty, 0);
                await dc.ExecuteAsync(p => o.Write(p, 0));

                wc.GivePane(200); // close dialog
            }
        }
    }
}