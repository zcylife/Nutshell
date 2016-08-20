using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.ThreadWorkerPool.Core
{
    public enum ThreadWorkerStatus
    {
        Created = 1,
        Abort = 2,
        Working = 3,
        Idle = 4
    }
}
