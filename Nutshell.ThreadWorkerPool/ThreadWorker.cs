using Nutshell.ThreadWorkerPool.Core;
using Nutshell.ThreadWorkerPool.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nutshell.ThreadWorkerPool
{
    public sealed class ThreadWorker : ThreadWorkerBase, IThreadWorker
    {
        private readonly WorkingHandler _workingHandler;

        public ThreadWorker(string name, WorkingHandler workingHandler, ThreadPriority priority, bool isBackground, int idleTime)
            : base(name, priority, isBackground, idleTime)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            _workingHandler = workingHandler;
        }

        protected override void Working()
        {
            _workingHandler();
        }
    }

    public sealed class ThreadWorker<T> : ThreadWorkerBase, IThreadWorker<T>
    {
        private readonly WorkingHandler<T> _workingHandler;
        private T _workData;

        public ThreadWorker(string name, WorkingHandler<T> workingHandler, ThreadPriority priority, bool isBackground, int idleTime)
            : base(name, priority, isBackground, idleTime)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            _workingHandler = workingHandler;
        }

        protected override void Working()
        {
            _workingHandler(_workData);
        }

        public void Work(T workData)
        {
            _workData = workData;
            base.Work();
        }
    }
}
