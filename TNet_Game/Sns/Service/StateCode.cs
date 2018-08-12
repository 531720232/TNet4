
namespace TNet.Sns.Service
{
    /// <summary>
    /// 
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 
        /// </summary>
        OK = 0,
        /// <summary>
        /// 
        /// </summary>
        Error = 100,
        /// <summary>
        /// 
        /// </summary>
        SignError = 101,
        /// <summary>
        /// 
        /// </summary>
        NoHandler = 102,
        /// <summary>
        /// 
        /// </summary>
        PassworkError = 103,
        /// <summary>
        /// 
        /// </summary>
        NoToken = 105,
        /// <summary>
        /// 
        /// </summary>
        TokenExpired = 106,
        /// <summary>
        /// Request account server Timeout.
        /// </summary>
        Timeout = 107,
        /// <summary>
        /// Parse json error.
        /// </summary>
        ParseError = 108
    }
}