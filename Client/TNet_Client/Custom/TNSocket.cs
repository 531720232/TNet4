using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TNet.RPC.Sockets.ws;

namespace TNet.RPC.Sockets
{
    /// <summary>
    /// TNET套接字（KCP+WS）
    /// </summary>
    public class TNSocket
    {
        private System.Net.Sockets.Socket socket;
        private System.Net.IPEndPoint remoteEndPoint;
        private Queue<SocketAsyncResult> sendQueue;
        private int isInSending;
        internal DateTime LastAccessTime;
        public TNSocket(System.Net.Sockets.Socket socket)
        {
            HashCode = Guid.NewGuid();
            sendQueue = new Queue<SocketAsyncResult>();
            this.socket = socket;
            InitData();
        }
        private void InitData()
        {
            try
            {
                remoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            }
            catch (Exception)
            {
            }
        }

        public Guid HashCode { get; private set; }
        /// <summary>
        /// Is closed flag.
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Is connected of socket
        /// </summary>
        public bool Connected { get { return socket.Connected; } }

        /// <summary>
        /// Gets the work socket.
        /// </summary>
        /// <value>The work socket.</value>
        internal Socket WorkSocket { get { return socket; } }
        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>The remote end point.</value>
        public EndPoint RemoteEndPoint { get { return remoteEndPoint; } }
        /// <summary>
        /// Gets the length of the queue.
        /// </summary>
        /// <value>The length of the queue.</value>
        public int QueueLength { get { return sendQueue.Count; } }


        /// <summary>
        /// Web socket handshake data
        /// </summary>
        public HandshakeData Handshake { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsWebSocket { get { return Handshake != null; } }


        /// <summary>
        /// re-connection use.
        /// </summary>
        /// <param name="key"></param>
        public void Reset(Guid key)
        {
            HashCode = key;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            try
            {
                WorkSocket.Shutdown(SocketShutdown.Both);
            }
            catch { }
            WorkSocket.Close();
        }
        internal void ResetSendFlag()
        {
            Interlocked.Exchange(ref isInSending, 0);
        }
        internal bool DirectSendOrEnqueue(byte[] data, Action<SocketAsyncResult> callback)
        {
            lock (socket)
            {
                sendQueue.Enqueue(new SocketAsyncResult(data) { Socket = this, ResultCallback = callback });
                return Interlocked.CompareExchange(ref isInSending, 1, 0) == 0;
            }
        }
        internal bool TryDequeueOrReset(out SocketAsyncResult result)
        {
            lock (socket)
            {
                result = null;
                try
                {
                    result = sendQueue.Dequeue();
                    if (result != null)
                    {
                        return true;
                    }
                    else Interlocked.Exchange(ref isInSending, 0);
                }
                catch
                {

                }
                // if (sendQueue.Dequeue(out result)) return true;
                // else Interlocked.Exchange(ref isInSending, 0);
                return false;
            }
        }
    }
}