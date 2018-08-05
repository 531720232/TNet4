using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.Sockets.ws
{
    /// <summary>
    /// 
    /// </summary>
    public class Rfc6455CloseStatusCode : CloseStatusCode
    {
        /// <summary>
        /// 
        /// </summary>
        public Rfc6455CloseStatusCode()
        {
            InternalServerError = 1010;
            TLSHandshake = 1015;
        }

        /// <summary>
        /// 1011 RFC-6455
        /// </summary>
        public int InternalServerError { get; protected set; }
        /// <summary>
        /// 1015 RFC-6455
        /// </summary>
        public int TLSHandshake { get; protected set; }
    }
}
