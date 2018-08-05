using System;
using System.Collections.Generic;
using System.Text;
using TNet.RPC.Sockets;

namespace TNet.Service
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BinaryAction : BaseStruct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aActionId"></param>
        /// <param name="actionGetter"></param>
        protected BinaryAction(int aActionId, ActionGetter actionGetter)
            : base(aActionId, actionGetter)
        {
            actionGetter.OpCode = OpCode.Binary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public override void WriteResponse(BaseGameResponse response)
        {
            byte[] buffer = BuildResponsePack();
            response.BinaryWrite(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract byte[] BuildResponsePack();
    }
}
