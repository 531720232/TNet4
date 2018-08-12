using System;
using System.Net.Sockets;

namespace TNet.RPC.Sockets
{
    public abstract class ISocket
    {
        /// <summary>
        /// not proccess buildpack
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="callback"></param>
        protected internal abstract bool SendAsync(TNSocket socket, byte[] buffer, Action<SocketAsyncResult> callback);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public abstract void PostSend(TNSocket socket, byte[] data, int offset, int count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public abstract void PostSend(TNSocket socket, sbyte opCode, byte[] data, int offset, int count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        public abstract void PostSend(TNSocket socket, sbyte opCode, byte[] data, int offset, int count, Action<SocketAsyncResult> callback);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public abstract void Ping(TNSocket socket);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public abstract void Pong(TNSocket socket);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="reason"></param>
        public abstract void CloseHandshake(TNSocket socket, string reason);

        /// <summary>
        /// has trigger CloseHandshake method
        /// </summary>
        /// <param name="ioEventArgs"></param>
        /// <param name="opCode"></param>
        /// <param name="reason"></param>
        protected internal abstract void Closing(SocketAsyncEventArgs ioEventArgs, sbyte opCode, string reason);


    }
}