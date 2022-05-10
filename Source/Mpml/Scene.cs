using System;
using Chainly;

namespace Urbrural.Mpml
{
    public class Scene : IKeyable<short>
    {
        readonly short key;

        readonly string name;

        public Action DoLayout;

        public short Key => key;

        Scene(short key, string name)
        {
            this.key = key;
            this.name = name;
        }
        //
        // processing cycles and logics


        public static Map<short, Scene> All = new Map<short, Scene>()
        {
            new Scene(1, "田园种植"),
            new Scene(2, "温室栽培"),
            new Scene(3, "生态果园"),
            new Scene(4, "鱼菜共生"),
            new Scene(5, "生态畜牧"),
            new Scene(6, "碳汇林业"),
            new Scene(7, "田园文化艺术"),
            new Scene(8, "绿色电力"),
        };
    }
}