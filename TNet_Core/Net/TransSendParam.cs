using System;
using System.Collections.Generic;
using System.Text;
using TNet.Model;

namespace TNet.Net
{
    /// <summary>
    /// 转发器发送参数
    /// </summary>
    public class TransSendParam
    {
        ///<summary>
        ///</summary>
        public TransSendParam()
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="key"></param>
        public TransSendParam(string key)
        {
            Key = key;
        }

        ///<summary>
        /// 
        ///</summary>
        public SchemaTable Schema { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public string Key { get; set; }

        /// <summary>
        /// 更新到库中是全部列，还是改变的列
        /// </summary>
        public bool IsChange { get; set; }

    }
}
