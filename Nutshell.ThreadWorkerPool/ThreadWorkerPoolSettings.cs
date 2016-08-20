using System.Threading;

namespace Nutshell.ThreadWorkerPool
{
    /// <summary>
    /// 线程池设置信息，它是一个struct结构类型
    /// </summary>
    public struct ThreadWorkerPoolSettings
    {
        private int _minThreadWorkerCount;
        private int _maxThreadWorkerCount;
        private bool _isBackground;
        private int _idleTime;
        private ThreadPriority _priority;

        /// <summary>
        /// 初始化 ThreadWorkerPoolSettings
        /// </summary>
        /// <param name="minThreadWorkerCount">最小线程数</param>
        /// <param name="maxThreadWorkerCount">最大线程数</param>
        /// <param name="isBackground">一个标识，设置线程是否在后台运行</param>
        /// <param name="idleTime">线程空闲时间（毫秒）,空闲时间到期后将自动被销毁， -1表示永不到期（不建议）</param>
        /// <param name="priority">线程优先级</param>
        public ThreadWorkerPoolSettings(int minThreadWorkerCount, int maxThreadWorkerCount, bool isBackground, int idleTime, ThreadPriority priority)
        {
            _minThreadWorkerCount = minThreadWorkerCount;
            _maxThreadWorkerCount = maxThreadWorkerCount;
            _isBackground = isBackground;
            _idleTime = idleTime;
            _priority = priority;
        }

        /// <summary>
        /// 获取默认设置
        /// minThreadWorkerCount: 10
        /// maxThreadWorkerCount: 100
        /// isBackground: false
        /// idleTime: 600000 十分钟
        /// priority: Normal
        /// </summary>
        public static ThreadWorkerPoolSettings Default
        {
            get
            {
                return new ThreadWorkerPoolSettings(10, 100, false, 60 * 1000 * 10, ThreadPriority.Normal);
            }
        }

        /// <summary>
        /// 获取或设置最小线程数
        /// </summary>
        public int MinThreadWorkerCount
        {
            get { return _minThreadWorkerCount; }
            set { _minThreadWorkerCount = value; }
        }

        /// <summary>
        /// 获取或设置最大线程数
        /// </summary>
        public int MaxThreadWorkerCount
        {
            get { return _maxThreadWorkerCount; }
            set { _maxThreadWorkerCount = value; }
        }

        /// <summary>
        /// 获取或设置一个标识，表示线程是否在后台运行
        /// </summary>
        public bool IsBackground
        {
            get { return _isBackground; }
            set { _isBackground = value; }
        }

        /// <summary>
        /// 获取或设置线程空闲时间（毫秒）,空闲时间到期后将自动被销毁， -1表示永不到期（不建议）
        /// </summary>
        public int IdleTime
        {
            get { return _idleTime; }
            set { _idleTime = value; }
        }

        /// <summary>
        /// 获取或设置线程优先级
        /// </summary>
        public ThreadPriority Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
    }
}
