﻿using System;
using ChainPort.Core;

namespace ChainPort.Tasks
{
    public class WechatPayTask : MvTask
    {
        public static readonly MvTaskInfo Info = new MvTaskInfo(typeof(WechatPayTask));
        
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