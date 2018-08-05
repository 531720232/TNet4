using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.IO
{
    /// <summary>
    /// 接收消息
    /// </summary>
    public abstract class Receiver : IDisposable
    {
        /// <summary>
        /// 对消息解码
        /// </summary>
        public abstract void Decode();

        /// <summary>
        /// 较验签名
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckSign();

        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        public abstract void Process();
        /// <summary>
        /// Releases all resource used by the <see cref="TNet.RPC.IO.Receiver"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="TNet.RPC.IO.Receiver"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="TNet.RPC.IO.Receiver"/> in an unusable state.
        /// After calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="TNet.RPC.IO.Receiver"/> so the garbage collector can reclaim the memory that the
        /// <see cref="TNet.RPC.IO.Receiver"/> was occupying.</remarks>
        public void Dispose()
        {
            DoDispose(true);
        }
        /// <summary>
        /// Dos the dispose.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected virtual void DoDispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管对象
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
        }
    }
}
