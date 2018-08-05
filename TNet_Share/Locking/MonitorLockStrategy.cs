using System;
using System.Collections.Generic;
using System.Text;
using TNet.Log;

namespace TNet.Locking
{
    /// <summary>
    /// Monitor锁策略
    /// </summary>
    public class MonitorLockStrategy : IMonitorStrategy
    {
        private readonly object _syncRoot = new object();
        private readonly int _timeOut;
        /// <summary>
        /// 
        /// </summary>
        public MonitorLockStrategy()
            : this(1000)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeOut"></param>
        public MonitorLockStrategy(int timeOut)
        {
            _timeOut = timeOut > 0 ? timeOut : 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool TryEnterLock(LockCallback handle)
        {
            using (var l = Lock())
            {
                if (l != null && l.TryEnterLock())
                {
                    if (handle != null)
                    {
                        handle();
                    }
                }
                else
                {
                    TraceLog.WriteError("Monitor lock timeout:{0}", _timeOut);
                }
                return l != null && l.IsLocked;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILocking Lock()
        {
            return new MonitorSlim(_syncRoot, _timeOut);
        }
    }
}
