using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.Sockets
{
    class ThreadSafeStack<T> where T : class
    {
        private ConcurrentStack<T> stack;

        public ThreadSafeStack(int capacity)
        {
            stack = new ConcurrentStack<T>();
        }

        public int Count { get { return stack.Count; } }

        public T Pop()
        {
            T result;
            if (stack.TryPop(out result))
            {
                return result;
            }
            return null;
        }

        public void Push(T item)
        {
            stack.Push(item);
        }
    }
}
