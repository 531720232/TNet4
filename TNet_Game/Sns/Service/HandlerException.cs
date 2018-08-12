
using System;

namespace TNet.Sns.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class HandlerException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="error"></param>
        public HandlerException(StateCode stateCode, string error)
            : base(error)
        {
            StateCode = stateCode;
        }
        /// <summary>
        /// 
        /// </summary>
        public StateCode StateCode { get; set; }

    }
}