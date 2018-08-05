using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.Sockets.Events
{
    public class SocketEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public DataMessage Source { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data
        {
            get { return Source.Data; }
            set
            {
                if (Source == null)
                {
                    Source = new DataMessage() { Data = value };
                }
                else
                {
                    Source.Data = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TNSocket Socket { get; set; }

        /// <summary>
        /// The empty.
        /// </summary>
        public new static SocketEventArgs Empty = new SocketEventArgs();
    }
}
