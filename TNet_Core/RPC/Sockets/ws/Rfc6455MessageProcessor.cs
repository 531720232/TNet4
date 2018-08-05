using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.Sockets.ws
{
    /// <summary>
    /// 
    /// </summary>
    public class Rfc6455MessageProcessor : Hybi10MessageProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        public Rfc6455MessageProcessor()
        {
            CloseStatusCode = new Rfc6455CloseStatusCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected override bool IsValidCloseCode(int code)
        {
            var closeCode = this.CloseStatusCode as Rfc6455CloseStatusCode;
            if (closeCode == null) return false;

            if (code >= 0 && code <= 999)
                return false;

            if (code >= 1000 && code <= 2999)
            {
                if (code == closeCode.NormalClosure
                    || code == closeCode.GoingAway
                    || code == closeCode.ProtocolError
                    || code == closeCode.UnexpectedCondition
                    //|| code == closeCode.Reserved
                    //|| code == closeCode.NoStatusRcvd
                    || code == closeCode.AbnormalClosure
                    || code == closeCode.InvalidUTF8
                    || code == closeCode.PolicyViolation
                    || code == closeCode.MessageTooBig
                    || code == closeCode.MandatoryExt
                    || code == closeCode.InternalServerError
                    || code == closeCode.TLSHandshake)
                {
                    return true;
                }

                return false;
            }

            if (code >= 3000 && code <= 4999)
                return true;

            return false;
        }
    }
}
