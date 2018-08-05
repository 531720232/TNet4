using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Service
{
    /// <summary>
    /// 通讯协议版本
    /// </summary>
    public enum ProtocolVersion
    {
        /// <summary>
        /// default
        /// </summary>
        Default = 0,
        /// <summary>
        /// support head extend for sync property to client
        /// </summary>
        ExtendHead = 1
    }
}
