using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.Http
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpRequestContext
    {
        /// <summary>
        /// 
        /// </summary>
        IHttpAsyncHostHandlerContext HostContext { get; }
        /// <summary>
        /// 
        /// </summary>
       System.Net.HttpListenerRequest Request { get; }
        /// <summary>
        /// 
        /// </summary>
      System.Security.Principal.IPrincipal User { get; }
        /// <summary>
        /// 
        /// </summary>
        string UserHostAddress { get; }
    }
}
