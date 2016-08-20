
namespace Nutshell.ThreadWorkerPool
{
    /// <summary>
    /// 没有WorkData的线程工作器
    /// </summary>
    public interface IThreadWorker
    {
        /// <summary>
        /// 开始工作，只可使用一次
        /// </summary>
        void Work();
    }

    /// <summary>
    /// 拥有WorkData的线程工作器
    /// </summary>
    /// <typeparam name="TWorkData"></typeparam>
    public interface IThreadWorker<TWorkData>
    {
        /// <summary>
        /// 开始工作，只可使用一次
        /// </summary>
        /// <param name="workData">用传递到线程内的工作数据</param>
        void Work(TWorkData workData);
    }
}
