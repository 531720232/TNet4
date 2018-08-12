using System;

namespace TNet.RPC.Sockets
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