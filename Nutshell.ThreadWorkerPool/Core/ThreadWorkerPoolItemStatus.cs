using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.ThreadWorkerPool.Core
{
    internal enum ThreadWorkerPoolItemStatus
    {
        Idle = 1,
        Take = 2,
        Abort = 3
    }
}
