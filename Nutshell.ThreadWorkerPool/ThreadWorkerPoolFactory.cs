using Nutshell.ThreadWorkerPool.Handlers;
using System;

namespace Nutshell.ThreadWorkerPool
{
    /// <summary>
    /// 线程池创建工厂
    /// </summary>
    public static class ThreadWorkerPoolFactory
    {
        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <param name="workingHandler">工作处理程序</param>
        public static IThreadWorkerPool Create(WorkingHandler workingHandler)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            return new ThreadWorkerPool(workingHandler);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <param name="name">线程池名称</param>
        /// <param name="workingHandler">工作处理程序</param>
        public static IThreadWorkerPool Create(string name, WorkingHandler workingHandler)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            return new ThreadWorkerPool(name, workingHandler, ThreadWorkerPoolSettings.Default);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <param name="workingHandler">工作处理程序</param>
        /// <param name="minThreadWorkerCount">最小线程数</param>
        /// <param name="maxThreadWorkerCount">最大线程数</param>
        public static IThreadWorkerPool Create(WorkingHandler workingHandler, int minThreadWorkerCount, int maxThreadWorkerCount)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            ThreadWorkerPoolSettings settings = ThreadWorkerPoolSettings.Default;
            settings.MinThreadWorkerCount = minThreadWorkerCount;
            settings.MaxThreadWorkerCount = maxThreadWorkerCount;
            return new ThreadWorkerPool(workingHandler, settings);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <param name="name">线程池名称</param>
        /// <param name="workingHandler">工作处理程序</param>
        /// <param name="minThreadWorkerCount">最小线程数</param>
        /// <param name="maxThreadWorkerCount">最大线程数</param>
        public static IThreadWorkerPool Create(string name, WorkingHandler workingHandler, int minThreadWorkerCount, int maxThreadWorkerCount)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            ThreadWorkerPoolSettings settings = ThreadWorkerPoolSettings.Default;
            settings.MinThreadWorkerCount = minThreadWorkerCount;
            settings.MaxThreadWorkerCount = maxThreadWorkerCount;
            return new ThreadWorkerPool(name, workingHandler, settings);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <param name="workingHandler">工作处理程序</param>
        /// <param name="settings">线程池设置</param>
        public static IThreadWorkerPool Create(WorkingHandler workingHandler, ThreadWorkerPoolSettings settings)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            return new ThreadWorkerPool(workingHandler, settings);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <param name="name">线程池名称</param>
        /// <param name="workingHandler">工作处理程序</param>
        /// <param name="settings">线程池设置</param>
        public static IThreadWorkerPool Create(string name, WorkingHandler workingHandler, ThreadWorkerPoolSettings settings)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            return new ThreadWorkerPool(name, workingHandler, settings);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <typeparam name="TWorkData">线程工作数据类型</typeparam>
        /// <param name="workingHandler">工作处理程序</param>
        public static IThreadWorkerPool<TWorkData> Create<TWorkData>(WorkingHandler<TWorkData> workingHandler)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            return new ThreadWorkerPool<TWorkData>(workingHandler);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <typeparam name="TWorkData">线程工作数据类型</typeparam>
        /// <param name="name">线程池名称</param>
        /// <param name="workingHandler">工作处理程序</param>
        public static IThreadWorkerPool<TWorkData> Create<TWorkData>(string name, WorkingHandler<TWorkData> workingHandler)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            return new ThreadWorkerPool<TWorkData>(name, workingHandler, ThreadWorkerPoolSettings.Default);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <typeparam name="TWorkData">线程工作数据类型</typeparam>
        /// <param name="workingHandler">工作处理程序</param>
        /// <param name="minThreadWorkerCount">最小线程数</param>
        /// <param name="maxThreadWorkerCount">最大线程数</param>
        public static IThreadWorkerPool<TWorkData> Create<TWorkData>(WorkingHandler<TWorkData> workingHandler, int minThreadWorkerCount, int maxThreadWorkerCount)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            ThreadWorkerPoolSettings settings = ThreadWorkerPoolSettings.Default;
            settings.MinThreadWorkerCount = minThreadWorkerCount;
            settings.MaxThreadWorkerCount = maxThreadWorkerCount;
            return new ThreadWorkerPool<TWorkData>(workingHandler, settings);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <typeparam name="TWorkData">线程工作数据类型</typeparam>
        /// <param name="name">线程池名称</param>
        /// <param name="workingHandler">工作处理程序</param>
        /// <param name="minThreadWorkerCount">最小线程数</param>
        /// <param name="maxThreadWorkerCount">最大线程数</param>
        /// <returns></returns>
        public static IThreadWorkerPool<TWorkData> Create<TWorkData>(string name, WorkingHandler<TWorkData> workingHandler, int minThreadWorkerCount, int maxThreadWorkerCount)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            ThreadWorkerPoolSettings settings = ThreadWorkerPoolSettings.Default;
            settings.MinThreadWorkerCount = minThreadWorkerCount;
            settings.MaxThreadWorkerCount = maxThreadWorkerCount;
            return new ThreadWorkerPool<TWorkData>(name, workingHandler, settings);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <typeparam name="TWorkData">线程工作数据类型</typeparam>
        /// <param name="workingHandler">工作处理程序</param>
        /// <param name="settings">线程池设置</param>
        public static IThreadWorkerPool<TWorkData> Create<TWorkData>(WorkingHandler<TWorkData> workingHandler, ThreadWorkerPoolSettings settings)
        {
            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            return new ThreadWorkerPool<TWorkData>(workingHandler, settings);
        }

        /// <summary>
        /// 创建一个新的线程池
        /// </summary>
        /// <typeparam name="TWorkData">线程工作数据类型</typeparam>
        /// <param name="name">线程池名称</param>
        /// <param name="workingHandler">工作处理程序</param>
        /// <param name="settings">线程池设置</param>
        public static IThreadWorkerPool<TWorkData> Create<TWorkData>(string name, WorkingHandler<TWorkData> workingHandler, ThreadWorkerPoolSettings settings)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (workingHandler == null)
                throw new ArgumentNullException("workingHandler");

            return new ThreadWorkerPool<TWorkData>(name, workingHandler, settings);
        }
    }
}
