using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TNet.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public RequestHandler(BaseMessageProcessor messageProcessor)
        {
            MessageProcessor = messageProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appServer"></param>
        internal virtual void Bind(ISocket appServer)
        {
            AppServer = appServer;
        }

        /// <summary>
        /// 
        /// </summary>
        public ISocket AppServer { get; private set; }


        /// <summary>
        /// websocket schema is wss, need use sub protocol
        /// </summary>
        public bool IsSecurity { get; set; }


        /// <summary>
        /// 
        /// </summary>
        internal protected BaseMessageProcessor MessageProcessor { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <param name="encoding"></param>
        /// <param name="callback"></param>
        public void SendMessage(TNSocket socket, string message, Encoding encoding, Action<SocketAsyncResult> callback)
        {
            byte[] buffer = encoding.GetBytes(message);
            SendMessage(socket, buffer, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="callback"></param>
        public void SendMessage(TNSocket socket, byte[] buffer, Action<SocketAsyncResult> callback)
        {
            AppServer.SendAsync(socket, buffer, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool SendHandshake(SocketAsyncEventArgs ioEventArgs)
        {
            //not handshake data
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ioEventArgs"></param>
        /// <param name="messages"></param>
        /// <param name="hasHandshaked"></param>
        /// <returns></returns>
        public virtual bool TryReceiveMessage(SocketAsyncEventArgs ioEventArgs, out List<DataMessage> messages, out bool hasHandshaked)
        {
            messages = new List<DataMessage>();
            hasHandshaked = false;
            try
            {
                var dataToken = ioEventArgs.UserToken as DataToken;
                if (dataToken == null) return false;

                byte[] buffer = new byte[ioEventArgs.BytesTransferred];
                System.Buffer.BlockCopy(ioEventArgs.Buffer, dataToken.DataOffset, buffer, 0, buffer.Length);

                if (MessageProcessor != null)
                {
                    if (!MessageProcessor.TryReadMeaage(dataToken, buffer, out messages))
                    {
                        AppServer.Closing(ioEventArgs, OpCode.Close, "read data fail");
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("TryReceiveMessage error:{0}", ex);
            }
            return true;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        public virtual void SendCloseHandshake(TNSocket socket, sbyte opCode, string reason)
        {
            if (MessageProcessor != null)
            {
                byte[] data = MessageProcessor.CloseMessage(socket, opCode, reason);
                if (data != null) AppServer.SendAsync(socket, data, result => { });
            }
        }
    }
}