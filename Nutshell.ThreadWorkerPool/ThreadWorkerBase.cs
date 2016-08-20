using Nutshell.ThreadWorkerPool.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace Nutshell.ThreadWorkerPool
{
    public abstract class ThreadWorkerBase
    {
        private readonly Thread _thread;
        private readonly int _idleTime;
        private readonly AutoResetEvent _waitEvent;
        private volatile ThreadWorkerStatus _status;
        private bool _isCanIdleExpired = true;
        protected readonly TraceSource _logger = new TraceSource("Nutshell");

        internal event EventHandler OnIdleExpired;
        internal event EventHandler OnWorkCompleted;

        public ThreadWorkerBase(string name, ThreadPriority priority, bool isBackground, int idleTime)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _thread = new Thread(ThreadWorking);
            _thread.Name = name;
            _thread.Priority = priority;
            _thread.IsBackground = isBackground;
            _idleTime = idleTime;
            _waitEvent = new AutoResetEvent(false);
            _status = ThreadWorkerStatus.Created;
        }

        internal bool IsCanIdleExpried
        {
            get { return _isCanIdleExpired; }
            set { _isCanIdleExpired = value; }
        }

        internal void Start()
        {
            if (_status != ThreadWorkerStatus.Created)
                return;

            _thread.Start();
            _status = ThreadWorkerStatus.Idle;
        }

        internal ThreadWorkerStatus Status
        {
            get { return _status; }
        }

        public void Work()
        {
            if (_status == ThreadWorkerStatus.Abort)
                throw new InvalidOperationException("this ThreadWorker was Abort!");

            if (_status == ThreadWorkerStatus.Working)
                throw new InvalidOperationException("this ThreadWorker was working, unable to duplicate work!");

            _status = ThreadWorkerStatus.Working;
            _waitEvent.Set(); //通知线程有个新的工作要开始
        }

        protected abstract void Working();

        private void ThreadWorking()
        {
            while (_status != ThreadWorkerStatus.Abort)
            {
                //WaitOne 返回false表示等待超时，true接到取消等待的通知
                //这里利用AutoResetEvent.WaitOne的特性来设计闲时控制，false表示空闲到期，true表示新的任务开始
                if (!_waitEvent.WaitOne(_idleTime)) 
                {
                    if (!_isCanIdleExpired) //_isCanIdleExpired变量控制是否允许超时，例如被取出后将不能超时
                        continue;

                    _status = ThreadWorkerStatus.Abort;
                    _waitEvent.Close();
                    _waitEvent.Dispose();
                    if (OnIdleExpired != null)
                        OnIdleExpired(this, null); //空闲到期事件通知
                    return;
                }
                else if (_status == ThreadWorkerStatus.Abort)
                    return;

                try
                {
                    Working();
                }
                catch (Exception ex)
                {
                    _logger.TraceEvent(TraceEventType.Error, (int)TraceEventType.Error, ex.ToString());
                }
                finally
                {
                    _status = ThreadWorkerStatus.Idle;
                    if (OnWorkCompleted != null)
                        OnWorkCompleted(this, null); //任务完成事件通知
                }
            }
        }

        internal void Abort()
        {
            if (_status != ThreadWorkerStatus.Abort)
            {
                _status = ThreadWorkerStatus.Abort;
                _waitEvent.Set();
                _waitEvent.Close();
                _waitEvent.Dispose();
                _thread.Join();
            }
        }
    }
}
