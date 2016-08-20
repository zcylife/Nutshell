using Nutshell.ThreadWorkerPool.Handlers;
using System;

namespace Nutshell.ThreadWorkerPool
{
    public sealed class ThreadWorkerPool : ThreadWorkerPoolBase, IThreadWorkerPool
    {
        private readonly WorkingHandler _workingHandler;

        internal ThreadWorkerPool(WorkingHandler workingHandler)
            : this(workingHandler, ThreadWorkerPoolSettings.Default)
        {

        }

        internal ThreadWorkerPool(WorkingHandler workingHandler, ThreadWorkerPoolSettings settings)
            : this(GetName(workingHandler), workingHandler, settings)
        {

        }

        internal ThreadWorkerPool(string name, WorkingHandler workingHandler, ThreadWorkerPoolSettings settings)
            : base(name, settings)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            _workingHandler = workingHandler;
            Init();
        }

        protected override ThreadWorkerPoolItem CreatePoolItem(int index, int idleTime)
        {
            string workerName = _workingHandler.ToString() + "_" + index.ToString();
            var settings = base.Settings;
            settings.IdleTime = idleTime;
            var worker = new ThreadWorker(workerName, _workingHandler, settings.Priority, settings.IsBackground, settings.IdleTime);
            return new ThreadWorkerPoolItem(this, worker, settings);
        }

        public IThreadWorker Take()
        {
            IThreadWorker worker;
            this.TryTake(-1, out worker);
            return worker;
        }

        public bool TryTake(out IThreadWorker threadWorker)
        {
            return this.TryTake(0, out threadWorker);
        }

        public bool TryTake(int timeout, out IThreadWorker threadWorker)
        {
            ThreadWorkerBase worker;
            if (base.TryTake(timeout, out worker))
            {
                threadWorker = (IThreadWorker)worker;
                return true;
            }
            threadWorker = null;
            return false;
        }

        public void Dispose()
        {
            this.Destory();
        }

        private static string GetName(WorkingHandler workingHandler)
        {
            if (workingHandler == null)
                return null;

            return workingHandler.GetType().FullName + "_Pool";
        }
    }

    public sealed class ThreadWorkerPool<T> : ThreadWorkerPoolBase, IThreadWorkerPool<T>
    {
        private readonly WorkingHandler<T> _workingHandler;

        internal ThreadWorkerPool(WorkingHandler<T> workerHandler)
            : this(workerHandler, ThreadWorkerPoolSettings.Default)
        {

        }

        internal ThreadWorkerPool(WorkingHandler<T> workingHandler, ThreadWorkerPoolSettings settings)
            : this(GetName(workingHandler), workingHandler, settings)
        {

        }

        internal ThreadWorkerPool(string name, WorkingHandler<T> workingHandler, ThreadWorkerPoolSettings settings)
            : base(name, settings)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            _workingHandler = workingHandler;
            Init();
        }

        protected override ThreadWorkerPoolItem CreatePoolItem(int index, int idleTime)
        {
            string workerName = _workingHandler.ToString() + "_" + index.ToString();
            var settings = base.Settings;
            settings.IdleTime = idleTime;
            var worker = new ThreadWorker<T>(workerName, _workingHandler, settings.Priority, settings.IsBackground, settings.IdleTime);
            return new ThreadWorkerPoolItem(this, worker, settings);
        }

        public IThreadWorker<T> Take()
        {
            IThreadWorker<T> worker;
            this.TryTake(-1, out worker);
            return worker;
        }

        public bool TryTake(out IThreadWorker<T> threadWorker)
        {
            return this.TryTake(0, out threadWorker);
        }

        public bool TryTake(int timeout, out IThreadWorker<T> threadWorker)
        {
            ThreadWorkerBase worker;
            if (base.TryTake(timeout, out worker))
            {
                threadWorker = (IThreadWorker<T>)worker;
                return true;
            }
            threadWorker = null;
            return false;
        }

        public void Dispose()
        {
            this.Destory();
        }

        private static string GetName(WorkingHandler<T> workingHandler)
        {
            if (workingHandler == null)
                return null;

            return workingHandler.GetType().FullName + "_Pool";
        }
    }
}
