using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TNet.Contract;
using TNet.Extend;
using TNet.RPC.Sockets;
using TNet.RPC.Sockets.Events;
using TNet.Service;

namespace TNet.Coms.FF
{
    /// <summary>
    /// 句柄分发
    /// </summary>
  public  interface IHandlerDispatcher
    {
        bool TryDePack(TNet.RPC.Sockets.Events.ConnectionEventArgs e, out TNet.Contract.RequestPackage request);
        bool TryDePack(System.Net.HttpListenerContext e, out TNet.Contract.RequestPackage request);
     
        ActionGetter GetActionGetter(RequestPackage package, GameSession session);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="actionGetter"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorInfo"></param>
        void ResponseError(BaseGameResponse response, ActionGetter actionGetter, int errorCode, string errorInfo);
    }

  
    /// <summary>
    /// 句柄分发
    /// </summary>
    public class TN_HandlerDispatcher : IHandlerDispatcher
    {
        public ActionGetter GetActionGetter(RequestPackage e, GameSession session)
        {

            return null;
        }
        /// </summary>
        /// <param name="packageReader"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        protected virtual bool TryBuildPackage(PackageReader packageReader, out RequestPackage package)
        {
            package = null;
            Guid proxySid;
            packageReader.TryGetParam("ssid", out proxySid);
            int actionid;
            if (!packageReader.TryGetParam("actionid", out actionid))
            {
                return false;
            }
            int msgid;
            if (!packageReader.TryGetParam("msgid", out msgid))
            {
                return false;
            }
            int userId;
            packageReader.TryGetParam("uid", out userId);
            string sessionId;
            string proxyId;
            int ptcl;
            packageReader.TryGetParam("sid", out sessionId);
            packageReader.TryGetParam("proxyId", out proxyId);
            packageReader.TryGetParam("ptcl", out ptcl);

            package = new RequestPackage(msgid, sessionId, actionid, userId, ptcl.ToEnum<ProtocolVersion>())
            {
                ProxySid = proxySid,
                ProxyId = proxyId,
                IsProxyRequest = packageReader.ContainsKey("isproxy"),
                RouteName = packageReader.RouteName,
                IsUrlParam = true,
                Params = packageReader.Params,
                Message = packageReader.InputStream,
                OriginalParam = packageReader.RawParam
            };
            return true;
        }
        public void ResponseError(BaseGameResponse response, ActionGetter actionGetter, int errorCode, string errorInfo)
        {
            throw new NotImplementedException();
        }

        public bool TryDePack(ConnectionEventArgs e, out RequestPackage package)
        {
            var packageReader = new PackageReader(e.Data, Encoding.UTF8);
            if (TryBuildPackage(packageReader, out package))
            {
                package.OpCode = e.Message.OpCode;
                package.CommandMessage = e.Socket.IsWebSocket && e.Message.OpCode == OpCode.Text
                    ? e.Message.Message
                    : null;
                return true;
            }
            return false;
        }

        public bool TryDePack(HttpListenerContext context, out RequestPackage package)
        {
           
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

           
            int sc;

            throw new NotImplementedException();
        }
    }
}
