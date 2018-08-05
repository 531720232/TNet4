using System.Collections.Generic;
using TNet.RPC.Sockets.ws;

namespace TNet.RPC.Sockets
{
    /// <summary>
    /// stream reader
    /// </summary>
    public abstract class BaseMessageProcessor
    {
        /// <summary>
        /// Response is encode mask data
        /// </summary>
        public bool IsMask { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CloseStatusCode CloseStatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected BaseMessageProcessor()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataToken"></param>
        /// <param name="buffer"></param>
        /// <param name="messageList"></param>
        /// <returns></returns>
        public abstract bool TryReadMeaage(DataToken dataToken, byte[] buffer, out List<DataMessage> messageList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exSocket"></param>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public abstract byte[] BuildMessagePack(TNSocket exSocket, sbyte opCode, byte[] data, int offset, int count);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exSocket"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        public abstract byte[] CloseMessage(TNSocket exSocket, sbyte opCode, string reason);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int GetCloseStatus(byte[] data)
        {
            if (data == null || data.Length <= 1)
            {
                return OpCode.Empty;
            }
            var code = data[0] * 256 + data[1];

            if (!IsValidCloseCode(code))
            {
                return OpCode.Empty;
            }
            return code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected abstract bool IsValidCloseCode(int code);
    }
}