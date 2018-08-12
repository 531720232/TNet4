using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TNet.Custom
{
    [ProtoContract]
    public class MessagePack
    {
        [ProtoMember(1)]
        public int MsgId
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public int ActionId
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public string SessionId
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public int UserId
        {
            get;
            set;
        }
    }
    [ProtoContract]
    public class ResponsePack
    {
        [ProtoMember(1)]
        public int MsgId
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public int ActionId
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public int ErrorCode
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public string ErrorInfo
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public string St
        {
            get;
            set;
        }
    }
    [ProtoContract]
    public class Request1001Pack
    {
        [ProtoMember(101)]
        public int PageIndex
        {
            get;
            set;
        }

        [ProtoMember(102)]
        public int PageSize
        {
            get;
            set;
        }
    }
}