using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.QueueWorker
{
    /// <summary>
    /// Nutshell 列队工作器公共接口
    /// </summary>
    /// <typeparam name="TWorkData"></typeparam>
    public interface IQueueWorker<TWorkData>
    {
        /// <summary>
        /// 获取当前列队数据的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 尝试添加数据到列队中
        /// </summary>
        /// <param name="data">要添加到列队的数据</param>
        /// <returns>true表示数据成功添加到列表，false表示数据无法添加的数据，原因列队已达到设置的最大值</returns>
        bool TryAdd(TWorkData data);

        /// <summary>
        /// 命令列队开始处理列队数据
        /// </summary>
        void Start();
    }
}
