using System.Text;

namespace TNet.RPC.Sockets.ws
{
    /// <summary>
    /// 
    /// </summary>
    public class Rfc6455HandshakeProcessor : Hybi10HandshakeProcessor
    {
        /// <summary>
        /// init
        /// </summary>
        /// <param name="version"></param>
        /// <param name="encoding"></param>
        public Rfc6455HandshakeProcessor(int version, Encoding encoding)
            : base(version, encoding)
        {
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="encoding"></param>
        public Rfc6455HandshakeProcessor(Encoding encoding)
            : base(13, encoding)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Rfc6455HandshakeProcessor()
            : this(Encoding.UTF8)
        {

        }
    }
}