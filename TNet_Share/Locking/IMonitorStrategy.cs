using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Locking
{

    /// <summary>
    /// Monitor锁策略接口
    /// </summary>
    public interface IMonitorStrategy
    {
        /// <summary>
        /// 尝试进入锁
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        bool TryEnterLock(LockCallback handle);
        /// <summary>
        /// 获取锁操作接口
        /// </summary>
        /// <returns></returns>
        ILocking Lock();
    }
}
