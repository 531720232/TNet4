using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.IO
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PacketBaseHead
    {
        internal int PacketLength { get; set; }

        /// <summary>
        /// to bytes.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToBytes();
    }

    ///<summary>
    ///</summary>
    public class PacketHead : PacketBaseHead
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override byte[] ToBytes()
        {
            return new byte[0];
        }
    }
}
