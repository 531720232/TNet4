using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.Sockets.ws
{
    /// <summary>
    /// 
    /// </summary>
    public class HandshakeItems : Dictionary<string, object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            object obj;
            if (TryGetValue(key, out obj))
            {
                value = (T)obj;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Web socket handshake data
    /// </summary>
    public class HandshakeData
    {
        /// <summary>
        /// init
        /// </summary>
        public HandshakeData()
        {
            UriSchema = "ws";
            ParamItems = new HandshakeItems();
        }

        ///// <summary>
        ///// more then sec-websocket-accept
        ///// </summary>
        //public object WebSocketSignKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsClient { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Handshaked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            get { return string.Format(HandshakeHeadKeys.RespUrl, UriSchema, Host, UrlPath); }
        }

        /// <summary>
        /// ws or wss
        /// </summary>
        public string UriSchema { get; set; }

        /// <summary>
        /// Http request method, GET
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Http request url path
        /// </summary>
        public string UrlPath { get; set; }

        /// <summary>
        /// Client http verion, HTTP/1.1
        /// </summary>
        public string HttpVersion { get; set; }

        /// <summary>
        /// client host
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Client WebSocket Version
        /// </summary>
        public int WebSocketVersion { get; set; }

        /// <summary>
        /// Client request param items.
        /// </summary>
        public HandshakeItems ParamItems { get; set; }

        /// <summary>
        /// Client cookies.
        /// </summary>
        public Dictionary<string, string> Cookies { get; set; }

        ///// <summary>
        ///// versioin 00 secKey3
        ///// </summary>
        //internal byte[] SecKey3 { get; set; }

        /// <summary>
        /// Sec protocol
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QueueString
        {
            get
            {
                int index = UrlPath.IndexOf('?');
                return index > -1 ? UrlPath.Substring(index) : UrlPath;
            }
        }
    }
}
