using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TNet.RPC.Sockets
{
  public  class BufferManager
    {
        int capacity;
        byte[] bufferBlock;
        Stack<int> freeIndexPool;
        int currentIndex;
        int saeaSize;

        public BufferManager(int capacity, int saeaSize)
        {
            this.capacity = capacity;
            this.saeaSize = saeaSize;
            this.freeIndexPool = new Stack<int>();
        }

        internal void InitBuffer()
        {
            this.bufferBlock = new byte[capacity];
        }

        internal bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (this.freeIndexPool.Count > 0)
            {
                args.SetBuffer(this.bufferBlock, this.freeIndexPool.Pop(), this.saeaSize);
            }
            else
            {
                if ((capacity - this.saeaSize) < this.currentIndex)
                {
                    return false;
                }
                args.SetBuffer(this.bufferBlock, this.currentIndex, this.saeaSize);
                this.currentIndex += this.saeaSize;
            }
            return true;
        }

        internal void FreeBuffer(SocketAsyncEventArgs args)
        {
            this.freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}

