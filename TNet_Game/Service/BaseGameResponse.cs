using System;

namespace TNet.Service
{
    /// <summary>
    /// 游戏输出接口
    /// </summary>
    public abstract class BaseGameResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public event Action<BaseGameResponse, ActionGetter, int, string> WriteErrorCallback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public abstract void BinaryWrite(byte[] buffer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public abstract void Write(byte[] buffer);

        internal void WriteError(ActionGetter actionGetter, int errorCode, string errorInfo)
        {
            Action<BaseGameResponse, ActionGetter, int, string> handler = WriteErrorCallback;

            if (handler != null) handler(this, actionGetter, errorCode, errorInfo);
        }

    }

}