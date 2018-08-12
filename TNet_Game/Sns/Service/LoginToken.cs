
namespace TNet.Sns.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginToken : ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PassportId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsGuest
        {
            get { return UserType == (int) RegType.Guest; }
    }
    }
}