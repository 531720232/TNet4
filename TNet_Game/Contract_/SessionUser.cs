using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using TNet.Context_;
using TNet.Extend;

namespace TNet.Contract
{
    /// <summary>
    /// session's user
    /// </summary>
    [ProtoContract]
    public class SessionUser : IUser
    {
        /// <summary>
        /// 
        /// </summary>
        public SessionUser()
        {
            OnlineInterval = new TimeSpan(0, 1, 0);
            RefleshOnlineDate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleUser"></param>
        public SessionUser(BaseUser roleUser)
            : this()
        {
            PassportId = roleUser.GetPassportId();
            UserId = roleUser.GetUserId();
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(2)]
        public string PassportId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(3)]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOnlining { get { return MathUtils.DiffDate(OnlineDate) < OnlineInterval; } }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(4)]
        public DateTime OnlineDate { get; set; }


        /// <summary>
        /// 在线间隔
        /// </summary>
        [ProtoMember(5)]
        public TimeSpan OnlineInterval { get; set; }

        /// <summary>
        /// 被替换掉
        /// </summary>
        [ProtoMember(6)]
        public bool IsReplaced { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetUserId()
        {
            return UserId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPassportId()
        {
            return PassportId;
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefleshOnlineDate()
        {
            OnlineDate = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetExpired(DateTime time)
        {
            OnlineDate = time;
        }
    }
}
