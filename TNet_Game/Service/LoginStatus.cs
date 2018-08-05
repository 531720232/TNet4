using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Service
{
    /// <summary>
    /// Login status.
    /// </summary>
    public enum LoginStatus
    {
        /// <summary>
        /// 未登录
        /// </summary>
        NoLogin = 0,
        /// <summary>
        /// 已登录(重复的)
        /// </summary>
        Logined,
        /// <summary>
        /// 登录成功
        /// </summary>
        Success,
        /// <summary>
        /// 登录超时
        /// </summary>
        Timeout,
        /// <summary>
        /// 退出登录
        /// </summary>
        Exit
    }
}
