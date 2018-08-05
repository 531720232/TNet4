using System;
using System.Collections.Generic;
using System.Text;
using TNet.Log;
using TNet.RPC.IO;

namespace TNet.Service
{
    /// <summary>
    /// Remote struct
    /// </summary>
    public abstract class RemoteStruct
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly ActionGetter paramGetter;
        /// <summary>
        /// 
        /// </summary>
        protected readonly MessageStructure response;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="paramGetter"></param>
        /// <param name="response"></param>
        protected RemoteStruct(ActionGetter paramGetter, MessageStructure response)
        {
            this.paramGetter = paramGetter;
            this.response = response;
        }

        /// <summary>
        /// 
        /// </summary>
        protected int ErrorCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected string ErrorInfo { get; set; }

        /// <summary>
        /// 是否影响输出, True：不响应
        /// </summary>
        protected bool IsNotRespond;

        /// <summary>
        /// 
        /// </summary>
        internal void DoRemote()
        {
            if (Check())
            {
                try
                {
                    TakeRemote();
                    if (!IsNotRespond)
                    {
                        WriteResponse();
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("DoRemote error:{0}", ex);
                    if (!IsNotRespond)
                    {
                        WriteError();
                    }
                }
            }
            else
            {
                if (!IsNotRespond)
                {
                    WriteError();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void WriteResponse()
        {
            BuildPacket();
            WriteRemote();
        }

        private void WriteRemote()
        {
            int msgId = paramGetter.GetMsgId();
            int actionId = paramGetter.GetActionId();
            var head = new MessageHead(msgId, actionId, ErrorCode, ErrorInfo);
            response.WriteBuffer(head);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void WriteError()
        {
            int msgId = paramGetter.GetMsgId();
            int actionId = paramGetter.GetActionId();
            var head = new MessageHead(msgId, actionId, (int)MessageError.SystemError, ErrorInfo);
            response.WriteBuffer(head);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool Check();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract void TakeRemote();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void BuildPacket();

    }
}
