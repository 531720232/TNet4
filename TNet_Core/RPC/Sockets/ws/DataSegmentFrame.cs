using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.Sockets.ws
{
    /// <summary>
    /// 
    /// </summary>
    public class DataSegmentFrame
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageHeadFrame Head { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ArraySegment<byte> Data { get; set; }
    }
}
