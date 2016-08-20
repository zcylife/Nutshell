using Nutshell.ThreadWorkerPool.Core;
using System;

namespace Nutshell.ThreadWorkerPool
{
    public sealed class ThreadWorkerPoolItem
    {
        private ThreadWorkerPoolItemStatus _status;
        private readonly ThreadWorkerBase _threadWorker;
        private readonly ThreadWorkerPoolBase _threadWorkerPool;
        private readonly int _idleTime;
        private DateTime _startIdleTime;

        internal ThreadWorkerPoolItem(ThreadWorkerPoolBase pool, ThreadWorkerBase threadWorker, ThreadWorkerPoolSettings poolSettings)
        {
            _threadWorkerPool = pool;
            _threadWorker = threadWorker;
            _threadWorker.OnIdleExpired += _threadWorker_OnIdleExpired;
            _threadWorker.OnWorkCompleted += _threadWorker_OnWorkCompleted;
            _threadWorker.Start();
            _status = ThreadWorkerPoolItemStatus.Idle;
            _idleTime = poolSettings.IdleTime;
        }

        void _threadWorker_OnWorkCompleted(object sender, EventArgs args)
        {
            _threadWorkerPool.Return(this);
        }

        void _threadWorker_OnIdleExpired(object sender, EventArgs args)
        {
            _threadWorkerPool.Remove(this);
        }

        internal ThreadWorkerPoolItemStatus Status
        {
            get
            {
                if (_threadWorker.Status == ThreadWorkerStatus.Abort || _status == ThreadWorkerPoolItemStatus.Abort)
                    return ThreadWorkerPoolItemStatus.Abort;

                return _status;
            }
        }

        internal int SurplusIdleTime
        {
            get
            {
                if (_status == ThreadWorkerPoolItemStatus.Take || _idleTime == -1)
                    return -1;

                int idledTime = (int)(_startIdleTime - DateTime.Now).TotalMilliseconds;
                if (idledTime >= _idleTime)
                    return 0;

                return idledTime;
            }
        }

        internal void SetTake()
        {
            _threadWorker.IsCanIdleExpried = false;
            _status = ThreadWorkerPoolItemStatus.Take;
        }

        internal void SetIdle()
        {
            _startIdleTime = DateTime.Now;
            _status = ThreadWorkerPoolItemStatus.Idle;
            _threadWorker.IsCanIdleExpried = true;
        }

        internal ThreadWorkerBase ThreadWorker
        {
            get { return _threadWorker; }
        }
    }
}
