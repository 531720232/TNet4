using System;

namespace TNet.Sns
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SnsUser
    {
        /// <summary>
        /// 微信号OpenID
        /// </summary>
        public string WeixinCode
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string IMEI { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PassportId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public RegType RegType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PwdType PwdType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RetailID
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RetailUser
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mail
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RealName
        {
            get;
            set;
        }
        /// <summary>
        /// 身份ID
        /// </summary>
        public string IDCards
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RegTime
        {
            get;
            set;
        }
        /// <summary>
        /// 激活码
        /// </summary>
        public string ActiveCode
        {
            get;
            set;
        }
        /// <summary>
        /// 发送激活码时间
        /// </summary>
        public DateTime SendActiveDate
        {
            get;
            set;
        }
        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime ActiveDate
        {
            get;
            set;
        }
    }
}