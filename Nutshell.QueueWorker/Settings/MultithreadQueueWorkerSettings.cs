using Nutshell.ThreadWorkerPool;
using System;

namespace Nutshell.QueueWorker.Settings
{
    /// <summary>
    /// 多线程列表工作器设置信息，它是一个结构类型
    /// </summary>
    public struct MultithreadQueueWorkerSettings
    {
        private int _maxQueueLength;
        private ThreadWorkerPoolSettings _threadWorkerPoolSettings;

        /// <summary>
        /// 初始化 MultithreadQueueWorkerSettings 结构的新实例
        /// </summary>
        /// <param name="maxQueueLength">最大列队数</param>
        /// <param name="minThreadWorkerCount">最小线程数</param>
        /// <param name="maxThreadWorkerCount">最大线程数</param>
        public MultithreadQueueWorkerSettings(int maxQueueLength, int minThreadWorkerCount, int maxThreadWorkerCount)
        {
            if (maxQueueLength < 1)
                throw new ArgumentException("Must be greater than zero!", "maxQueueLength");

            _maxQueueLength = maxQueueLength;
            _threadWorkerPoolSettings = ThreadWorkerPoolSettings.Default;
            _threadWorkerPoolSettings.MinThreadWorkerCount = minThreadWorkerCount;
            _threadWorkerPoolSettings.MaxThreadWorkerCount = maxThreadWorkerCount;
        }

        /// <summary>
        /// 初始化 MultithreadQueueWorkerSettings 结构的新实例
        /// </summary>
        /// <param name="maxQueueLength">最大列队数</param>
        /// <param name="threadWorkerPoolSettings">线程池设置</param>
        public MultithreadQueueWorkerSettings(int maxQueueLength, ThreadWorkerPoolSettings threadWorkerPoolSettings)
        {
            if (maxQueueLength < 1)
                throw new ArgumentException("Must be greater than zero!", "maxQueueLength");

            _maxQueueLength = maxQueueLength;
            _threadWorkerPoolSettings = threadWorkerPoolSettings;
        }

        /// <summary>
        /// 获取或者设置最大列队数
        /// </summary>
        public int MaxQueueLength
        {
            get { return _maxQueueLength; }
            set { _maxQueueLength = value; }
        }

        /// <summary>
        /// 获取或者设置线程池的设置信息
        /// </summary>
        public ThreadWorkerPoolSettings ThreadWorkerPoolSettings
        {
            get { return _threadWorkerPoolSettings; }
            set { _threadWorkerPoolSettings = value; }
        }
    }
}
