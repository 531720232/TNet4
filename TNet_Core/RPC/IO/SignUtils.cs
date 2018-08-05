using System;
using System.Collections.Generic;
using System.Text;

namespace TNet.RPC.IO
{
    /// <summary>
    /// 签名配置
    /// </summary>
    public sealed class SignUtils
    {
        /// <summary>
        /// The default key of 32 bit.
        /// </summary>
        public const string DefaultKey = "44CAC8ED53714BF18D60C5C7B6296000";


        /// <summary>
        /// md5 encode sign
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeSign(string str, string key = DefaultKey)
        {
            string attachParam = str + key;
            return TNet.Security.CryptoHelper.MD5_Encrypt(attachParam).ToLower();
        }
    }
}
