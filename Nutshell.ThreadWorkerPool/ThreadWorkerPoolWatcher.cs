using System.Collections.Generic;
using System.Linq;

namespace Nutshell.ThreadWorkerPool
{
    /// <summary>
    /// 线程池监视器
    /// </summary>
    public sealed class ThreadWorkerPoolWatcher
    {
        private readonly static ThreadWorkerPoolWatcher _threadWorkerPoolManager = new ThreadWorkerPoolWatcher();
        private readonly List<ThreadWorkerPoolBase> _poolItems;

        static ThreadWorkerPoolWatcher() { }
        ThreadWorkerPoolWatcher()
        {
            _poolItems = new List<ThreadWorkerPoolBase>();
        }

        /// <summary>
        /// 获取 ThreadWorkerPoolWatcher 实例
        /// </summary>
        public static ThreadWorkerPoolWatcher Instance
        {
            get { return _threadWorkerPoolManager; }
        }

        internal void Add(ThreadWorkerPoolBase pool)
        {
            _poolItems.Add(pool);
        }

        internal void Remove(ThreadWorkerPoolBase pool)
        {
            _poolItems.Remove(pool);
        }

        /// <summary>
        /// 获取线程池总数
        /// </summary>
        public int TotalPoolCount
        {
            get { return _poolItems.Count; }
        }

        /// <summary>
        /// 获取所有线程池的线程总数
        /// </summary>
        public int TotalThreadWorkerCount
        {
            get { return _poolItems.Sum(e => e.TotalThreadWorkerCount); }
        }

        /// <summary>
        /// 获取所有线程池的空闲线程总数
        /// </summary>
        public int TotalIdleThreadWorkerCount
        {
            get { return _poolItems.Sum(e => e.IdleThreadWorkerCount); }
        }
    }
}
