using System.Collections.Generic;
using System.Net;

namespace TNet.RPC.Sockets
{
    /// <summary>
    /// Client socket settings.
    /// </summary>
    public class ClientSocketSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TNet.RPC.Sockets.ClientSocketSettings"/> class.
        /// </summary>
        /// <param name="bufferSize">Buffer size.</param>
        /// <param name="remoteEndPoint">Remote end point.</param>
        public ClientSocketSettings(int bufferSize, IPEndPoint remoteEndPoint)
        {
            this.BufferSize = bufferSize;
            this.RemoteEndPoint = remoteEndPoint;
        }
        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        /// <value>The size of the buffer.</value>
        public int BufferSize { get; private set; }
        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>The remote end point.</value>
		public IPEndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Scheme { get; set; }
        /// <summary>
        /// 
        /// </summary>
	    public string UrlPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Extensions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 
        /// </summary>
	    public string SecWebSocketExtensions { get; set; }

        /// <summary>
        /// Client cookies.
        /// </summary>
        public Dictionary<string, string> Cookies { get; set; }

    }
}