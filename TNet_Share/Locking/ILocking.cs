using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Locking
{
    /// <summary>
    /// 锁回调委托
    /// </summary>
    public delegate void LockCallback();

    /// <summary>
    /// 锁操作接口
    /// </summary>
    public interface ILocking : IDisposable
    {
        /// <summary>
        /// 是否已锁成功
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// 尝试进入锁
        /// </summary>
        /// <returns></returns>
        bool TryEnterLock();
    }
}
