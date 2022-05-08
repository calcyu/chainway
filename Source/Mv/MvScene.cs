using System;
using Chainly;

namespace Urbrural.Mv
{
    public class MvScene : IKeyable<short>
    {
        readonly short key;

        readonly string name;

        public Action DoLayout;

        public short Key => key;

        MvScene(short key, string name)
        {
            this.key = key;
            this.name = name;
        }
        //
        // processing cycles and logics


        public static Map<short, MvScene> All = new Map<short, MvScene>()
        {
            new MvScene(1, "田园种植"),
            new MvScene(2, "温室栽培"),
            new MvScene(3, "生态果园"),
            new MvScene(4, "鱼菜共生"),
            new MvScene(5, "生态畜牧"),
            new MvScene(6, "碳汇林业"),
            new MvScene(7, "田园文化艺术"),
            new MvScene(8, "绿色电力"),
        };
    }
}