
namespace Nutshell.QueueWorker
{
    /// <summary>
    /// 用于列队数据处理的程序接口
    /// </summary>
    /// <typeparam name="TWorkData">需要列队的数据的类型</typeparam>
    public interface IWorkingHandler<TWorkData>
    {
        /// <summary>
        /// 表示如何处理列队中的数据
        /// </summary>
        /// <param name="data">当前需要处理的数据</param>
        void Working(TWorkData data);
    }
}
