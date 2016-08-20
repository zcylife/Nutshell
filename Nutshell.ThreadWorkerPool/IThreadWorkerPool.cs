using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.ThreadWorkerPool
{
    /// <summary>
    /// 没有WorkData的线程池接口
    /// </summary>
    public interface IThreadWorkerPool : IDisposable
    {
        /// <summary>
        /// 获取线程池名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取当前线程池中线程总数
        /// </summary>
        int TotalThreadWorkerCount { get; }

        /// <summary>
        /// 获取当前线程池中空闲线程数量
        /// </summary>
        int IdleThreadWorkerCount { get; }

        /// <summary>
        /// 获取线程池的设置信息
        /// </summary>
        ThreadWorkerPoolSettings Settings { get; }

        /// <summary>
        /// 获取一个空闲线程，如果没有可用的空闲线程将发生堵塞
        /// </summary>
        IThreadWorker Take();

        /// <summary>
        /// 尝试获取一个空闲线程
        /// </summary>
        /// <param name="threadWorker">输出空闲线程，如果结果为false，则输出null</param>
        /// <returns>false没有可用的空闲线程，ture获取到可用的空闲线程</returns>
        bool TryTake(out IThreadWorker threadWorker);

        /// <summary>
        /// 尝试获取一个空闲线程
        /// </summary>
        /// <param name="timeout">超时时间(毫秒)</param>
        /// <param name="threadWorker">输出空闲线程，如果结果为false，则输出null</param>
        /// <returns>false没有可用的空闲线程，ture获取到可用的空闲线程</returns>
        bool TryTake(int timeout, out IThreadWorker threadWorker);
    }

    /// <summary>
    /// 拥有WorkData的线程池接口
    /// </summary>
    /// <typeparam name="TWorkData">线程工作数据类型</typeparam>
    public interface IThreadWorkerPool<TWorkData> : IDisposable
    {
        /// <summary>
        /// 获取线程池名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取当前线程池中线程总数
        /// </summary>
        int TotalThreadWorkerCount { get; }

        /// <summary>
        /// 获取当前线程池中空闲线程数量
        /// </summary>
        int IdleThreadWorkerCount { get; }

        /// <summary>
        /// 获取线程池的设置信息
        /// </summary>
        ThreadWorkerPoolSettings Settings { get; }

        /// <summary>
        /// 获取一个空闲线程，如果没有可用的空闲线程将发生堵塞
        /// </summary>
        IThreadWorker<TWorkData> Take();

        /// <summary>
        /// 尝试获取一个空闲线程
        /// </summary>
        /// <param name="threadWorker">输出空闲线程，如果结果为false，则输出null</param>
        /// <returns>false没有可用的空闲线程，ture获取到可用的空闲线程</returns>
        bool TryTake(out IThreadWorker<TWorkData> threadWorker);

        /// <summary>
        /// 尝试获取一个空闲线程
        /// </summary>
        /// <param name="timeout">超时时间(毫秒)</param>
        /// <param name="threadWorker">输出空闲线程，如果结果为false，则输出null</param>
        /// <returns>false没有可用的空闲线程，ture获取到可用的空闲线程</returns>
        bool TryTake(int timeout, out IThreadWorker<TWorkData> threadWorker);
    }
}
