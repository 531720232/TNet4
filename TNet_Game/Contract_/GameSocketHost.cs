using System;
using System.IO;
using System.Net;
using System.Threading;
using TNet.Contract;
using TNet.Lang;
using TNet.Log;
using TNet.RPC.Http;
using TNet.RPC.IO;
using TNet.RPC.Sockets;
using TNet.RPC.Sockets.Events;
using TNet.Service;


using System.Collections.Generic;
using System.Text;
using TNet.Config;
using TNet.Configuration;

namespace TNet.Contract
{
    /// <summary>
    /// 游戏服Socket通讯宿主基类
    /// </summary>
    public abstract class GameSocketHost : GameBaseHost
    {
        //private SmartThreadPool threadPool;
        private SocketListener socketListener;
        private HttpListener httpListener;
        private HttpCDNAddress _httpCdnAddress;

        /// <summary>
        /// Protocol Section
        /// </summary>
        public ProtocolSection GetSection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<ProtocolSection>();
        }

        /// <summary>
        /// The enable http.
        /// </summary>
        protected bool EnableHttp;


        /// <summary>
        /// Sets Address key of http
        /// </summary>
        protected string[] HttpCdnAddressKeys
        {
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _httpCdnAddress = new HttpCDNAddress(value);
            }
        }

        /// <summary>
        /// Action repeater
        /// </summary>
        public IActionDispatcher ActionDispatcher
        {
            get { return _setting == null ? null : _setting.ActionDispatcher; }
            set
            {
                if (_setting != null)
                {
                    _setting.ActionDispatcher = value;
                }
            }
        }

        private Runtime.ZoneSetting _setting;

        /// <summary>
        /// 
        /// </summary>
        protected GameSocketHost()
            : this(new RequestHandler(new MessageHandler()))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected GameSocketHost(RequestHandler requestHandler)
        {
            _setting = Runtime.GameZone.Setting;
            int port = _setting != null ? _setting.GamePort : 0;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            var section = GetSection();
            int maxConnections = section.SocketMaxConnection;
            int backlog = section.SocketBacklog;
            int maxAcceptOps = section.SocketMaxAcceptOps;
            int bufferSize = section.SocketBufferSize;
            int expireInterval = section.SocketExpireInterval;
            int expireTime = section.SocketExpireTime;

            //threadPool = new SmartThreadPool(180 * 1000, 100, 5);
            //threadPool.Start();

            var socketSettings = new SocketSettings(maxConnections, backlog, maxAcceptOps, bufferSize, localEndPoint, expireInterval, expireTime);
            socketListener = new SocketListener(socketSettings, requestHandler);
            socketListener.DataReceived += new ConnectionEventHandler(socketLintener_DataReceived);
            socketListener.Connected += new ConnectionEventHandler(socketLintener_OnConnectCompleted);
            socketListener.Disconnected += new ConnectionEventHandler(socketLintener_Disconnected);

            _httpCdnAddress = new HttpCDNAddress();
            httpListener = new HttpListener();
            var httpHost = section.HttpHost;
            httpHost = "127.0.0.1:2017";
            var httpPort = section.HttpPort;
            var httpName = section.HttpName;

            if (!string.IsNullOrEmpty(httpHost))
            {
                EnableHttp = true;
                var hosts = httpHost.Split(',');
                foreach (var point in hosts)
                {
                    var addressList = point.Split(':');
                    string host = addressList[0];
                    int hport = httpPort;
                    if (addressList.Length > 1)
                    {
                        int.TryParse(addressList[1], out hport);
                    }

                    string address = host.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
                                         ? host
                                         : "http://" + host;
                    httpListener.Prefixes.Add(string.Format("{0}:{1}/{2}/", address, hport, httpName));
                }
            }
        }

        private void socketLintener_OnConnectCompleted(object sender, ConnectionEventArgs e)
        {
            try
            {
                TraceLog.WriteInfo(e.Socket.RemoteEndPoint.ToString());
                var session = GameSession.CreateNew(e.Socket.HashCode, e.Socket, socketListener);
               
                session.HeartbeatTimeoutHandle += OnHeartbeatTimeout;
                OnConnectCompleted(sender, e);
            }
            catch (Exception err)
            {
                TraceLog.WriteError("ConnectCompleted error:{0}", err);
            }
        }

        private void socketLintener_Disconnected(object sender, ConnectionEventArgs e)
        {
            try
            {
                GameSession session = GameSession.Get(e.Socket.HashCode);
                if (session != null)
                {
                    OnDisconnected(session);
                    session.ProxySid = Guid.Empty;
                    session.Close();
                }
            }
            catch (Exception err)
            {
                TraceLog.WriteError("Disconnected error:{0}", err);
            }
        }

        private void socketLintener_DataReceived(object sender, ConnectionEventArgs e)
        {
            try
            {


              
         
             
           
                OnReceivedBefore(e);
                //  socketListener.PostSend(e.Socket, new byte[] { 5, 20 }, 0, 2).Wait();
               
                
               // var reader=new  TNet.IO.EndianBinaryReader(TNet.IO.EndianBitConverter.Little, new System.IO.MemoryStream(e.Data));
             // var packs=  reader.ReadString();
                //PackageReader packagereader = new PackageReader(e.Data, Encoding.UTF8);

                RequestPackage package;
                if (!ActionDispatcher.TryDecodePackage(e, out package))
                {
                    return;
                }
                var session = GetSession(e, package);
                if (CheckSpecialPackge(package, session))
                {
                    return;
                }
                package.Bind(session);
                ProcessPackage(package, session).Wait();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Received to Host:{0} error:{1}", e.Socket.RemoteEndPoint, ex);
            }
        }


        private GameSession GetSession(ConnectionEventArgs e, RequestPackage package)
        {
            //使用代理分发器时,每个ssid建立一个游服Serssion
            GameSession session;
            if (package.ProxySid != Guid.Empty)
            {
                session = GameSession.Get(package.ProxySid) ??
                          (package.IsProxyRequest
                              ? GameSession.Get(e.Socket.HashCode)
                              : GameSession.CreateNew(package.ProxySid, e.Socket, socketListener));
                if (session != null)
                {
                    session.ProxySid = package.ProxySid;
                }
            }
            else
            {
                session = GameSession.Get(package.SessionId) ?? GameSession.Get(e.Socket.HashCode);
            }
            if (session == null)
            {
                session = GameSession.CreateNew(package.ProxySid, e.Socket, socketListener);
            }
            if ((!session.Connected || !Equals(session.RemoteAddress, e.Socket.RemoteEndPoint.ToString())))
            {
                GameSession.Recover(session, e.Socket.HashCode, e.Socket, socketListener);
            }
            session.InitSocket(e.Socket, socketListener);
            return session;
        }

        /// <summary>
        /// Raises the received before event.
        /// </summary>
        /// <param name="e">E.</param>
        protected virtual void OnReceivedBefore(ConnectionEventArgs e)
        {
        }

        private async System.Threading.Tasks.Task ProcessPackage(RequestPackage package, GameSession session)
        {
            if (package == null) return;

            try
            {
                ActionGetter actionGetter;
                byte[] data = new byte[0];
                if (!string.IsNullOrEmpty(package.RouteName))
                {
                    actionGetter = ActionDispatcher.GetActionGetter(package, session);
                    if (CheckRemote(package.RouteName, actionGetter))
                    {
                        MessageStructure response = new MessageStructure();
                        OnCallRemote(package.RouteName, actionGetter, response);
                        data = response.PopBuffer();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    SocketGameResponse response = new SocketGameResponse();
                    response.WriteErrorCallback += ActionDispatcher.ResponseError;
                    actionGetter = ActionDispatcher.GetActionGetter(package, session);
                    DoAction(actionGetter, response);
                    data = response.ReadByte();
                    var byw = System.Text.Encoding.UTF8.GetString(data);
                }
                try
                {
                    if (session != null && data.Length > 0)
                    {
                        await session.SendAsync(actionGetter.OpCode, data, 0, data.Length, OnSendCompleted);
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("PostSend error:{0}", ex);
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Task error:{0}", ex);
            }
            finally
            {
                if (session != null) session.ExitSession();
            }
        }

        /// <summary>
        /// Response hearbeat stream.
        /// </summary>
        /// <param name="package"></param>
        /// <param name="session"></param>
        protected void ResponseHearbeat(RequestPackage package, GameSession session)
        {
            try
            {
                MessageStructure response = new MessageStructure();
                response.WriteBuffer(new MessageHead(package.MsgId, package.ActionId, 0));
                var data = response.PopBuffer();
                if (session != null && data.Length > 0)
                {
                    session.SendAsync(OpCode.Binary, data, 0, data.Length, OnSendCompleted).Wait();
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Post Heartbeat error:{0}", ex);
            }
        }



        /// <summary>
        /// Raises the connect completed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected virtual void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {

        }
        /// <summary>
        /// Send data success
        /// </summary>
        /// <param name="result"></param>
        protected virtual void OnSendCompleted(SocketAsyncResult result)
        {

        }


        #region http server
        private void OnHttpRequest(IAsyncResult result)
        {
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);
                listener.BeginGetContext(OnHttpRequest, listener);
                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.WriteLine("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/><title>测试服务器</title></head><body>");
                    writer.WriteLine("<div style=\"height:20px;color:blue;text-align:center;\"><p> hello</p></div>");
                    writer.WriteLine("<ul>");
                    writer.WriteLine("</ul>");
                    writer.WriteLine("</body></html>");

                }

                string userHostAddress = _httpCdnAddress.GetUserHostAddress(context.Request.RemoteEndPoint,
                    key => context.Request.Headers[key]);
                RequestPackage package;
                if (!ActionDispatcher.TryDecodePackage(context, out package))
                {
                    return;
                }

                GameSession session;
                if (package.ProxySid != Guid.Empty)
                {
                    session = GameSession.Get(package.ProxySid) ?? GameSession.CreateNew(package.ProxySid, context.Request);
                    session.ProxySid = package.ProxySid;
                }
                else
                {
                    session = (string.IsNullOrEmpty(package.SessionId)
                            ? GameSession.GetSessionByCookie(context.Request)
                            : GameSession.Get(package.SessionId))
                        ?? GameSession.CreateNew(Guid.NewGuid(), context.Request);
                }
                session.RemoteAddress = userHostAddress;
                package.Bind(session);
             
                ActionGetter httpGet = ActionDispatcher.GetActionGetter(package, session);
                if (package.IsUrlParam)
                {
                    httpGet["UserHostAddress"] = session.RemoteAddress;
                    httpGet["ssid"] = session.KeyCode.ToString("N");
                    httpGet["http"] = "1";
                }
                //set cookie
                var cookie = context.Request.Cookies["sid"];
                if (cookie == null)
                {
                    cookie = new Cookie("sid", session.SessionId);
                    cookie.Expires = DateTime.Now.AddMinutes(5);
                    context.Response.SetCookie(cookie);
                }


                var httpresponse = new SocketGameResponse();
                httpresponse.WriteErrorCallback += new ScutActionDispatcher().ResponseError;

                var clientConnection = new HttpClientConnection
                {
                    Context = context,
                    Session = session,
                    ActionGetter = httpGet,
                    GameResponse = httpresponse
                };
                var section = GetSection();
                clientConnection.TimeoutTimer = new Timer(OnHttpRequestTimeout, clientConnection, section.HttpRequestTimeout, Timeout.Infinite);
                byte[] respData = new byte[0];
                if (!string.IsNullOrEmpty(package.RouteName))
                {
                    if (CheckRemote(package.RouteName, httpGet))
                    {
                        MessageStructure response = new MessageStructure();
                        OnCallRemote(package.RouteName, httpGet, response);
                        respData = response.PopBuffer();
                    }
                }
                else
                {
                    DoAction(httpGet, httpresponse);
                    respData = httpresponse.ReadByte();
                }
                OnHttpResponse(clientConnection, respData, 0, respData.Length);

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnHttpRequest error:{0}", ex);
            }
        }

        private void OnHttpRequestTimeout(object state)
        {
            try
            {
                HttpClientConnection clientConnection = (HttpClientConnection)state;
                if (clientConnection == null) return;
                var actionGetter = clientConnection.ActionGetter;
                clientConnection.GameResponse.WriteError(actionGetter, Language.Instance.ErrorCode, "Request Timeout.");
                byte[] respData = clientConnection.GameResponse.ReadByte();
                OnHttpResponse(clientConnection, respData, 0, respData.Length);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnHttpRequestTimeout:{0}", ex);
            }
        }

        private void OnHttpResponse(HttpClientConnection connection, byte[] data, int offset, int count)
        {
            try
            {
                connection.TimeoutTimer.Dispose();
                HttpListenerResponse response = connection.Context.Response;
                //response.ContentType = "text/html";
                response.ContentType = "application/octet-stream";
                if (data[offset] == 0x1f && data[offset + 1] == 0x8b && data[offset + 2] == 0x08 && data[offset + 3] == 0x00)
                {
                    response.AddHeader("Content-Encoding", "gzip");
                }
                response.AddHeader("Access-Control-Allow-Origin", "*");
                response.ContentLength64 = count;
                Stream output = response.OutputStream;
                output.Write(data, offset, count);
                output.Close();
                connection.Close();
            }
            catch
            {

            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public override void Start(string[] args)
        {
            socketListener.StartListen();
          
            if (EnableHttp)
            {
                httpListener.Start();
                foreach (var prefix in httpListener.Prefixes)
                {
                    TraceLog.WriteLine("{0} Http service:{1} is started.", DateTime.Now.ToString("HH:mm:ss"), prefix.TrimEnd('/'));
                }
                httpListener.BeginGetContext(OnHttpRequest, httpListener);
            }
            EntitySyncManger.SendHandle += (userId, data) =>
            {
                GameSession session = GameSession.Get(userId);
                if (session != null)
                {
                    var task = session.SendAsync(OpCode.Binary, data, 0, data.Length, result => { });
                    task.Wait();
                    return task.Result;
                }
                return false;
            };
            TraceLog.WriteLine("{0} Socket service {1}:{2} is started.", DateTime.Now.ToString("HH:mm:ss"), _setting.GameIpAddress, _setting.GamePort);
            base.Start(args);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            if (EnableHttp)
            {
                httpListener.Stop();
            }
            socketListener.Dispose();
            try
            {
                OnServiceStop();
                //threadPool.Dispose();
                EntitySyncManger.Dispose();
                //threadPool = null;
            }
            catch
            {
            }
        }


        private class HttpClientConnection
        {
            public GameSession Session;
            public HttpListenerContext Context;
            public Timer TimeoutTimer;
            public ActionGetter ActionGetter;
            public SocketGameResponse GameResponse;
            public void Close()
            {
            }
        }
    }
}
