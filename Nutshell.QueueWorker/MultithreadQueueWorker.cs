using System;
using System.Collections.Concurrent;
using Nutshell.QueueWorker.Settings;
using System.Threading;
using Nutshell.ThreadWorkerPool;
using Nutshell.ThreadWorkerPool.Handlers;

namespace Nutshell.QueueWorker
{
    /// <summary>
    /// 基于线程池的多线程本地列队工作器(FIFO)
    /// </summary>
    /// <typeparam name="TWorkData">需要列队的数据的类型</typeparam>
    public sealed class MultithreadQueueWorker<TWorkData> : IQueueWorker<TWorkData>, IDisposable
    {
        private readonly ConcurrentQueue<TWorkData> _queue;
        private readonly MultithreadQueueWorkerSettings _settings;
        private Thread _mainThread;
        private AutoResetEvent _waitEvent;
        private bool _isStarted = false;
        private IThreadWorkerPool<TWorkData> _threadPool;
        private readonly WorkingHandler<TWorkData> _workingHandler;

        /// <summary>
        /// 初始化 MultithreadQueueWorker 类的新实例
        /// </summary>
        /// <param name="workingHandler">用于对列队数据的处理程序</param>
        /// <param name="settings">列队设置信息</param>
        public MultithreadQueueWorker(WorkingHandler<TWorkData> workingHandler, MultithreadQueueWorkerSettings settings)
            : this(settings)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            _workingHandler = workingHandler;
        }

        /// <summary>
        /// 初始化 MultithreadQueueWorker 类的新实例
        /// </summary>
        /// <param name="workingHandler">用于对列队数据的处理程序</param>
        /// <param name="settings">列队设置信息</param>
        public MultithreadQueueWorker(IWorkingHandler<TWorkData> workingHandler, MultithreadQueueWorkerSettings settings)
            : this(settings)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            _workingHandler = workingHandler.Working;
        }

        private MultithreadQueueWorker(MultithreadQueueWorkerSettings settings)
        {
            _queue = new ConcurrentQueue<TWorkData>();
            _settings = settings;
        }

        /// <summary>
        /// 获取当前列队数据的数量
        /// </summary>
        public int Count
        {
            get { return _queue.Count; }
        }

        /// <summary>
        /// 尝试添加数据到列队中
        /// </summary>
        /// <param name="data">要添加到列队的数据</param>
        /// <returns>true表示数据成功添加到列表，false表示数据无法添加的数据，原因列队已达到设置的最大值</returns>
        public bool TryAdd(TWorkData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (_settings.MaxQueueLength != 0 && _queue.Count >= _settings.MaxQueueLength)
                return false;

            _queue.Enqueue(data);
            if (_waitEvent != null && !_isDisposed)
                _waitEvent.Set();

            return true;
        }

        /// <summary>
        /// 命令列队开始处理列队数据
        /// </summary>
        public void Start()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            if (_isStarted)
                throw new InvalidOperationException("This QueueWorker was started, not duplicate start!");

            _waitEvent = new AutoResetEvent(true);

            _threadPool = ThreadWorkerPoolFactory.Create(_workingHandler, _settings.ThreadWorkerPoolSettings);

            _mainThread = new Thread(Working);
            _mainThread.Name = this.GetType().FullName;
            _mainThread.Start();
        }

        private void Working()
        {
            TWorkData data;
            while (!_isDisposed)
            {
                if (_queue.IsEmpty)
                    _waitEvent.WaitOne(100);

                if (_queue.IsEmpty || _isDisposed)
                    continue;

                if (_queue.TryDequeue(out data))
                    _threadPool.Take()
                               .Work(data);
            }
        }

        private bool _isDisposed = false;

        /// <summary>
        /// 释放列队资源
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed && _isStarted)
            {
                _isDisposed = true;
                _waitEvent.Set();
                _waitEvent.Close();
                _waitEvent.Dispose();
                _mainThread.Join();
            }
        }
    }
}
