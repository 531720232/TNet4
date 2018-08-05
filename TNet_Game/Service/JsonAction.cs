using System;
using System.Collections.Generic;
using System.Text;
using TNet.RPC.Sockets;

namespace TNet.Service
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JsonAction : BaseStruct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aActionId"></param>
        /// <param name="actionGetter"></param>
        protected JsonAction(int aActionId, ActionGetter actionGetter)
            : base(aActionId, actionGetter)
        {
            IsWebSocket = true;
            actionGetter.OpCode = OpCode.Text;
        }

    }
}
