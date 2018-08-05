using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TNet.Locking
{
    internal class MonitorSlim : ILocking
    {
        private readonly object _lockObject;
        private readonly int _timeOut;

        public MonitorSlim(object syncRoot, int timeOut)
        {
            _lockObject = syncRoot;
            _timeOut = timeOut;
        }

        public bool IsLocked
        {
            get;
            private set;
        }

        public bool TryEnterLock()
        {
            if (_timeOut == 0)
            {
                Monitor.Enter(_lockObject);
                IsLocked = true;
            }
            else
            {
                IsLocked = Monitor.TryEnter(_lockObject, _timeOut);
            }
            return IsLocked;
        }

        public void Dispose()
        {
            if (IsLocked)
            {
                IsLocked = false;
                Monitor.Exit(_lockObject);
            }
        }

    }
}
