using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.Sockets.Events
{
    /// <summary>
    /// Connection event handler.
    /// </summary>
    public delegate void ConnectionEventHandler(ISocket socket, ConnectionEventArgs e);

    /// <summary>
    /// Connection event arguments.
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the socket.
        /// </summary>
        /// <value>The socket.</value>
        public TNSocket Socket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DataMessage Message { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data
        {
            get { return Message != null ? Message.Data : null; }
            set
            {
                if (Message == null)
                {
                    Message = new DataMessage() { Data = value };
                }
                else
                {
                    Message.Data = value;
                }
            }
        }
    }
}
