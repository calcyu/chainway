using System.Threading.Tasks;
using SkyChain.Web;
using static SkyChain.Web.Modal;

namespace Zhnt.Supply
{
    [UserAuthorize(admly: User.ADMLY_OP)]
    [Ui("供应计划", "future")]
    public class AdmlyPlanWork : WebWork
    {
        protected override void OnMake()
        {
            MakeVarWork<AdmlyPlanVarWork>();
        }

        public void @default(WebContext wc, int page)
        {
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Plan.Empty).T(" FROM plans ORDER BY typ, status DESC LIMIT 40 OFFSET 40 * @1");
            var arr = dc.Query<Plan>(p => p.Set(page));
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();

                if (arr == null) return;

                h.TABLE_();
                short last = 0;
                foreach (var o in arr)
                {
                    if (o.typ != last)
                    {
                        h.TR_().TD_("uk-label uk-padding-tiny-left", colspan: 6).T(Item.Typs[o.typ])._TD()._TR();
                    }
                    h.TR_();
                    h.TD(Art_.Statuses[o.status]);
                    h.TD_("uk-visible@l").T(o.tip)._TD();
                    h._TR();
                    last = o.typ;
                }
                h._TABLE();
                h.PAGINATION(arr.Length == 40);
            });
        }

        [Ui("✚", "新建"), Tool(ButtonShow)]
        public async Task @new(WebContext wc)
        {
            var items = ObtainMap<short, Item>();
            if (wc.IsGet)
            {
                var o = new Plan();
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_();

                    h.LI_().SELECT_ITEM("类别", nameof(o.typ), o.typ, items, Item.Typs)._LI();
                    h.LI_().TEXT("名称", nameof(o.name), o.name, max: 10, required: true).TEXT("单位", nameof(o.bunit), o.bunit, max: 4)._LI();
                    h.LI_().TEXTAREA("简介", nameof(o.tip), o.tip, max: 40)._LI();

                    h._FIELDSUL().FIELDSUL_();

                    h.LI_().NUMBER("下游起订量", nameof(o.bmin), o.bmin, max: 10).NUMBER("上游起订量", nameof(o.pmin), o.pmin, max: 10)._LI();
                    h.LI_().NUMBER("限订量", nameof(o.bmax), o.bmax, max: 10).NUMBER("限订量", nameof(o.pmax), o.pmax, max: 10)._LI();
                    h.LI_().NUMBER("递增量", nameof(o.bstep), o.bstep, max: 10).NUMBER("递增量", nameof(o.pstep), o.pstep, max: 10)._LI();
                    h.LI_().NUMBER("价格", nameof(o.bprice), o.bprice, max: 10).NUMBER("价格", nameof(o.pprice), o.pprice, max: 10)._LI();
                    h.LI_().NUMBER("优惠", nameof(o.boff), o.boff, max: 10).NUMBER("优惠", nameof(o.poff), o.poff, max: 10)._LI();

                    h._FIELDSUL().FIELDSUL_();

                    h.LI_().SELECT("状态", nameof(o.status), o.status, Art_.Statuses)._LI();

                    h._FIELDSUL()._FORM();
                });
            }
            else // POST
            {
                var o = await wc.ReadObjectAsync<Item>(0);
                using var dc = NewDbContext();
                dc.Sql("INSERT INTO items ").colset(Item.Empty, 0)._VALUES_(Item.Empty, 0);
                await dc.ExecuteAsync(p => o.Write(p, 0));
                wc.GivePane(200); // close dialog
            }
        }
    }
}