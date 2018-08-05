using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.Sockets.ws
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CloseStatusCode
    {
        /// <summary>
        /// 
        /// </summary>
        protected CloseStatusCode()
        {
            NormalClosure = 1000;
            GoingAway = 1001;
            ProtocolError = 1002;
            UnexpectedCondition = 1003;
            Reserved = 1004;
            NoStatusRcvd = 1005;
            AbnormalClosure = 1006;
            InvalidUTF8 = 1007;
            PolicyViolation = 1008;
            MessageTooBig = 1009;
            MandatoryExt = 1010;
        }

        /// <summary>
        /// 1000
        /// </summary>
        public int NormalClosure { get; protected set; }
        /// <summary>
        /// 1001
        /// </summary>
        public int GoingAway { get; protected set; }
        /// <summary>
        /// 1002
        /// </summary>
        public int ProtocolError { get; protected set; }
        /// <summary>
        /// 1003
        /// </summary>
        public int UnexpectedCondition { get; protected set; }
        /// <summary>
        /// 1004
        /// </summary>
        public int Reserved { get; protected set; }
        /// <summary>
        /// 1005
        /// </summary>
        public int NoStatusRcvd { get; protected set; }

        /// <summary>
        /// 1006
        /// </summary>
        public int AbnormalClosure { get; protected set; }
        /// <summary>
        /// 1007
        /// </summary>
        public int InvalidUTF8 { get; protected set; }
        /// <summary>
        /// 1008
        /// </summary>
        public int PolicyViolation { get; protected set; }
        /// <summary>
        /// 1009
        /// </summary>
        public int MessageTooBig { get; protected set; }
        /// <summary>
        /// 1010
        /// </summary>
        public int MandatoryExt { get; protected set; }
    }
}
