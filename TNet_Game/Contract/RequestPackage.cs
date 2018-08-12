using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using TNet.Service;

namespace TNet.Contract
{
    [MessagePack.MessagePackObject]
    /// <summary>
    /// Request message package
    /// </summary>
    [ProtoContract, Serializable]
    public class RequestPackage
    {
        /// <summary>
        /// init
        /// </summary>
        public RequestPackage()
        {

        }
        /// <summary>
        /// init
        /// </summary>
        public RequestPackage(int msgId, string sessionId, int actionId, int userId, ProtocolVersion ptcl = ProtocolVersion.Default)
        {
            MsgId = msgId;
            SessionId = sessionId;
            ProxySid = Guid.Empty;
            ActionId = actionId;
            UserId = userId;
            Ptcl = ptcl;
        }


        [MessagePack.Key(0)]
        public DataNode Aode { get; set; }

        /// <summary>
        /// 
        /// </summary>
     
        [MessagePack.IgnoreMember]
        public Dictionary<string, string> Params { get; set; }

        /// <summary>
        /// message id of client request
        /// </summary>
        [ProtoMember(1)]
        [MessagePack.Key(1)]
        public int MsgId { get; protected set; }
        /// <summary>
        /// Action route key
        /// </summary>
        [ProtoMember(2)]
        [MessagePack.Key(2)]
        public int ActionId { get; protected set; }

        /// <summary>
        /// 服务器间内部通讯通道
        /// </summary>
        [ProtoMember(3)]
        [MessagePack.Key(3)]
        public string RouteName { get; set; }
        /// <summary>
        /// session id of client
        /// </summary>
        [ProtoMember(4)]
        [MessagePack.Key(4)]
        public string SessionId { get; protected set; }

        /// <summary>
        /// session id of client
        /// </summary>
        [ProtoMember(5)]
        [MessagePack.Key(5)]
        public int UserId { get; protected set; }

        /// <summary>
        /// is proxy server connect
        /// </summary>
        [ProtoMember(6)]
        [MessagePack.Key(6)]
        public bool IsProxyRequest { get; set; }

        /// <summary>
        /// Proxy server connect sid
        /// </summary>
        [ProtoMember(7)]
        [MessagePack.Key(8)]
        public Guid ProxySid { get; set; }

        /// <summary>
        /// 是否是Url格式参数类型
        /// </summary>
        [ProtoMember(8)]
        [MessagePack.Key(9)]
        public bool IsUrlParam { get; internal protected set; }

        //注释原因：重复解析参数字串
        ///// <summary>
        ///// param for url request
        ///// </summary>
        //[ProtoMember(9)]
        //public string UrlParam { get; set; }

        /// <summary>
        /// 远程代理客户端的标识ID
        /// </summary>
        [ProtoMember(10)]
        [MessagePack.Key(10)]
        public string ProxyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(11)]
        [MessagePack.Key(11)]
        public sbyte OpCode { get; set; }


        [MessagePack.IgnoreMember]
        /// <summary>
        /// 通讯协议版本，0: default, 1: use head extend for sync property to client
        /// </summary>
        public ProtocolVersion Ptcl { get; set; }

        /// <summary>
        /// sigin use
        /// </summary>
        [MessagePack.IgnoreMember]
        public string OriginalParam { get; set; }
        [MessagePack.IgnoreMember]
        /// <summary>
        /// websocket use
        /// </summary>
        public string CommandMessage { get; set; }
        [MessagePack.IgnoreMember]
        /// <summary>
        /// Message of custom
        /// </summary>
        public object Message { get; set; }

        ///// <summary>
        ///// GameSession
        ///// </summary>
        //[JsonIgnore]
        //public GameSession Session { get; protected set; }
        [MessagePack.IgnoreMember]
        /// <summary>
        /// Receive time
        /// </summary>
        public DateTime ReceiveTime { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public void Bind(GameSession session)
        {
            if (session != null)
            {
                SessionId = session.SessionId;
                session.ProxyId = ProxyId;
                //push not refresh
                if (MsgId > 0) session.Refresh();
            }
            //Session = session;
            ReceiveTime = DateTime.Now;
        }

        //internal string ToJson()
        //{
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        //}
    }
}
