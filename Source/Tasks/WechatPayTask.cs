using System;
using Urbrural.Core;

namespace Urbrural.Tasks
{
    public class WechatPayTask : MvTask
    {
        public bool Ecny { get; set; }

        public Func<MvContext, bool> Before { get; set; }

        public void @default(MvContext ctx)
        {
        }

        public void exec(MvContext ctx)
        {
        }
    }
}