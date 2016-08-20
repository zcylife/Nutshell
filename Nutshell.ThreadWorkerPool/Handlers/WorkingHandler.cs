
namespace Nutshell.ThreadWorkerPool.Handlers
{
    /// <summary>
    /// 线程工作处理程序
    /// </summary>
    public delegate void WorkingHandler();

    /// <summary>
    /// 线程工作处理程序
    /// </summary>
    /// <typeparam name="TWorkData">线程工作数据类型</typeparam>
    /// <param name="workData">线程工作数据</param>
    public delegate void WorkingHandler<TWorkData>(TWorkData workData);
}
