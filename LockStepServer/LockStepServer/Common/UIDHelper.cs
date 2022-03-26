using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepServer.Common
{
    class UIDHelper
    {
        private static int uid=0;
        public static int GetUID()
        {
            uid++;
            return uid;
        }
    }
}
