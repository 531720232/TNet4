using System;
using System.Collections.Generic;
using System.Text;
using TNet.Log;

namespace TNet.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 
        /// </summary>
        Wait,
        /// <summary>
        /// 
        /// </summary>
        Success,
        /// <summary>
        /// 
        /// </summary>
        Close,
        /// <summary>
        /// 
        /// </summary>
        Error
    }
    /// <summary>
    /// Socket send async result
    /// </summary>
    public class SocketAsyncResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public SocketAsyncResult(byte[] data)
        {
            Result = ResultCode.Wait;
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        public TNSocket Socket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected internal Action<SocketAsyncResult> ResultCallback;

        /// <summary>
        /// 
        /// </summary>
        public ResultCode Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal void Callback()
        {
            if (ResultCallback != null)
            {
                try
                {
                    ResultCallback(this);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("ResultCallback error{0}", ex);
                }
            }
        }
    }
}
