﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepServer.Common
{
    class MessageID
    {
        public const int Heat = 100;//心跳
        public const int Login = 10001;//登录
        public const int Match = 10002;//匹配对手
        public const int MatchResond = 10003;//匹配结果
    }
}
