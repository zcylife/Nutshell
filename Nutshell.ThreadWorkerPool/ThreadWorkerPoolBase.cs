using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Nutshell.ThreadWorkerPool
{
    public abstract class ThreadWorkerPoolBase
    {
        private readonly List<ThreadWorkerPoolItem> _threadWorkerList;
        private readonly string _name;
        private readonly ThreadWorkerPoolSettings _settings;
        private readonly object _takeLocker = new object();
        private readonly AutoResetEvent _takeWaitEvent;
        private bool _isDestoryed;

        internal ThreadWorkerPoolBase(string name, ThreadWorkerPoolSettings settings)
        {
            _name = name;
            _settings = settings;
            _takeWaitEvent = new AutoResetEvent(false);
            _threadWorkerList = new List<ThreadWorkerPoolItem>(_settings.MaxThreadWorkerCount);
            ThreadWorkerPoolWatcher.Instance.Add(this);
        }

        internal void Init()
        {
            if (_settings.MinThreadWorkerCount > 0)
            {
                for (int index = 1; index <= _settings.MinThreadWorkerCount; index++)
                {
                    _threadWorkerList.Add(this.CreatePoolItem(index, -1));
                }
            }
        }

        protected abstract ThreadWorkerPoolItem CreatePoolItem(int index, int idleTime);

        internal void Return(ThreadWorkerPoolItem item)
        {
            item.SetIdle();
            _takeWaitEvent.Set();
        }

        internal void Remove(ThreadWorkerPoolItem item)
        {
            _threadWorkerList.Remove(item);
        }

        public string Name
        {
            get { return _name; }
        }

        public int TotalThreadWorkerCount
        {
            get { return _threadWorkerList.Count; }
        }

        public int IdleThreadWorkerCount
        {
            get { return _threadWorkerList.Where(e => e.Status == Core.ThreadWorkerPoolItemStatus.Idle).Count(); }
        }

        public ThreadWorkerPoolSettings Settings
        {
            get { return _settings; }
        }

        protected bool TryTake(out ThreadWorkerBase threadWorker)
        {
            return this.TryTake(-1, out threadWorker);
        }

        protected bool TryTake(int timeout, out ThreadWorkerBase threadWorker)
        {
            threadWorker = null;
            lock (_takeLocker)
            {
                ThreadWorkerPoolItem worker = null;
                DateTime startWaitTime;
                while (!_isDestoryed)
                {
                    worker = _threadWorkerList.Where(e => e.Status == Core.ThreadWorkerPoolItemStatus.Idle).OrderByDescending(e => e.SurplusIdleTime).FirstOrDefault();
                    if (worker == null)
                    {
                        if (_threadWorkerList.Count < _settings.MaxThreadWorkerCount)
                        {
                            worker = this.CreatePoolItem(_threadWorkerList.Count + 1, _settings.IdleTime);
                            worker.SetTake();
                            _threadWorkerList.Add(worker);
                            threadWorker = worker.ThreadWorker;
                            return true;
                        }

                        startWaitTime = DateTime.Now;
                        if (!_takeWaitEvent.WaitOne(timeout))
                        {
                            threadWorker = null;
                            return false;
                        }

                        if (timeout != -1)
                        {
                            timeout = timeout - (int)(DateTime.Now - startWaitTime).TotalMilliseconds;
                            if (timeout <= 0)
                            {
                                threadWorker = null;
                                return false;
                            }
                        }
                        continue;
                    }

                    threadWorker = worker.ThreadWorker;
                    worker.SetTake();
                    return true;
                }

                threadWorker = null;
                return false;
            }
        }

        internal void Destory()
        {
            if (!_isDestoryed)
            {
                _isDestoryed = true;
                ThreadWorkerPoolWatcher.Instance.Remove(this);
                _takeWaitEvent.Set();
                _takeWaitEvent.Dispose();
                foreach (var worker in _threadWorkerList)
                {
                    worker.ThreadWorker.Abort();
                }
                _threadWorkerList.Clear();
            }
        }
    }
}
