using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.Sns
{
    /// <summary>
    /// 登录处理接口
    /// </summary>
    public interface ILogin
    {
        /// <summary>
        /// 
        /// </summary>
        string PassportID { get; }
        /// <summary>
        /// 
        /// </summary>
        string UserID { get; }

        /// <summary>
        /// 用户类型
        /// </summary>
        int UserType { get; }
        /// <summary>
        /// 
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string SessionID { get; }

        /// <summary>
        /// 注册通行证
        /// </summary>
        /// <returns></returns>
        string GetRegPassport();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CheckLogin();

    }
}
